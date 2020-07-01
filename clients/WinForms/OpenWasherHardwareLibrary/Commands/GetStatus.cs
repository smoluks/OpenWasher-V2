using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class GetStatus : IWasherCommand<Status>
    {
        public byte[] GetRequest()
        {
            return new byte[1] { (byte)PacketType.GetStatus };
        }

        public Status ParceResponse(byte[] data)
        {
            if (data.Length != 6 && data.Length != 14)
                throw new CommandException($"Bad status answer length: {data.Length} instead of 5/13");

            var status = new Status();
            status.program = (WashProgram)data[1];
            status.stage = (Stage)data[2];
            status.temperature = data[3];
            status.spinningSpeed= (double)data[4] / 16;
            status.error = data[5];

            if (data.Length == 14)
            {
                status.timefull = data[9] << 24 + data[8] << 16 + data[7] << 8 + data[6];
                status.timepassed = data[13] << 24 + data[12] << 16 + data[11] << 8 + data[10];
            }

            return status;
        }
    }
}
