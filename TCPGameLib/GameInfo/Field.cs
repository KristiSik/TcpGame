using System;
using System.Collections.Generic;

namespace TCPGameLib.GameInfo
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

        public CellType[] Cells { get; set; }

        private List<Move> _history;

        public Player[] Players;

        public Field()
        {
            Cells = new CellType[_fieldSize * _fieldSize];
            Players = new Player[_userCount];
            _history = new List<Move>();

            for (int i = 0; i < _fieldSize * _fieldSize; i++)
            {
                Cells[i] = CellType.Empty;
            }
        }

        public void Display()
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < _fieldSize * _fieldSize; i++)
            {
                switch (Cells[i])
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
                for(int i = 0; i < _fieldSize * _fieldSize; i+=3)
                {
                    status = true;
                    for (int j = i; j < i + _fieldSize; j++)
                    {
                        if (Cells[j] != Cells[i] || Cells[i] == CellType.Empty)
                        {
                            status = false;
                        }
                    }
                    if (status == true)
                    {
                        return true;
                    }
                }
                return false;
            }
            bool CheckVertical()
            {
                bool status = true;
                for(int i = 0; i < _fieldSize; i++)
                {
                    status = true;
                    for (int j = i; j < i + (_fieldSize * _fieldSize); j+=3)
                    {
                        if (Cells[j] != Cells[i] || Cells[j] == CellType.Empty)
                        {
                            status = false;
                        }
                    }
                    if(status == true)
                    {
                        return true;
                    }
                }
                return false;
            }
            bool CheckDiagonalA()
            {
                bool status = true;
                int i = 0;
                status = true;
                for (int j = i; j < _fieldSize * _fieldSize; j+= _fieldSize + 1)
                {
                    if (Cells[j] != Cells[i] || Cells[j] == CellType.Empty)
                    {
                        status = false;
                    }
                }
                if(status == true)
                {
                    return true;
                }
                return false;
            }
            bool CheckDiagonalB()
            {
                bool status = true;
                int i = _fieldSize-1;
                for (int j = i; j < _fieldSize * (_fieldSize - 1); j+= _fieldSize - 1)
                {
                    if (Cells[j] != Cells[i] || Cells[j] == CellType.Empty)
                    {
                        status = false;
                    }
                }
                if(status == true)
                {
                    return true;
                }
                return false;
            }

            if (CheckHoriszontal())
            {
                return true;
            }
            if (CheckVertical())
            {
                return true;
            }
            if (CheckDiagonalA())
            {
                return true;
            }
            if (CheckDiagonalB())
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
            if (Cells[cell] != CellType.Empty)
            {
                _messages += "Incorrect position\n";
                return false;
            }

            // set figure
            Cells[cell] = (CellType)player.Type;

            // save in history
            _history.Add(new Move(cell, player));

            return true;
        }
    }
}
