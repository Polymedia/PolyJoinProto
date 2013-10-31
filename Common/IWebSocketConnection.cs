using System;

namespace Polymedia.PolyJoin.Common
{
    public interface IWebSocketConnection
    {
        bool SendMessage(string message);
        bool SendData(byte[] data);

        event EventHandler<SimpleEventArgs<string>> MessageRecived;
        event EventHandler<SimpleEventArgs<byte[]>> DataRecived;
        event EventHandler<SimpleEventArgs<bool>> ConnectionStateChanged; 
    }
}
