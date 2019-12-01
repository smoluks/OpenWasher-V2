using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using System;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class GetTime : IWasherCommand<(DateTime time, sbyte correction)>
    {
        public byte[] GetRequest()
        {
            return new byte[1] { (byte)PacketType.GetSetTime };
        }

        public (DateTime time, sbyte correction) ParceResponse(byte[] data)
        {
            if (data.Length != 9)
                throw new CommandException($"Bad GetSetTime answer length: {data.Length} instead of 9");

            var time = new DateTime(
                ConvertBCDToInt(data[7]) + 2000,
                ConvertBCDToInt((byte)(data[6] & 0x1F)),
                ConvertBCDToInt(data[5]),
                ConvertBCDToInt(data[3]),
                ConvertBCDToInt(data[2]),
                ConvertBCDToInt(data[1]));

            return
            (
                time,
                (sbyte)data[8]
            ); 
        }

        private int ConvertBCDToInt(byte number)
        {
            return (number >> 4) * 10 + (number & 0x0F);
        }
    }
}
