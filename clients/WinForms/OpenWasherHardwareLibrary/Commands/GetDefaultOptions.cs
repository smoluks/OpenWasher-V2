using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class GetDefaultOptions : IWasherCommand<ProgramOptions>
    {
        private WashProgram program;

        public GetDefaultOptions(WashProgram program)
        {
            this.program = program;
        }

        public byte[] GetRequest()
        {
            return new byte[2] { (byte)PacketType.GetDefaultOptions, (byte)program };
        }

        public ProgramOptions ParceResponse(byte[] data)
        {
            return new ProgramOptions
            {
                temperature = GetSingleOption(data, 1),
                duration = GetSingleOption(data, 2),
                washingSpeed = GetSingleOption(data, 3),
                spinningSpeed = GetSingleOption(data, 4),
                waterLevel = GetSingleOption(data, 5),
                rinsingCycles = GetSingleOption(data, 6),
            };
        }

        byte? GetSingleOption(byte[] data, int number)
        {
            if (data.Length <= number)
                return null;

            return data[number] == 0xFF ? null : (byte?)data[number];
        }
    }
}
