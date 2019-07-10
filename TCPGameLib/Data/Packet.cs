using Newtonsoft.Json;
using System;
using System.Text;

namespace TCPGameLib.Data
{
    public enum CommandType
    {
        PlayerInfo,
        RequestMove,
        MakeMove,
        GetGameInfo,
        GameInfo,
    }

    public class Packet
    {
        public CommandType CommandType { get; set; }
        public string Value { get; set; }

        public Packet(CommandType type, string value = null)
        {
            CommandType = type;
            Value = value;
        }

        public byte[] Pack()
        {
            var jsonPacket = JsonConvert.SerializeObject(this);
            var packetBytes = Encoding.UTF8.GetBytes(jsonPacket);

            var packetBytesLength = packetBytes.Length;
            var lengthBytes = BitConverter.GetBytes(packetBytesLength);

            var bufferBytes = new byte[lengthBytes.Length + packetBytes.Length];
            Array.Copy(lengthBytes, bufferBytes, 2);
            Array.Copy(packetBytes, 0, bufferBytes, 2, packetBytes.Length);

            return bufferBytes;
        }

        public static Packet Unpack(byte[] packetBytes)
        {
            string jsonString = Encoding.UTF8.GetString(packetBytes);
            return JsonConvert.DeserializeObject<Packet>(jsonString);
        }

        public T GetObjectValue<T>()
        {
            return JsonConvert.DeserializeObject<T>(Value);
        }
    }
}
