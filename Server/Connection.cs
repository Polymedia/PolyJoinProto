using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceModel.WebSockets;

namespace Polymedia.PolyJoin.Server
{
    class Connection : WebSocketService, IWebSocketConnection
    {
        public override void OnOpen()
        {
            ConnectionManager.AddConnection(this);

            Console.WriteLine("Connection opened");
        }

        public override void OnMessage(byte[] data)
        {
            Console.WriteLine("Connection message bytes");

            DataRecived.Invoke(this, new WebSocketEventArgs<byte[]>() {Value = data});
        }

        public override void OnMessage(string message)
        {
            Console.WriteLine("Connection message string");

            MessageRecived.Invoke(this, new WebSocketEventArgs<string>() { Value = message });
        }

        protected override void OnError()
        {
            Console.WriteLine("Connection error");
        }

        protected override void OnClose()
        {
            ConnectionManager.RemoveConnection(this);

            Console.WriteLine("Connection closed");
        }

        #region IWebSocketConnection

        public bool SendMessage(string message)
        {
            //async/await?
            Send(message);

            return true;
        }

        public bool SendData(byte[] data)
        {
            //async/await?
            try
            {
                Send(data);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public event EventHandler<WebSocketEventArgs<string>> MessageRecived = delegate {  };

        public event EventHandler<WebSocketEventArgs<byte[]>> DataRecived = delegate { };

        #endregion IWebSocketConnection
    }
}
