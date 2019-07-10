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
        public Player CurrentPlayer { get; set; }
        private Field _field;
        private List<Player> _players;

        public Game()
        {
            _field = new Field();
            _players = new List<Player>();
        }

        public void SetCurrentPlayerInfo()
        {
            string currentPlayerName = ConsoleExtensions.RequestPlayerName();
            CurrentPlayer = new Player(currentPlayerName);
            AddPlayer(CurrentPlayer);
        }

        public void Start()
        {
            setRandomPlayerTypes();
            ConsoleExtensions.WriteSuccessMessage("Game has started.");
        }

        public void Step()
        {
            
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new GameData() { Cells = _field.Cells, Players = _players.ToArray() });
        }

        public void UpdateState(GameData gameData)
        {
            _players = gameData.Players.ToList();
            _field.Cells = gameData.Cells;

            var currentPlayerInList = _players.FirstOrDefault(p => p.Id == CurrentPlayer.Id);
            if (currentPlayerInList != null)
            {
                CurrentPlayer = currentPlayerInList;
            }
        }

        private void setRandomPlayerTypes()
        {
            var random = new Random().NextDouble();
            _players[0].Type = (PlayerType)Math.Round(random);
            _players[1].Type = (1 - _players[0].Type);
            ConsoleExtensions.WriteSuccessMessage($"{_players[0].Name} is {_players[0].Type.ToString()} and {_players[1].Name} is {_players[1].Type.ToString()}");
        }

        public bool AddPlayer(Player player)
        {
            _players.Add(player);
            return true;
        }
    }
}
