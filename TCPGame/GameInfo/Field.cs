using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{
    public class Field
    {
        private int _fielsdSize = 3;
        private int _userCount = 2;
        private string _fig_null = "[ ]";
        private string _fig_one = " X ";
        private string _fig_two = "O";


        private string[] _cells;

        private List<Move> _history;

        public Player[] Players;

        public Field()
        {
            _cells = new string[_fielsdSize * _fielsdSize];
            Players = new Player[_userCount];
            _history = new List<Move>();

            for(int i = 0; i < _fielsdSize * _fielsdSize; i++)
            {
                _cells[i] = _fig_null;
            }
        }
        
        public void Display()
        {
            for(int i = 0; i < _fielsdSize * _fielsdSize; i++)
            {
                Console.Write("  " + _cells[i]);
                if((i+1) % _fielsdSize == 0)
                {
                    Console.WriteLine();
                }
            }
        }

        public bool CheckField()
        {
            return false;
        }

        public bool Move(int cell, Player player)
        {
            // set figure
            if(player.Type == 0)
            {
                _cells[cell] = _fig_one;
            }
            else if (player.Type == 1) {
                _cells[cell] = _fig_two;
            }
            else {
                _cells[cell] = _fig_null;
            }
            //save in history
            _history.Add(new Move(cell, player));

            return true;
        }
    }
}
