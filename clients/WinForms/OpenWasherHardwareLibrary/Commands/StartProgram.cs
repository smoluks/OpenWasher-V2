using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Commands
{
    public class StartProgram : IWasherCommand
    {
        readonly WashProgram program;
        readonly ProgramOptions options;

        public StartProgram(WashProgram program, ProgramOptions options = null)
        {
            this.program = program;
            this.options = options;
        }

        public byte[] GetRequest()
        {
            if (options == null)
                return new byte[2] { 1, (byte)program };
            else
            {
                byte[] data = new byte[8] { 1, (byte)program, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                if (options.temperature.HasValue)
                    data[2] = options.temperature.Value;
                if (options.duration.HasValue)
                    data[3] = options.duration.Value;
                //if (options.spinningspeed.HasValue)
                //    data[4] = options.spinningspeed.Value;
                if (options.spinningspeed.HasValue)
                    data[5] = options.spinningspeed.Value;
                if (options.waterlevel.HasValue)
                    data[6] = options.waterlevel.Value;
                if (options.rinsingCycles.HasValue)
                    data[7] = options.rinsingCycles.Value;

                return data;
            }
        }
    }
}
