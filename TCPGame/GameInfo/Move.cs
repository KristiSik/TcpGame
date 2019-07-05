using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{
    public class Move
    {
        public Player Player { get; set; }

        public int Cell { get; set; }

        public Move(int cell, Player player)
        {
            this.Player = player;
            this.Cell = cell;
        }
    }
}
