using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TCPGame.Options;

namespace TCPGame
{
    public class ConnectionManager
    {
        private readonly AppSettings _appSettings;

        public ConnectionManager(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public bool StartServer()
        {
            Task.Run(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(_appSettings.IpAddress), _appSettings.Port);
                listener.Start();
                while (true)
                {
                    if (listener.Pending())
                    {
                        var socket = listener.AcceptSocket();
                        Console.WriteLine("Player connected.");
                    }
                    Thread.Sleep(1000);
                }
            });
            return true;
        }
    }
}
