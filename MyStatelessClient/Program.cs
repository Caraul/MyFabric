using System;
using System.Threading;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using MyStateless.Interfaces;

namespace MyStatelessClient
{
    public class Program
    {
        public static void Main()
        {
            var isEven = true;
            while (!Console.KeyAvailable)
            {
                var myStateless = ServiceProxy.Create<IMyStateless>(new Uri("fabric:/MyFabric/MyStateless"), new ServicePartitionKey(isEven ? "even" : "odd"));
                Console.WriteLine(myStateless.Now().Result);
                isEven = !isEven;
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}
