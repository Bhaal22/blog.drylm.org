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