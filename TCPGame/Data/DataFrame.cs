namespace TCPGame.Data
{
    public enum DataType
    {
        PlayerName,
        Move,
        RequestPlayerType,
        PlayerType
    }

    public class DataFrame
    {
        public DataType DataType { get; set; }

        public object Value { get; set; }

        public DataFrame(DataType type, object value)
        {
            DataType = type;
            Value = value;
        }
    }
}
