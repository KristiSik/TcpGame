using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{
    public enum PlayerType
    {
        X,
        O
    }

    public class Player
    {
        public string Name { get; set; }
        
        public PlayerType Type { get; set; }

        public Player(string name)
        {
            this.Name = name;
        }
    }
}
