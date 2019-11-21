using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class GetConfig : IWasherCommand<byte[]>
    {
        public byte[] GetRequest()
        {
            return new byte[1] { (byte)PacketType.GetSetConfig };
        }

        public byte[] ParceResponse(byte[] data)
        {
            var config = new byte[data.Length - 1];
            data.CopyTo(config, 1);

            return config;
        }
    }
}
