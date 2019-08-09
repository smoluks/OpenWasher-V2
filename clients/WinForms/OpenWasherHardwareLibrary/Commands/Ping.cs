using System.Collections.Generic;

namespace OpenWasherHardwareLibrary.Commands
{
    public class Ping : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[0];
        }
    }
}
