namespace TCPGameLib.GameInfo
{
    public class Move
    {
        public Player Player { get; set; }

        public int Cell { get; set; }

        public Move(int cell, Player player)
        {
            Player = player;
            Cell = cell;
        }
    }
}
