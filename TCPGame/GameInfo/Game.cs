using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TCPGame.Data;
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

        private Player _currentPlayer { get; set; }

        public Game(IOptions<AppSettings> options, ConnectionManager connectionManager)
        {
            Field = new Field();
            _players = new List<Player>();

            _appSettings = options.Value;
            _connectionManager = connectionManager;

            string currentPlayerName = ConsoleExtensions.RequestPlayerName();

            _currentPlayer = new Player(currentPlayerName);
            AddPlayer(_currentPlayer);

            _connectionManager.InitializeConnection();
            _connectionManager.OnDataReceived += (frame) => updateGameData(frame);
        }

        public void Start()
        {
            setRandomPlayerTypes();
            do
            {

            } while (!Field.CheckField());
        }

        public void Step()
        {

        }

        private void updateGameData(DataFrame frame)
        {
            switch (frame.DataType)
            {
                case DataType.PlayerName:
                    {
                        if (_appSettings.IsServer)
                        {
                            AddPlayer(new Player((string)frame.Value));
                        } else
                        {
                            AddPlayer(new Player((string)frame.Value));
                            if (_currentPlayer == null)
                            {
                                _currentPlayer = new Player((string)frame.Value);
                                _connectionManager.SendPlayerTypeRequest();
                            } else
                            {
                                
                            }
                        }
                        break;
                    }
            }
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
