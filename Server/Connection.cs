using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.ServiceModel.WebSockets;

namespace Polymedia.PolyJoin.Server
{
    class Connection : WebSocketService, IWebSocketConnection
    {
        private AutoResetEvent _receiveMessages = new AutoResetEvent(false);
        private volatile bool _isOpened = false;


        public override void OnOpen()
        {
            ConnectionManager.AddConnection(this);

            Console.WriteLine("Connection opened");

            ConnectionStateChanged.Invoke(this, new WebSocketEventArgs<bool>(true));

            _isOpened = true;

            _receiveMessages.Set();
        }

        public override void OnMessage(byte[] data)
        {
            if(!_isOpened)
                _receiveMessages.WaitOne();
            
            Console.WriteLine("Connection message bytes");

            DataRecived.Invoke(this, new WebSocketEventArgs<byte[]>() {Value = data});
        }

        public override void OnMessage(string message)
        {
            if (!_isOpened)
                _receiveMessages.WaitOne();
            
            Console.WriteLine("Connection message string");

            MessageRecived.Invoke(this, new WebSocketEventArgs<string>() { Value = message });
        }

        protected override void OnError()
        {
            Console.WriteLine("Connection error");
        }

        protected override void OnClose()
        {
            _isOpened = false;

            _receiveMessages.Reset();
            
            ConnectionManager.RemoveConnection(this);

            ConnectionStateChanged.Invoke(this, new WebSocketEventArgs<bool>(false));

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
                Debug.Assert(data.Length <= 65536, "Sending big data to client");
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

        public event EventHandler<WebSocketEventArgs<bool>> ConnectionStateChanged = delegate { }; 

        #endregion IWebSocketConnection
    }
}
