using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TCPGame.Extensions;
using TCPGame.Options;

namespace TCPGame.GameInfo
{
    public class Game
    {
        public bool IsActive { get; set; }

        public Field Field { get; set; }

        private readonly List<Player> _players;

        private readonly AppSettings _appSettings;

        private readonly ConnectionManager _connectionManager;

        internal void setPlayers(Player player1, Player player2)
        {
            throw new NotImplementedException();
        }

        public Player CurrentPlayer { get; set; }

        public Game(IOptions<AppSettings> options, ConnectionManager connectionManager)
        {
            Field = new Field();
            _players = new List<Player>();

            _appSettings = options.Value;
            _connectionManager = connectionManager;

            string currentPlayerName = ConsoleExtensions.RequestPlayerName();

            AddPlayer(new Player(currentPlayerName));

            _connectionManager.InitializeConnection();
        }

        public void Start()
        {
            setRandomPlayerTypes();
        }

        private void setRandomPlayerTypes()
        {
            var random = new Random().NextDouble();
            if (random > 0.5)
            {
                _players[0].Type = PlayerType.O;
                _players[1].Type = PlayerType.X;
            }
            else
            {
                _players[0].Type = PlayerType.X;
                _players[1].Type = PlayerType.O;
            }
        }


        public bool AddPlayer(Player player)
        {
            _players.Add(player);
            return true;
        }
    }
}
