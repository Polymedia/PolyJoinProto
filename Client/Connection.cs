using Common;
using System;
using System.Threading;
using WebSocket4Net;

namespace Polymedia.PolyJoin.Client
{
    public class Connection : IWebSocketConnection
    {
        private WebSocket _webSocket = null;

        private AutoResetEvent _reconnect = new AutoResetEvent(false);
        private bool _stop = false;

        public Connection(string uri)
        {
            _webSocket = new WebSocket(uri);

            _webSocket.Opened += (sender, args) =>
                {
                    Console.WriteLine("OPENED");
                    _reconnect.Reset();
                };

            _webSocket.Error += (sender, args) =>
                {
                    Console.WriteLine("ERROR");
                    _reconnect.Set();
                };

            _webSocket.Closed += (sender, args) =>
                {
                    Console.WriteLine("CLOSED");
                    _reconnect.Set();
                };

            _webSocket.MessageReceived += (sender, args) =>
                {
                    MessageRecived.Invoke(this, new WebSocketEventArgs<string>() { Value = args.Message });
                };

            _webSocket.DataReceived += (sender, args) =>
                {
                    DataRecived.Invoke(this, new WebSocketEventArgs<byte[]>() { Value = args.Data });
                };
        }

        public void Start()
        {
            new Thread(() =>
                {
                    while (!_stop)
                    {
                        Console.WriteLine("Open socket");
                        _webSocket.Open();

                        _reconnect.WaitOne();

                        try
                        {
                            Console.WriteLine("Close socket");
                            _webSocket.Close();
                        }
                        catch
                        {
                        }

                        _reconnect.Reset();

                        Thread.Sleep(5000);
                    }
                }).Start();
        }

        public void Stop()
        {
            _stop = true;
            _reconnect.Set();
        }

        #region IWebSocketConnection

        public bool SendMessage(string message)
        {
            _webSocket.Send(message);
            return true;
        }

        public bool SendData(byte[] data)
        {
            try
            {
                if (data.Length < 65000)
                    _webSocket.Send(data, 0, data.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public event EventHandler<WebSocketEventArgs<string>> MessageRecived = delegate { };

        public event EventHandler<WebSocketEventArgs<byte[]>> DataRecived = delegate { };

        #endregion IWebSocketConnection
    }
}
