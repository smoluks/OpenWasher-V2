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
            if (data.Length != 5 && data.Length != 13)
                throw new CommandException($"Bad status answer length: {data.Length} instead of 5/13");

            var status = new Status();
            status.program = (WashProgram)data[1];
            status.stage = (Stage)data[2];
            status.temperature = data[3];
            status.spinningSpeed= (double)data[4] / 16;

            if (data.Length == 13)
            {
                status.timefull = data[8] << 24 + data[7] << 16 + data[6] << 8 + data[5];
                status.timepassed = data[12] << 24 + data[11] << 16 + data[10] << 8 + data[9];
            }

            return status;
        }
    }
}
