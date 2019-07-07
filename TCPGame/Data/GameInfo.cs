using System;
using System.Collections.Generic;
using System.Text;
using TCPGame.GameInfo;

namespace TCPGame.Data
{
    public class GameInfo
    {
        public Player[] Players { get; set; }

        public CellType[] Cells { get; set; }
    }
}
