<!-- 
.. title: Microsoft SSL implementation usage is a mess(l)
.. slug: microsoft_ssl
.. date: 2014-01-15 00:05:32 UTC
.. tags: .net,mirosoft,programming
.. link: 
.. description:Microsoft SSL usage is complteley a mess 
.. type: text
-->




TLS/SSL Description
----

Since the different applicative protocols can run with or without SSL, servers must expose dedicated port (443 for https) or switch (STARTTLS in SMTP, POP, NNTP)

Above a description of the handshake between a client and a server running TLS (or SSL)

![](https://cdn.monetizejs.com/resources/button-32.png)

TLS/SSL history
----

Regarding [SSL wikipedia page](http://en.wikipedia.org/wiki/Transport_Layer_Security) this protocol has a long history with various versions :

|                  |                         |
 ----------------- | ------------------------|
| SSL 2.0          | 1995                    | 
| SSL 3.0          | 1996                    |
| TLS 1.0          | 1999                    |
| TLS 1.1          | 2006                    |
| TLS 1.2          | 2008                    |
| TLS 1.3          | Next ...                |

If we look at the TLS 1.0 specification, an implementation can downgrade automatically from TLS 1.0 to SSL 3.0.

Microsoft .net applicative protocols implementation
---

For the purpose of this article we will have a look at the implementation regarding the [**HttpWebRequest**](http://msdn.microsoft.com/fr-fr/library/system.net.httpwebrequest%28v=vs.110%29.aspx) class from the **System.Net** namespace.

Here is a simple snippet code using this class :

```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://www.google.com");
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      Console.WriteLine(response.StatusCode);
    }
  }
}
```

Output : 
```
OK
```

As you can see here we simply specify *https* as http scheme to get the body from main google page.
Suppose now you want to get response from an internal web site under IP address **192.168.1.195**. The web server hosts a website on TLS/SSL but with a self signed certificate. If you keep the previous code, you will have a beatiful **WebException** whose content is the following one :

> The underlying connection was closed: Could not establish trust relationship for the SSL/TLS secure channel.

Ok, we can easily fix that with this line of code :
```c#
ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
```

With this piece of code, our client program will accept ANY certificates (can be a security hole, but for our example, it is fine). We introduce here the static class **ServicePointManager**. We will come back on this point a bit later.

In the last months, some hackers have found issues in OpenSSL implementation and the main issue in SSL3.0 has been found by some Google engineers. This security hole has the name [Poodle](http://en.wikipedia.org/wiki/POODLE)

### POODLE

It results of a man in a middle attack on TLS to SSL 3.0 fallback. Check wikipedia page for more tehcnical information. With this serious issue, it results some guidelines :

 - disabling SSL 3.0 from clients 
	 - Internet explorer
	 - disabled by default from Firefox 34
	 - disabled by default in Chrome 39
 - diabling SSL 3.0 from servers and migrate them to TLS 1.0 (at least)

With this recommendation, suppose you want to remove SSL3 support.

```
# from apache2 ssl.conf
# enables only TLSv1

SSLProtocol TLSv1
```

Our program example still runs perfectly. Now suppose you REALLY want to keep compatibility with old devices / clients for some of your websites and you need to keep SSL 3 running on server side.


```
# from apache2 ssl.conf
# enables only SSL 3.0

SSLProtocol SSLv3
```

And now we get the following **WebException**

>The request was aborted: Could not create SSL/TLS secure channel.

Ok, after some little researches on google redirecting on stackoverflow, we get the response why this behavior. We need to add this line to our piece of code :
```c#
ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

/** Default value is : SSL3 | TLS which means an automatic fallback from TLS 1.0 to SSL 3.0 if needed **/
```


Ah !! Our **ServicePointManager** comes back. As you can see, the **ServicePointManager** is a static class with static properties such as 

 - SecurityProtocolType
 - ServerCertificateValidationCallback 
 - and many others you can find on this [page](http://msdn.microsoft.com/fr-fr/library/system.net.servicepointmanager%28v=vs.110%29.aspx).

**ServicePointManager** is in charge of managing ServicePoint instances representing connections on endpoints and drive the behavior of SSLStreams with the certificate policy validation, the type of security protocol, what to do with HTTP 100 status code ...
Good, it works well. Suppose now you want to your server accepts SSL 3 AND TLS 1.0

```
# from apache2 ssl.conf
# enables only SSL 3.0

SSLProtocol SSLv3 TLSv1
```

And you want to test individually each SSL type, we can modify our existing code like that :
