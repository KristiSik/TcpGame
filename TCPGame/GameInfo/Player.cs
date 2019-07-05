using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{


    public class Player
    {
        public string Name { get; set; }
        
        public int Type { get; set; }

        public Player(string name, int type)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
