using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPGame.Data;
using TCPGame.Extensions;
using TCPGame.Options;

namespace TCPGame
{
    public class ConnectionManager
    {
        public Action<DataFrame> OnDataReceived;

        private const int SOCKET_RECEIVE_BUFFER_SIZE = 1000000;

        private readonly AppSettings _appSettings;

        private TcpListener _server;

        private TcpClient _client;

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
                var socket = _server.AcceptSocket();
                socket.ReceiveBufferSize = SOCKET_RECEIVE_BUFFER_SIZE;
                ConsoleExtensions.WriteErrorMessage("Player connected.");

                while (true)
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
                        Thread.Sleep(1000);
                    }
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
                ConsoleExtensions.WriteSuccessMessage("Successfully connected to the server.");
                Task.Run(() =>
                {
                    while (true)
                    {
                        byte[] receivedBytes = new byte[_client.Client.ReceiveBufferSize];
                        if (_client.Connected)
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
                            }
                        }
                        Thread.Sleep(1000);
                    }
                });
            }
            catch (SocketException e)
            {
                ConsoleExtensions.WriteErrorMessage(e.Message);
            }
            return true;
        }

        public bool SendPlayerName(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name);
            return sendData(DataType.PlayerName, bytes);
        }

        public bool SendMove(int cellIndex)
        {
            return sendData(DataType.Move, BitConverter.GetBytes(cellIndex));
        }

        public bool SendPlayerTypeRequest()
        {
            return sendData(DataType.RequestPlayerType, new byte[0]);
        }

        private bool sendData(DataType type, byte[] data)
        {
            byte[] bytesToSend = new byte[data.Length + 1];
            bytesToSend[0] = (byte)type;
            Array.Copy(data, 0, bytesToSend, 1, data.Length);

            if (_client != null)
            {
                _client.Client.Send(bytesToSend);

                return true;
            }
            else if (_server != null)
            {
                _server.Server.Send(bytesToSend);

                return true;
            }

            return false;
        }
    }
}
