using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;

namespace OpenWasherHardwareLibrary.Commands
{
    public class GetStatus : IWasherCommand<Status>
    {
        public byte[] GetRequest()
        {
            return new byte[1] { 3 };
        }

        public Status ParceResponse(byte[] data)
        {
            if (data.Length != 12)
                throw new CommandException($"Bad status answer length: {data.Length} instead of 12");

            var status = new Status();
            status.program = (WashProgram)data[1];
            status.stage = (Stage)data[2];
            status.temperature = data[3];
            status.timefull = data[7] << 24 + data[6] << 16 + data[5] << 8 + data[4];
            status.timepassed = data[11] << 24 + data[10] << 16 + data[9] << 8 + data[8];

            return status;
        }
    }
}
