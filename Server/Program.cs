using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceModel.WebSockets;

namespace Polymedia.PolyJoin.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebSocketHost(typeof(Connection), new ServiceThrottlingBehavior()
            {
                MaxConcurrentSessions = int.MaxValue,
                MaxConcurrentCalls = int.MaxValue,
                MaxConcurrentInstances = int.MaxValue
            },
            new Uri("ws://localhost:9080/PolyJoin"));

            var binding =
                WebSocketHost.CreateWebSocketBinding(https: false, sendBufferSize: 2048, receiveBufferSize: 2048);

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
