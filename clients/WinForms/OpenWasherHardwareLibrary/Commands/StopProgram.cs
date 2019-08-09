using System.Collections.Generic;

namespace OpenWasherHardwareLibrary.Commands
{
    public class StopProgram : IWasherCommand
    {
        public byte[] GetRequest()
        {
            return new byte[1] { 2 };
        }
    }
}
