using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class GoToBootloader : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[1] { (byte)PacketType.EnterProgrammingMode };
        }
    }
}
