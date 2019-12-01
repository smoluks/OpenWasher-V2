using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class StopProgram : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[1] { (byte)PacketType.CommandStop };
        }
    }
}
