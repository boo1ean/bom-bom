using System.Net.Sockets;

namespace AirHockey.Client.WinPhone.Network
{
    public delegate void ConnectSucceedEventHandler();
    public delegate void ConnectFailedEventHandler(SocketError result);
    public delegate void ReceivedEventHandler(byte[] data);

    internal interface ISocketClient
    {
        void Send(byte[] data);
        void Receive();
        event ReceivedEventHandler Received;
    }
}