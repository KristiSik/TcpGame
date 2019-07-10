using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TCPGameLib.Data;
using TCPGameLib.Extensions;
using TCPGameLib.GameInfo;
using TCPGameLib.Options;

namespace TCPGameServer
{
    public class Server
    {
        private readonly ServerSettings _appSettings;
        private TcpListener _listener;
        private Dictionary<Socket, Player> _connectedSockets;
        private Game _game;
        private byte[] _lengthBuffer;

        public Server(IOptions<ServerSettings> options)
        {
            _appSettings = options.Value;
            _connectedSockets = new Dictionary<Socket, Player>();
            _lengthBuffer = new byte[2];
            _game = new Game();
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _listener.Start();

            Console.WriteLine("Server started...");

            _listener.BeginAcceptSocket(new AsyncCallback(acceptCallback), null);
        }

        private void acceptCallback(IAsyncResult result)
        {
            var socket = _listener.EndAcceptSocket(result);

            var newPlayer = new Player();
            _connectedSockets.Add(socket, newPlayer);
            _game.AddPlayer(newPlayer);

            ConsoleExtensions.WriteSuccessMessage($"Player {_connectedSockets.Count} has connected.");

            socket.BeginReceive(_lengthBuffer, 0, _lengthBuffer.Length, SocketFlags.None, new AsyncCallback(beginReceivePacketLength), socket);

            if (_connectedSockets.Count < 2)
            {
                _listener.BeginAcceptSocket(new AsyncCallback(acceptCallback), null);
            }
            else
            {
                // waiting, until all players haven't set their names
                while (_connectedSockets.Values.Any(p => p.Name == null))
                {
                    Thread.Sleep(1000);
                }
                _game.Start();
            }
        }

        private void beginReceivePacketLength(IAsyncResult result)
        {
            var socket = (Socket)result.AsyncState;
            int received = socket.EndReceive(result);

            ushort packetByteSize = BitConverter.ToUInt16(_lengthBuffer, 0);
            if (packetByteSize > 0)
            {
                byte[] jsonBuffer = new byte[packetByteSize];
                socket.Receive(jsonBuffer, packetByteSize, SocketFlags.None);

                var packet = Packet.Unpack(jsonBuffer);
                processPacket(packet, socket);
            }

            socket.BeginReceive(_lengthBuffer, 0, _lengthBuffer.Length, SocketFlags.None, new AsyncCallback(beginReceivePacketLength), socket);
        }

        private void processPacket(Packet packet, Socket socket = null)
        {
            switch (packet.CommandType)
            {
                case CommandType.GetGameInfo:
                    sendGameInfo(socket);
                    break;
                case CommandType.PlayerInfo:
                    setPlayerInfo(socket, packet.GetObjectValue<Player>());
                    break;
            }
        }

        private void sendGameInfo(Socket socket)
        {
            var gameData = _game.ToString();
            var packet = new Packet(CommandType.GameInfo, gameData);
            sendPacket(socket, packet);
        }

        private void sendPacket(Socket socket, Packet packet)
        {
            socket.Send(packet.Pack());
        }

        private void setPlayerInfo(Socket socket, Player player)
        {
            _connectedSockets[socket].Name = player.Name;
            _connectedSockets[socket].Id = player.Id;
        }
    }
}
