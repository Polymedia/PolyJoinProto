using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Polymedia.PolyJoin.Common
{
    public abstract class ConnectionWrapper
    {
        private IWebSocketConnection _webSocketConnection;

        protected IWebSocketConnection WebSocketConnection
        {
            get { return _webSocketConnection; }
        }

        public event EventHandler<SimpleEventArgs<bool>> ConnectionStateChangedEvent = delegate { }; 

        public ConnectionWrapper(IWebSocketConnection webSocketConnection)
        {
            _webSocketConnection = webSocketConnection;
            _webSocketConnection.DataRecived += WebSocketConnectionOnDataRecived;
            _webSocketConnection.ConnectionStateChanged += WebSocketConnectionOnConnectionStateChanged;
        }

        protected void SendCommand(Command command)
        {
            byte[] commandBytes = ObjectToByteArray(command);

            Console.WriteLine("Sending bytes: " + commandBytes.Length);

            _webSocketConnection.SendData(commandBytes);
        }

        protected abstract void OnReceivedCommand(Command command);

        protected static byte[] ObjectToByteArray(Object obj)
        {
            if(obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        protected static Object ByteArrayToObject(byte[] bytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            return binForm.Deserialize(memStream);
        }

        protected static T ByteArrayToObject<T>(byte[] bytes)
        {
            return (T)ByteArrayToObject(bytes);
        }

        private void WebSocketConnectionOnConnectionStateChanged(object sender, SimpleEventArgs<bool> webSocketEventArgs)
        {
            ConnectionStateChangedEvent.Invoke(_webSocketConnection, webSocketEventArgs);
        }

        private void WebSocketConnectionOnDataRecived(object sender, SimpleEventArgs<byte[]> webSocketEventArgs)
        {
            Command command = ByteArrayToObject<Command>(webSocketEventArgs.Value);
            OnReceivedCommand(command);
        }
    }
}
