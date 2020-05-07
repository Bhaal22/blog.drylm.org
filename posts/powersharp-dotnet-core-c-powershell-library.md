<!--
.. title: powersharp: dotnet core C# powershell library
.. slug: powersharp-dotnet-core-c-powershell-library
.. date: 2020-05-07 20:52:26 UTC
.. tags: .net, microsoft, office365, c#, powershell
.. category: programming
.. link: 
.. description: powersharp is a C# library to run powershell script programmatically and extract objects to be processed by another .net process.
.. category: 
.. type: text
-->

Introduction
==

**powersharp** is a dotnet core c# library which has the capability to run powershell scripts and process the resulting objects.

<!-- TEASER_END -->

**powersharp** supports 3 different types of runspaces:

* Local sessions
* Remote Sessions: *Import* or *Enter* session modes

The library runs fine on major operating systems:

* Windows10
* Linux Debian 10
* MacOS X Catalina

Let's have a look at a sample code:

```powershell
Get-Process | Get-Member

Handles                    AliasProperty  Handles = Handlecount
Name                       AliasProperty  Name = ProcessName
NPM                        AliasProperty  NPM = NonpagedSystemMemorySize64
PM                         AliasProperty  PM = PagedMemorySize64
SI                         AliasProperty  SI = SessionId
VM                         AliasProperty  VM = VirtualMemorySize64
...
```

Let's then get 2 fields, **Name** and **VM**

```powershell
Get-Process | Select-Object -Property ProcessName, VM
```

```c#

class Process
{
    [PSMember("Id", typeof(int))]
    public int Id {get; set; }

    [PSMember("Name", typeof(string))]
    public String Name {get; set; }
}

var shell = new LocalShellInfo();

var initializer = new LocalPSSessionInitializer(shell);
var powershell = new PowershellRunner(initializer);

DefaultPSProcessor<Process> processor = new DefaultPSProcessor<Process>(process => 
{
    Console.WriteLine($"{process.Name}: {process.VirtualMemorySize64}");
});

using (var powershell = new PowershellRunner(init))
{
    try
    {
        await powershell.run("Get-Process", processor);
    }
    catch (Exception ex)
    {
        ....
    }
}
```

The output:

```
4001 bash
 4957 bash
 7620 bash
 9321 bash
11630 bash
14585 bash
....
```

This sample outputs on console, but we could imagine to send data to a queue system for logging purpose so that user could make some analytics.

Here I show a sample to run local PowerShell commands, but the library allows user to connect to a remote office365 endpoint to perform scripts over Exchange, Sharepoint, Teams, ....

I plan to release this library as open source in the coming weeks under MIT license and will be available through a nuget repository.

Stay tuned !




