namespace OpenWasherHardwareLibrary.Commands
{
    public class GoToBootloader : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[1] { 0x0A };
        }
    }
}
