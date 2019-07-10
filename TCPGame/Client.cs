using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

                sendPlayerInfo();

                Task.Run(() =>
                {
                    while (true)
                    {
                        if (_clientSocket.Connected)
                        {
                            requestGameInfo();

                            byte[] lengthBuffer = new byte[2];
                            int received = _clientSocket.Receive(lengthBuffer, 2, SocketFlags.None);
                            ushort packetByteSize = BitConverter.ToUInt16(lengthBuffer, 0);

                            if (packetByteSize > 0)
                            {
                                byte[] jsonBuffer = new byte[packetByteSize];
                                _clientSocket.Receive(jsonBuffer, packetByteSize, SocketFlags.None);

                                var packet = Packet.Unpack(jsonBuffer);
                                processPacket(packet);
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

        private void processPacket(Packet packet)
        {
            switch (packet.CommandType)
            {
                case CommandType.GameInfo:
                    _game.UpdateState(packet.GetObjectValue<GameData>());
                    break;
            }
        }

        private void requestGameInfo()
        {
            var packet = new Packet(CommandType.GetGameInfo);
            _clientSocket.Send(packet.Pack());
        }

        private void sendPlayerInfo()
        {
            var jsonPlayer = JsonConvert.SerializeObject(_game.CurrentPlayer);
            var packet = new Packet(CommandType.PlayerInfo, jsonPlayer);
            _clientSocket.Send(packet.Pack());
        }
    }
}
