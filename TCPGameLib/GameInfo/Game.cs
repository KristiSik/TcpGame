using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TCPGameLib.Data;
using TCPGameLib.Extensions;

namespace TCPGameLib.GameInfo
{
    public class Game
    {
        public bool IsActive { get; set; }
        private Field _field;
        private List<Player> _players;
        private Player _currentPlayer;

        public Game()
        {
            _field = new Field();
            _players = new List<Player>();
        }

        public void SetCurrentPlayerInfo()
        {
            string currentPlayerName = ConsoleExtensions.RequestPlayerName();
            _currentPlayer = new Player(currentPlayerName);
            AddPlayer(_currentPlayer);
        }

        public void Start()
        {
            setRandomPlayerTypes();
            //do
            //{

            //} while (!Field.CheckField());
        }

        public void Step()
        {

        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new GameData() { Cells = _field.Cells});
        }

        public void UpdateState(GameData gameData)
        {
            _players = gameData.Players.ToList();
            _field.Cells = gameData.Cells;
        }

        private void setRandomPlayerTypes()
        {
            var random = new Random().NextDouble();
            _players[0].Type = (PlayerType)Math.Round(random);
            _players[1].Type = (1 - _players[0].Type);
        }

        public bool AddPlayer(Player player)
        {
            _players.Add(player);
            return true;
        }
    }
}
