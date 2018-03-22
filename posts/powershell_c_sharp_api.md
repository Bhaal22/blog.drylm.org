<!-- 
.. title: Microsoft PowerShell issues
.. slug: microsoft-powershell-c-sharp
.. date: 2015-07-04 00:00:00 UTC
.. tags: .net, powershell, c#, dotnet
.. link: 
.. description: Microsoft PowerShell problems while running c&#35; applications
.. type: text
-->

Introduction
==

Here at [GSX](http://www.gsx.com) we make a heavy usage of Microsoft Powershell to perform our Monitoring features on 

  * Microsoft Exchange
  * Microsoft Sharepoint
<!-- TEASER_END -->

  * Microsoft Lync

For this particular purpose we use this kind of script :


```
$shell = "http://schemas.microsoft.com/powershell/Microsoft.Exchange"
$target = New-Object Uri("http://serverHostname/Powershell")

$emailusername = "username"
$encrypted = "password" | ConvertTo-SecureString -AsPlainText -Force


$credential = New-Object System.Management.Automation.PsCredential($emailusername, $encrypted)

$i = 0
while ($true)
{
    $connectionInfo = New-Object System.Management.Automation.Runspaces.WSManConnectionInfo($target, $shell, $credential)
    $connectionInfo.AuthenticationMechanism = "Kerberos"


    $Runspace = [RunspaceFactory]::CreateRunspace($connectionInfo)
    $Runspace.Open()

    Write-Host "Step " + $i
    $ps = [PowerShell]::Create()
    Try
    {
        $ps.Runspace = $Runspace
        $ps.Commands.AddCommand("Test-OWAConnectivity")

        $res = $ps.Invoke()

        $i = $i + 1
    }
    Catch [System.Management.Automation.Remoting.PSRemotingTransportException]
    {
        $ErrorMessage = $_.Exception.Message
        $FailedItem = $_.Exception.ItemName

        Write-Host $ErrorMessage
        
        Write-Host $FailedItem
        Write-Host "RunspaceInfo Status = " $ps.Runspace.RunspaceStateInfo
    }
    Finally 
    {
        $ps.Dispose()
        $Runspace.Close()
    }
}
```

This simple scripts connects to the Exchange PowerShell endpoint and perform the query : *[Test-OWAConnectivity](https://technet.microsoft.com/fr-fr/library/aa997682%28v=exchg.141%29.aspx)*

We got the same kind of code running in our tool *GSX Monitor*.

**Issue**

To reproduce this issue, we need to call multiple commands in a row in th eloop body :

```
    $ps.Runspace = $Runspace
    $ps.Commands.AddCommand("Test-OWAConnectivity")

    $res = $ps.Invoke()

    $ps.Commands.Clear()
    $ps.Commands.AddCommand("Test-ActiveSyncConnectivity")

    $res = $ps.Invoke()

    $ps.Commands.Clear()
    $ps.Commands.AddCommand("Test-OWAConnectivity")

    $res = $ps.Invoke()

    $ps.Commands.Clear()
    $ps.Commands.AddCommand("Test-ActiveSyncConnectivity")

    $res = $ps.Invoke()
```

This relies on an assembly provided by Microsoft : *System.Management.Automation* located in the GAC. This is the core PowerShell library deployed on a regular basis by Windows update.
This code was running perfectly for almost 2 or 3 years. Recently we noticed changes. We did not identified when those updates occured. Now this script generates exception on a regular basis :

![](/galleries/powershell/PowerShellIssue.jpg)

As we can see on the screenshot, we got a *PSRemotingTransportException* with a very tricky error message with a SOAP envelop issue. As you can see on the screenshot, the sessin is still in a correct opened state. Those errors occur since 2 or 3 months approximatively. We can reproduce easily on all Remote PowerShell endpoints : Lync, Sharepoint and even on raw wsman endpoint.