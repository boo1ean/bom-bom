﻿namespace AirHockey.Recognition.Client.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using AirHockey.Recognition.Client.Data;

    public class BomBomClient : IDisposable
    {
        private const string ServerIpAddress = "192.168.1.65";
        private const int ServerPort = 5000;

        private readonly TcpClient tcpClient;

        private readonly CommandRepository commandRepository;

        private NetworkStream networkStream;

        public BomBomClient()
        {
            this.commandRepository = new CommandRepository();
            this.tcpClient = new TcpClient();
        }

        public Task Connect()
        {
            var result = tcpClient.ConnectAsync(ServerIpAddress, ServerPort);

            result.ContinueWith(
                it =>
                {
                    if (it.Status == TaskStatus.RanToCompletion)
                    {
                        networkStream = tcpClient.GetStream();
                    }
                });

            return result;
        }

        public void Dispose()
        {
            if (networkStream != null)
            {
                networkStream.Dispose();
            }
        }

        public Task InitializeCommunication()
        {
            var initCommand = this.commandRepository.GetInitCommand();
            var data = initCommand.Serialize();
            return this.SendData(data);
        }

        public Task SendPolygon(IEnumerable<Polygon> polygon)
        {
            var command = this.commandRepository.GetSendPolygonCommand();
            var data = command.Serialize(polygon);
            return this.SendData(data);
        }

        private Task SendData(byte[] data)
        {
            return this.networkStream.WriteAsync(data, 0, data.Length);
        }
    }
}