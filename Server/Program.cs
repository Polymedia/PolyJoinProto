using System;
using System.ServiceModel.Description;
using DifferenceLib;
using Microsoft.ServiceModel.WebSockets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            DiffContainer.Init(100);

            var host = new WebSocketHost(typeof(Connection), new ServiceThrottlingBehavior()
            {
                MaxConcurrentSessions = int.MaxValue,
                MaxConcurrentCalls = int.MaxValue,
                MaxConcurrentInstances = int.MaxValue
            },
            new Uri("ws://localhost:9080/PolyJoin"));

            var binding =
                WebSocketHost.CreateWebSocketBinding(https: false, sendBufferSize: int.MaxValue, receiveBufferSize: int.MaxValue);

            binding.SendTimeout = TimeSpan.FromMilliseconds(5000);
            binding.OpenTimeout = TimeSpan.FromDays(1);

            host.AddWebSocketEndpoint(binding);

            Console.WriteLine("Open host");
            host.Open();

            host.Faulted += (sender, eventArgs) =>
            {
                Console.WriteLine("Host falted");
            };

            CommonService.Init();

            Console.ReadLine();

            Console.WriteLine("Close host");
            host.Close();
        }
    }
}
