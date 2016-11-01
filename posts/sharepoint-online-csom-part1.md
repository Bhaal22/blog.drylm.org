<!-- 
.. title: Sharepoint Online CSOM - Part1
.. slug: sharepoint-online-csom-part1
.. date: 2016-09-08 22:46:42 UTC
.. tags: .net, microsoft, office365, sharepoint, sharepoint online, c#
.. category: programming
.. link: 
.. description: Tutorial on how to interact with a remote SharePoint Online site with c sharp and how to perform automatic processes like connectiong to a site with CSOM Apis
.. type: text
-->

Introduction
==

Client Side Object Model (CSOM) was first introduced in SharePoint 2010. The Client Side Object Model is mainly used to build client applications and enable programs to access SharePoint Sites that are hosted outside without using web services. Before CSOM existence, developpers had no choice using SOAP web services.
<!-- TEASER_END -->

This has been changed with the introduction of CSOM. In SharePoint 2010, the CSOM exposed the core SharePoint functionalities only whereas in SharePoint 2013, the Microsoft SharePoint team has added a few more assemblies. Now we are able to access the Service Applications using the Client Side Object Model.

Since SharePoint 2010, client APIs use now the REST API  and JSON responses provided by the server to make all communications. This posts has interests regarding **SharePoint Onlione**. For this particular purpose you can download librairies using **nuget**:

```
PM >  Install-Package Microsoft.SharePointOnline.CSOM
```

Here are the libraries downloaded:

 * Microsoft.Office.Client.Policy.dll
 * Microsoft.Office.Client.TranslationServices.dll
 * Microsoft.Office.SharePoint.Tools.dll
 * Microsoft.Online.SharePoint.Client.Tenant.dll
 * Microsoft.ProjectServer.Client.dll
 * Microsoft.SharePoint.Client.DocumentManagement.dll
 * Microsoft.SharePoint.Client.Publishing.dll
 * Microsoft.SharePoint.Client.Runtime.Windows.dll
 * Microsoft.SharePoint.Client.Runtime.dll
 * Microsoft.SharePoint.Client.Search.Applications.dll
 * Microsoft.SharePoint.Client.Search.dll
 * Microsoft.SharePoint.Client.Taxonomy.dll
 * Microsoft.SharePoint.Client.UserProfiles.dll
 * Microsoft.SharePoint.Client.WorkflowServices.dll
 * Microsoft.SharePoint.Client.dll


The most important one is the : Microsoft.SharePoint.Client.dll
Lets create a regular c# project and add a reference to this dll. Here is the snippet on how to initiate a connection to the SharePoint site :

```java
using Microsoft.SharePoint.Client.;
var credentials = new SharePointOnlineCredentials(Login, BuildSecureStringCredential(Password));

// Url is the url of your sharepoint site.
var sharepointClientContext = new Microsoft.SharePoint.Client.ClientContext(Url);
sharepointClientContext.Credentials = credentials;
```

Nothing really complicated at this stage. Lets say now we want to retrieve information on the site on which we connect to and his subsites

```java
var site = sharepointClientContext.Web;
sharepointClientContext.Load(site, s => s.Title, s => s.ServerRelativeUrl, s => s.Webs);
```

This snippet generates a remote query which will be executed on the server as soon as we call :

```java
/*
 * Will perform a POST on the REST API to get the
 * result of the query. Client waits for the response
 * to come back.
 */
sharepointClientContext.ExecuteQuery();
```

Now, lets display information:

```java
var version = context.ServerSchemaVersion;

Console.WriteLine(version.ToString());
Console.WriteLine(site.Title);

Console.WriteLine("[Start] SubSites");
foreach (var subsite in site.Webs)
{
    Console.WriteLine($"Title: {subsite.Title} - {subsite.Url}");
}
Console.WriteLine("[End] SubSites");
```

Easy no ?
In a next article we will see how to :

 * create programmatically new sites
 * add documents to libraries
 * get documents from libraries
 * ...