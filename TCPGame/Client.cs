using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCPGameLib.Data;
using TCPGameLib.Extensions;
using TCPGameLib.GameInfo;
using TCPGameLib.Options;

namespace TCPGame
{
    public class Client
    {
        private readonly ClientSettings _appSettings;
        private Socket _clientSocket;
        private Game _game;

        public void InitializeConnection()
        {
            //if (_appSettings.IsServer)
            //{
            //    startServer();
            //}
            //else
            //{
            //    connectToServer();
            //}
        }

        public Client(IOptions<ClientSettings> options)
        {
            _appSettings = options.Value;
            _game = new Game();
            _game.SetCurrentPlayerInfo();
        }

        public bool ConnectToServer()
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(_appSettings.ServerIpAddress), _appSettings.ServerPort);
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _clientSocket.Connect(serverEndPoint);
                ConsoleExtensions.WriteSuccessMessage("Successfully connected to the server.");

                Task.Run(() =>
                {
                    while (true)
                    {
                        if (_clientSocket.Connected)
                        {
                            byte[] lengthBuffer = new byte[2];
                            int received = _clientSocket.Receive(lengthBuffer, 2, SocketFlags.None);
                            ushort packetByteSize = BitConverter.ToUInt16(lengthBuffer, 0);

                            byte[] jsonBuffer = new byte[packetByteSize];
                            _clientSocket.Receive(jsonBuffer, packetByteSize, SocketFlags.None);

                            var packet = Packet.Unpack(jsonBuffer);
                            processPacket(packet);

                            requestGameInfo();
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

        private void processPacket(Packet packet)
        {
            switch (packet.CommandType)
            {
                case CommandType.GameInfo:
                    _game.UpdateState((GameData)packet.ValueObject);
                    break;
            }
        }

        private void requestGameInfo()
        {
            var packet = new Packet(CommandType.GetGameInfo);
            _clientSocket.Send(packet.Pack());
        }

        //public bool SendPlayerName(string name)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(name);
        //    return sendData(CommandType.SetPlayerName, bytes);
        //}

        //public bool SendMove(int cellIndex)
        //{
        //    return sendData(CommandType.Move, BitConverter.GetBytes(cellIndex));
        //}

        //public bool SendPlayerTypeRequest()
        //{
        //    return sendData(CommandType.RequestPlayerType, new byte[0]);
        //}

        //private bool sendData(CommandType type, byte[] data)
        //{
        //    byte[] bytesToSend = new byte[data.Length + 1];
        //    bytesToSend[0] = (byte)type;
        //    Array.Copy(data, 0, bytesToSend, 1, data.Length);

        //    if (_client != null)
        //    {
        //        _client.Client.Send(bytesToSend);

        //        return true;
        //    }
        //    else if (_server != null)
        //    {
        //        _server.Server.Send(bytesToSend);

        //        return true;
        //    }

        //    return false;
        //}
    }
}
