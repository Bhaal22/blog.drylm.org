<!-- 
.. title: Skype For Business SDK Prerequisites
.. slug: skype-for-business-sdk-prerequisites
.. date: 2016-10-12 01:00:00 UTC
.. tags: .net, c&#35;, c sharp, microsoft, skype for business, skype, office365, powershell, 2016
.. category: programming
.. link: 
.. description: What are the requirements to develop a c sharp application using Skype For Business 2016 microsoft's APIs?
.. type: text
-->

Introduction
==

It could be a good idea to drive **Skype for business 2016** from a c&#35; application.
There a couple of use cases:
<!-- TEASER_END -->

  * Send automatic notifications from the application to some other people (like an alert sent to an administrator for example)
  * Reply automatically to incoming messages
  * Simulate a user scenario to test **Skype for business** service (connection - send message to a group of people - disconnect)

To develop application driving Skype for business, at some point you have to download:

  * [Lync 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=36824)
  * [Skype For Business 2016](https://www.microsoft.com/en-us/download/details.aspx?id=49440)

Setup
==

For your application to be run correctly you have to setup a couple of registry keys. Once **Skype For Business** is installed, when you launch it, skype process starts with a nice user interface asking you to login.

Let's close this user interface. Every application which wants to drive **Skype For Business** must be sure Skype is setup in the : **UISuppressMode**, e.g. : *No User Interface !*

To achieve that, 2 registry keys must be set :

```
[HKEY_CURRENT_USER\Software\Microsoft\Office\15.0\Lync]
"UISuppressionMode"=dword:00000001

[HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Lync]
"UISuppressionMode"=dword:00000001
```

While playing with c&#35; Skype SDK, I have discovered an issue when the application tried to disconnect.

Skype has a weird behavior which was a kind of impediment in my use case. I had a couple of credentials to test indefinitely, I mean connections with different users to skype.

When you diconnect from Skype, credentials are not cleared at all, they are cached. The next time you try to initiate a connection, those credentials are reused even if you specify other credentials.

I spent a lot of time to figure out what was my problem. When I found it, I looked for a way to clear the cache credentials and I have a found a nice API :

 - ForgetMe : https://msdn.microsoft.com/en-us/library/office/dn378085.aspx
 
 > **Deletes SignIn credentials and cached data**

Nice ! Lets use it ! AND ? .... I have a really nice exception :

```
Unable to cast COM object of type 'Microsoft.Office.Uc.SignInConfigurationClass' to interface type 'Microsoft.Office.Uc.ISignInConfiguration2'.
This operation failed because the QueryInterface call on the COM component for the interface with IID '{61CE9972-C619-4A88-A5D1-D2DFBCD4D2A1}' failed due to the following error:
      No such interface supported (Exception from HRESULT: 0x80004002 (E_NOINTERFACE)).

00039; 2016/04/13 13:17:56 204;                 at System.StubHelpers.StubHelpers.GetCOMIPFromRCW(Object objSrc, IntPtr pCPCMD, IntPtr& ppTarget, Boolean& pfNeedsRelease)
   at Microsoft.Office.Uc.SignInConfigurationClass.ForgetMe(String Uri)
   at Microsoft.Lync.Model.SignInConfiguration.ForgetMe(String uri)
```

Yeah, it seems Microsoft forget a COM component registration when we deploy **Skype For Business 2016**.
Fortunately, here is the registry script to fix it :

[Set SignInConfiguration Keys Script](https://github.com/Bhaal22/skype-for-business-checks/blob/master/registry_scripts/sb4-skype4Business%202016.reg)

Moreover, I have writter a powershell script to check pre requisites for **Lync 2013** and **Skype For Business 2016**

[Skype For Business Pre requisites checks script](https://github.com/Bhaal22/skype-for-business-checks/blob/master/skype_for_business_prerequisites.ps1)

In the next article on this subject, we will explore a bit the c&#35; APIs.

