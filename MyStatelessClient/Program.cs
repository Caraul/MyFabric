using System;
using System.Threading;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using MyStateless.Interfaces;

namespace MyStatelessClient
{
    public class Program
    {
        public static void Main()
        {
            while (!Console.KeyAvailable)
            {
                var myStateless = ServiceProxy.Create<IMyStateless>(new Uri("fabric:/MyFabric/MyStateless"));
                Console.WriteLine(myStateless.Now().Result);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
