using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AirHockey.Client.WinPhone.Network
{
    internal class SocketClient : IDisposable
    {
        private readonly Socket _socket;

        public delegate void ConnectCompletedEventHandler();
        public delegate void ConnectFailedEventHandler(SocketError result);
        public event ConnectCompletedEventHandler ConnectCompleted;
        public event ConnectFailedEventHandler ConnectFailed;

        public bool IsConnected { get; private set; }

        public SocketClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string hostName, int portNumber)
        {
            var endPoint = new DnsEndPoint(hostName, portNumber);
            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = endPoint };

            socketEventArg.Completed +=
                (s, e) =>
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        invokeConnectCompleted();
                        IsConnected = true;
                    }
                    else
                        invokeConnectFailed(e.SocketError);
                };

            _socket.ConnectAsync(socketEventArg);
        }

        public void Send(byte[] data)
        {
            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = _socket.RemoteEndPoint, UserToken = null };
            socketEventArg.SetBuffer(data, 0, data.Length);
            _socket.SendAsync(socketEventArg);
        }

        public void SendInitInfo()
        {
            var data = insertCommand(new byte[] { 2 }, ServerCommands.Name);
            Send(data);
        }

        public void SendName(string name)
        {
            var data = insertCommand(Encoding.UTF8.GetBytes(name), ServerCommands.Name);
            Send(data);
        }

        public void SendAccelerometerData(float x, float y, float z)
        {
            var coordinates = floatArrayToByteArray(new[] { x, y, z });
            var data = insertCommand(coordinates, ServerCommands.AccelerometerData);
            Send(data);
        }

        private static byte[] insertCommand(IEnumerable<byte> data, ServerCommands command)
        {
            var list = data.ToList();
            list.Insert(0, (byte)command);
            return list.ToArray();
        }

        private static IEnumerable<byte> floatArrayToByteArray(float[] floatArray)
        {
            var byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
        }

        private void invokeConnectCompleted()
        {
            var handler = ConnectCompleted;
            if (handler != null) handler();
        }

        private void invokeConnectFailed(SocketError result)
        {
            var handler = ConnectFailed;
            if (handler != null) handler(result);
        }

        public void Dispose()
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
}
