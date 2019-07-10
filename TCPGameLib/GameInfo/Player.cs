namespace TCPGameLib.GameInfo
{
    public enum PlayerType
    {
        O = 0,
        X = 1,
    }

    public class Player
    {
        public string Name { get; set; }

        public PlayerType Type { get; set; }

        public Player()
        {

        }

        public Player(string name)
        {
            this.Name = name;
        }
    }
}
