using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPGame.Extensions;
using TCPGame.Options;

namespace TCPGame
{
    public class ConnectionManager
    {
        public Action<SocketFrame> OnDataReceived;

        private const int SOCKET_RECEIVE_BUFFER_SIZE = 1000000;

        private readonly AppSettings _appSettings;

        private TcpListener _server;

        private TcpClient _client;

        private Dictionary<byte, Action> serverActions = new Dictionary<byte, Action>()
        {
        };

        public void InitializeConnection()
        {
            if (_appSettings.IsServer)
            {
                startServer();
            }
            else
            {
                connectToServer();
            }
        }

        public ConnectionManager(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        private bool startServer()
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
                        socket.ReceiveBufferSize = SOCKET_RECEIVE_BUFFER_SIZE;

                        ConsoleExtensions.WriteErrorMessage("Player connected.");

                        byte[] bytesData = new byte[socket.ReceiveBufferSize];
                        if (socket.Receive(bytesData) > 0)
                        {
                            string jsonSocketFrame = Encoding.UTF8.GetString(bytesData);
                            var socketFrame = JsonConvert.DeserializeObject<SocketFrame>(jsonSocketFrame);
                            OnDataReceived(socketFrame);
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            return true;
        }

        private bool connectToServer()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _client = new TcpClient();
            _client.ReceiveBufferSize = SOCKET_RECEIVE_BUFFER_SIZE;
            try
            {
                _client.Connect(endPoint);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully connected to the server.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (SocketException e)
            {
                ConsoleExtensions.WriteErrorMessage(e.Message);
            }
            return true;
        }
    }
}
