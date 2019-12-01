using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using System;
using System.Collections.Generic;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class SetTime : IWasherCommand
    {
        private readonly DateTime? time;
        private readonly sbyte? correction;

        public SetTime(DateTime? time = null, sbyte? correction = null)
        {
            this.time = time;
            this.correction = correction;
        }

        public byte[] GetRequest()
        {
            var data = new List<byte>(8) { (byte)PacketType.GetSetTime };
            data[1] = ConvertToBCD(time?.Second);
            data[2] = ConvertToBCD(time?.Minute);
            data[3] = ConvertToBCD(time?.Hour);
            data[4] = time == null ? (byte)0xFF : time.Value.DayOfWeek == DayOfWeek.Sunday ? (byte)7 : (byte)time.Value.DayOfWeek;
            data[5] = ConvertToBCD(time?.Day);
            data[6] = (byte)(ConvertToBCD(time?.Month) | (DateTime.IsLeapYear(2000) ? 0x20 : 0x00));
            data[7] = ConvertToBCD(time?.Year % 100);

            if (correction != null)
                data.Add((byte)correction.Value);

            return data.ToArray();
        }

        private byte ConvertToBCD(int? number)
        {
            if (number == null)
                return 0xFF;

            if (number > 99)
                throw new CommandException($"Trying to convert {number} to 2-character BCD");

            return (byte)(((number / 10) << 4) + (number % 10));
        }
    }
}
