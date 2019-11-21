namespace OpenWasherHardwareLibrary.Commands
{
    internal class Ping : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[0];
        }
    }
}
