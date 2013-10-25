using System;

namespace Common
{
    public interface IWebSocketConnection
    {
        bool SendMessage(string message);
        bool SendData(byte[] data);

        event EventHandler<WebSocketEventArgs<string>> MessageRecived;
        event EventHandler <WebSocketEventArgs<byte[]>> DataRecived;
        event EventHandler<WebSocketEventArgs<bool>> ConnectionStateChanged; 
    }

    public class WebSocketEventArgs<T>: EventArgs
    {
        public WebSocketEventArgs(){}
        
        public WebSocketEventArgs(T value)
        {
            Value = value;
        }
        
        public T Value { get; set; }
    }
}
