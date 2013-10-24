using System;

namespace Common
{
    public interface IWebSocketConnection
    {
        bool SendMessage(string message);
        bool SendData(byte[] data);

        event EventHandler<WebSocketEventArgs<string>> MessageRecived;
        event EventHandler <WebSocketEventArgs<byte[]>> DataRecived;
    }

    public class WebSocketEventArgs<T>: EventArgs
    {
        public T Value { get; set; }
    }
}
