using TCPGame.GameInfo;

namespace TCPGame
{
    public class SocketFrame
    {
        public Field Field { get; set; }

        public Player[] Players { get; set; }
    }
}
