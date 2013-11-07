﻿using System.Diagnostics;
using System;
using System.Threading;
using Polymedia.PolyJoin.Common;
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

                    ConnectionStateChanged.Invoke(this, new SimpleEventArgs<bool>(true));
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

                    ConnectionStateChanged.Invoke(this, new SimpleEventArgs<bool>(false));
                };

            _webSocket.MessageReceived += (sender, args) =>
                {
                    MessageRecived.Invoke(this, new SimpleEventArgs<string>() { Value = args.Message });
                };

            _webSocket.DataReceived += (sender, args) =>
                {
                    DataRecived.Invoke(this, new SimpleEventArgs<byte[]>() { Value = args.Data });
                };
        }

        public void Start()
        {
            new Thread(() =>
                {
                    while (!_stop)
                    {
                        try
                        {
                            Console.WriteLine("Open socket");
                            _webSocket.Open();

                            _reconnect.WaitOne();

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
                Debug.Assert(data.Length <= 65536, "Sending big data to server" + data.Length);

                _webSocket.Send(data, 0, data.Length);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public event EventHandler<SimpleEventArgs<string>> MessageRecived = delegate { };

        public event EventHandler<SimpleEventArgs<byte[]>> DataRecived = delegate { };

        public event EventHandler<SimpleEventArgs<bool>> ConnectionStateChanged = delegate { }; 

        #endregion IWebSocketConnection
    }
}
