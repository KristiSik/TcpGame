using System;
using System.Collections.Generic;

namespace TCPGame.GameInfo
{

    public enum CellType
    {
        O = 0,
        X = 1,
        Empty = 2
    }

    public class Field
    {
        private int _fieldSize = 3;
        private int _userCount = 2;
        private string _fig_null = "[ ]";
        private string _fig_one = " X ";
        private string _fig_two = " O ";
        private string _messages = "";


        private CellType[] _cells;

        private List<Move> _history;

        public Player[] Players;

        public Field()
        {
            _cells = new CellType[_fieldSize * _fieldSize];
            Players = new Player[_userCount];
            _history = new List<Move>();

            for (int i = 0; i < _fieldSize * _fieldSize; i++)
            {
                _cells[i] = CellType.Empty;
            }
        }

        public void Display()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < _fieldSize * _fieldSize; i++)
            {
                switch (_cells[i])
                {
                    case CellType.Empty:
                        Console.Write("  " + _fig_null);
                        break;
                    case CellType.X:
                        Console.Write("  " + _fig_one);
                        break;
                    case CellType.O:
                        Console.Write("  " + _fig_two);
                        break;

                    default: break;
                }

                if ((i + 1) % _fieldSize == 0)
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
                for (int i = 1; i < _fieldSize * _fieldSize; i += 3)
                {
                    for (int j = i + 1; j < i + _fieldSize - 1; j++)
                    {
                        if (_cells[j] != _cells[i])
                        {
                            status = false;
                        }
                    }
                    if (status == true)
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
            if (cell < 0 && cell + 1 > _fieldSize * _fieldSize)
            {
                _messages = "Incorrect position\n";
                return false;
            }
            if (_cells[cell] != CellType.Empty)
            {
                _messages += "Incorrect position\n";
                return false;
            }

            // set figure
            _cells[cell] = (CellType)player.Type;

            // save in history
            _history.Add(new Move(cell, player));

            return true;
        }
    }
}
