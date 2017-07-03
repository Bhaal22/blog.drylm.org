<!-- 
.. title: Skype For Business ADAL
.. slug: skype-for-business-sdk-adal
.. date: 2017-07-03 01:00:00 UTC
.. tags: skype for business, skype, office365, adal, azure, active directory
.. category: programming
.. link: 
.. description: Issues with Azure Active directory authentication and Skype For Business SDK?
.. type: text
-->

Introduction
==

If you use [Lync 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=36824) it could happen you get some issues when Signing-in to skype.
<!-- TEASER_END -->

When running this [Code](https://github.com/Bhaal22/skype-for-business-checks/blob/master/client/SkypeForBusinessClient.cs), you can have this behavior:
 * when SDK should login, then you have an Internet explorer popup asking for credentials.
 
If you are in this situation, then it looks like your Office365 domain is using Azure Active directory library (*ADAL*)

It depends as well of your Skype for business client:
  
  * Office Client 15.0.[0000-4766].*
  * Office Client 16.0.[0000-4293].*
  * Office Client 16.0.6001.[0000-1032]
  * Office Client 16.0.[6000-6224].*

What is ADAL?
==

ADAL is the acronym for the Active Directory Authentication Library, and, along with OAuth 2.0, it is an underpinning of Modern Authentication. This code library is designed to make secured resources in your directory available to client applications (like Skype for Business) via security tokens. ADAL works with OAuth 2.0 to enable more authentication and authorization scenarios, like Multi-factor Authentication (MFA), and more forms of SAML Auth.

A variety of apps that act as clients can leverage Modern Authentication for help in getting to secured resources. In Skype for Business Server 2015, this technology is used between on-premises clients and on-premises servers in order to give users a proper level of authorization to resources.

Modern Authentication conversations (which are based on ADAL and OAuth 2.0) have some elements in common.

 * There is a client making a request for a resource, in this case, the client is Skype for Business.
 * There is a resource to which the client needs a specific level of access, and this resource is secured by a directory service, in this case the resource is Skype for Business Server 2015.
 * There is an OAuth connection, in other words, a connection that is dedicated to authorizing a user to access a resource. (OAuth is also known by the more descriptive name, 'Server-to-Server' auth, and is often abbreviated as S2S.)

In Skype for Business Server 2015 Modern Authentication (ADAL) conversations, Skype for Business Server 2015 communicates through ADFS (ADFS 3.0 in Windows Server 2012 R2). The authentication may happen using some other Identity Provider (IdP), but Skype for Business server needs to be configured to communicate with ADFS, directly. If you haven't configured ADFS to work with Skype for Business Server 2015 please complete ADFS installation.

ADAL is included in the March 2016 Cumulative Update for Skype for Business Server 2015, and the March 2016 Cumulative Update for Skype for Business must be installed and is needed for successful configuration. noteNote: During the initial release, Modern Authentication in an on-premises environment is supported only if there is no mixed Skype topology involved. For example, if the environment is purely Skype for Business Server 2015. This statement may be subject to change.

How-to solve the issue
==

As far a sI know, for now, Skype for business SDK does not support this authentication mechanism. It means we *MUST* disable ADAL support.
You can find those registry scripts at this [github location](https://github.com/Bhaal22/skype-for-business-checks/tree/master/registry_scripts/adal)

```
﻿Windows Registry Editor Version 5.00

[HKEY_CURRENT_USER\Software\Microsoft\Office\15.0\Common\Identity]
"EnableADAL"=dword:00000000

[HKEY_CURRENT_USER\Software\Microsoft\Office\16.0\Common\Identity]
"EnableADAL"=dword:00000000
```

Happy Skype for business coding!




