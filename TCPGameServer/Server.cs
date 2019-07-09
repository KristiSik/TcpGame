using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCPGameLib.Extensions;
using TCPGameLib.GameInfo;
using TCPGameLib.Options;

namespace TCPGameServer
{
    public class Server
    {
        private readonly ServerSettings _appSettings;
        private TcpListener _listener;
        private List<Socket> _connectedSockets;
        private Game Game;
        private byte[] _buffer;

        public Server(IOptions<ServerSettings> options)
        {
            _appSettings = options.Value;
            _connectedSockets = new List<Socket>();
            _buffer = new byte[1024];
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
            _listener.Start();

            Console.WriteLine("Server started...");

            _listener.BeginAcceptSocket(new AsyncCallback(acceptCallback), null);

            //Task.Run(() =>
            //{
            //    while (_connectedSockets.Count < 2)
            //    {
            //        var socket = _listener.AcceptSocket();
            //        _connectedSockets.Add(socket);
            //        ConsoleExtensions.WriteSuccessMessage($"Player {_connectedSockets.Count} connected.");
            //    }
            //});

            //Task.Run(() =>
            //{
            //    //while (true)
            //    {
            //        var readSockets = new List<Socket>();
            //        var writeSockets = new List<Socket>();
            //        _connectedSockets.ForEach(s =>
            //        {
            //            readSockets.Add(s);
            //            writeSockets.Add(s);
            //        });
            //        Socket.Select(readSockets, writeSockets, null, 1000);

            //        foreach (var socket in readSockets)
            //        {
            //            byte[] receivedBytes = new byte[socket.ReceiveBufferSize];
            //            if (socket.Receive(receivedBytes) > 0)
            //            {
            //                byte controlByte = receivedBytes[0];
            //                byte[] dataBytes = new byte[receivedBytes.Length - 1];
            //                Array.Copy(receivedBytes, 1, dataBytes, 0, receivedBytes.Length - 1);
            //                //switch (controlByte)
            //                //{
            //                //    case (byte)DataType.PlayerName:
            //                //        {
            //                //            string playerName = Encoding.UTF8.GetString(dataBytes).TrimEnd('\0');
            //                //            OnDataReceived(new DataFrame(DataType.PlayerName, playerName));
            //                //            break;
            //                //        }
            //                //    case (byte)DataType.Move:
            //                //        {
            //                //            int cellIndex = BitConverter.ToInt32(dataBytes);
            //                //            OnDataReceived(new DataFrame(DataType.Move, cellIndex));
            //                //            break;
            //                //        }
            //                //    case (byte)DataType.RequestPlayerType:
            //                //        {
            //                //            OnDataReceived(new DataFrame(DataType.PlayerType, null));
            //                //            break;
            //                //        }
            //                //}
            //            }
            //            Thread.Sleep(100);
            //        }
            //    }
            //});
        }

        private void acceptCallback(IAsyncResult result)
        {
            var socket = _listener.EndAcceptSocket(result);
            _connectedSockets.Add(socket);

            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(beginReceive), socket);

            if (_connectedSockets.Count < 2)
            {
                _listener.BeginAcceptSocket(new AsyncCallback(acceptCallback), null);
            }
        }

        private void beginReceive(IAsyncResult result)
        {
            var socket = (Socket)result.AsyncState;
            int received = socket.EndReceive(result);
            byte[] receivedBytes = new byte[received];
            Array.Copy(receivedBytes, _buffer, received);

            var message = Encoding.UTF8.GetString(_buffer);
            Console.WriteLine(message);
        }

    }
}
