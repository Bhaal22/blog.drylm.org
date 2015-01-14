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

