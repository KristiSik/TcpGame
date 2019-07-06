using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCPGame.Extensions;
using TCPGame.Options;

namespace TCPGame
{
    public class ConnectionManager
    {
        private readonly AppSettings _appSettings;

        private TcpListener _server;

        private TcpClient _client;

        public ConnectionManager(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public bool StartServer()
        {
            _server = new TcpListener(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _server.Start();

            Console.WriteLine("Waiting for the player...");

            Task.Run(() =>
            {
                while (true)
                {
                    if (_server.Pending())
                    {
                        var socket = _server.AcceptSocket();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Player connected.");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        ConsoleExtensions.WriteErrorMessage("Player connected.");

                        byte[] bytesData = new byte[socket.ReceiveBufferSize];
                        if (socket.Receive(bytesData) > 0)
                        {
                            int cellIndex = bytesData[0];
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            return true;
        }

        public bool ConnectToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _client = new TcpClient();
            try
            {
                _client.Connect(endPoint);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully connected to the server.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (SocketException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                ConsoleExtensions.WriteSuccessMessage("Successfully connected to the server.");
            }
            return true;
        }
    }
}
