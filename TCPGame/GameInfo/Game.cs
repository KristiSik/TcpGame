using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{
    public class Game
    {
        public bool IsActive { get; set; }

        public Field Field { get; set; }

        public Player[] Players = new Player[2];

        public Game()
        {
            Field = new Field();
        }

        public bool setPlayers(Player player1, Player player2)
        {
            Players[0] = player1;
            Players[1] = player2;

            return true;
        }
    }
}
