using System;
using System.Net.Sockets;
using System.Text;

namespace TestTcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverSocket = new TcpListener(6666);
            serverSocket.Start();

            Console.WriteLine("Waiting for a client...");

            var client = serverSocket.AcceptTcpClient();
            var ns = client.GetStream();

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
