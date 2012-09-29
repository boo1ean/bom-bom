using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestTcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverSocket = new TcpListener(new IPAddress(new byte[] { 192, 168, 1, 139 }), 5000);
            serverSocket.Start();

            Console.WriteLine("Waiting for a client...");

            var client = serverSocket.AcceptTcpClient();
            var ns = client.GetStream();
            Console.WriteLine("Connected");
            while (true)
            {
                var data = new byte[1024];
                var recv = ns.Read(data, 0, data.Length);
                if (recv == 0)
                    break;

                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
                ns.Write(data, 0, recv);
            }

            ns.Close();
            client.Close();
        }
    }
}
