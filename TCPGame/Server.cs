using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCPGame.Data;
using TCPGame.Extensions;
using TCPGame.Options;

namespace TCPGame
{
    public class Server
    {
        private readonly ConnectionManager _connectionManager;
        private readonly AppSettings _appSettings;
        private TcpListener _listener;
        private List<Socket> _connectedSockets;

        public Server(ConnectionManager connectionManager, IOptions<AppSettings> options)
        {
            _connectionManager = connectionManager;
            _appSettings = options.Value;
            _connectedSockets = new List<Socket>();
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _listener.Start();

            Console.WriteLine("Server started...");

            Task.Run(() =>
            {
                while (_connectedSockets.Count < 2)
                {
                    var socket = _listener.AcceptSocket();
                    _connectedSockets.Add(socket);
                    ConsoleExtensions.WriteSuccessMessage($"Player {_connectedSockets.Count} connected.");
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var readSockets = new List<Socket>();
                    var writeSockets = new List<Socket>();
                    _connectedSockets.ForEach(s => {
                        readSockets.Add(s);
                        writeSockets.Add(s);
                    });
                    Socket.Select(readSockets, writeSockets, null, 1000);

                    foreach(var socket in readSockets)
                    {
                        byte[] receivedBytes = new byte[socket.ReceiveBufferSize];
                        if (socket.Receive(receivedBytes) > 0)
                        {
                            byte controlByte = receivedBytes[0];
                            byte[] dataBytes = new byte[receivedBytes.Length - 1];
                            Array.Copy(receivedBytes, 1, dataBytes, 0, receivedBytes.Length - 1);
                            switch (controlByte)
                            {
                                case (byte)DataType.PlayerName:
                                    {
                                        string playerName = Encoding.UTF8.GetString(dataBytes).TrimEnd('\0');
                                        OnDataReceived(new DataFrame(DataType.PlayerName, playerName));
                                        break;
                                    }
                                case (byte)DataType.Move:
                                    {
                                        int cellIndex = BitConverter.ToInt32(dataBytes);
                                        OnDataReceived(new DataFrame(DataType.Move, cellIndex));
                                        break;
                                    }
                                case (byte)DataType.RequestPlayerType:
                                    {
                                        OnDataReceived(new DataFrame(DataType.PlayerType, null));
                                        break;
                                    }
                            }
                        }
                        Thread.Sleep(100);
                    }
                }
            });
        }

    }
}
