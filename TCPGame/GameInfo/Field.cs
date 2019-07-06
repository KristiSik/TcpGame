using System;
using System.Collections.Generic;
using System.Text;

namespace TCPGame.GameInfo
{

    enum CellType : int
    {
        Empty = 0,
        X = 1,
        O = 2
    }

    public class Field
    {
        private int _fielsdSize = 3;
        private int _userCount = 2;
        private string _fig_null = "[ ]";
        private string _fig_one = " X ";
        private string _fig_two = " O ";
        private string _messages = "";


        private int[] _cells;

        private List<Move> _history;

        public Player[] Players;

        public Field()
        {
            _cells = new int[_fielsdSize * _fielsdSize];
            Players = new Player[_userCount];
            _history = new List<Move>();
            
            for(int i = 0; i < _fielsdSize * _fielsdSize; i++)
            {
                _cells[i] = (int)CellType.Empty;
            }
        }
        
        public void Display()
        {
            string print = "";
            Console.SetCursorPosition(0, 0);
            for(int i = 0; i < _fielsdSize * _fielsdSize; i++)
            {
                switch (_cells[i])
                {
                    case (int)CellType.Empty:
                        Console.Write("  " + _fig_null);
                        break;
                    case (int)CellType.X:
                        Console.Write("  " + _fig_one);
                        break;
                    case (int)CellType.O:
                        Console.Write("  " + _fig_two);
                        break;

                    default: break;
                }
                
                if((i+1) % _fielsdSize == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(_messages);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            _messages = "";
        }

        public bool CheckField()
        {
            bool CheckHoriszontal()
            {
                bool status = true;
                for(int i = 1; i < _fielsdSize * _fielsdSize; i+=3)
                {
                    for(int j = i+1; j < i+_fielsdSize-1; j++)
                    {
                        if (_cells[j] != _cells[i])
                        {
                            status = false;
                        }
                    }
                    if(status == true)
                    {
                        return true;
                    }
                    status = true;
                }
                return status;
            }

            if (CheckHoriszontal())
            {
                return true;
            }
            return false;
        }

        public bool Move(int cell, Player player)
        {
            // validation
            if (cell < 0 && cell + 1 > _fielsdSize * _fielsdSize)
            {
                _messages = "Incorrect position\n";
                return false;
            }
            if(_cells[cell] != (int)CellType.Empty)
            {
                _messages += "Incorrect position\n";
                return false;
            }

            // set figure
            if(player.Type == 0)
            {
                _cells[cell] = (int)CellType.X;
            }
            else if (player.Type == PlayerType.X) {
                _cells[cell] = _fig_two;
            }
            else {
                _cells[cell] = _fig_null;
            }

            // save in history
            _history.Add(new Move(cell, player));

            return true;
        }
    }
}
