using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using System;
using System.Collections.Generic;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class StartProgram : IWasherCommand
    {
        readonly WashProgram program;
        readonly ProgramOptions options;
        private readonly DateTime? startDate;

        public StartProgram(WashProgram program, ProgramOptions options = null, DateTime? startDate = null)
        {
            this.program = program;
            this.options = options;
            this.startDate = startDate;
        }

        public byte[] GetRequest()
        {
            var length = startDate.HasValue ? 15 : options != null ? 8 : 2;
            var data = new List<byte>(length) { (byte)PacketType.CommandStart, (byte)program };
            for(int i = 2; i<length; i++)
            {
                data.Add(Byte.MaxValue);
            }

            if (options != null)
            {
                data[2] = options.temperature ?? Byte.MaxValue;
                data[3] = options.duration ?? Byte.MaxValue;
                data[4] = options.washingSpeed ?? Byte.MaxValue;
                data[5] = options.spinningSpeed ?? Byte.MaxValue;
                data[6] = options.waterLevel ?? Byte.MaxValue;
                data[7] = options.rinsingCycles ?? Byte.MaxValue;
            }

            if (startDate != null)
            {
                data[8] = ConvertToBCD(startDate.Value.Second);
                data[9] = ConvertToBCD(startDate.Value.Minute);
                data[10] = ConvertToBCD(startDate.Value.Hour);
                data[11] = 0xFF;
                data[12] = ConvertToBCD(startDate.Value.Day);
                data[13] = ConvertToBCD(startDate.Value.Month);
                data[14] = ConvertToBCD(startDate.Value.Year % 100);
            }

            return data.ToArray();
        }

        private byte ConvertToBCD(int number)
        {
            if (number > 99)
                throw new CommandException($"Trying to convert {number} to 2-character BCD");

            return (byte)(((number / 10) << 4) + (number % 10));
        }
    }
}
