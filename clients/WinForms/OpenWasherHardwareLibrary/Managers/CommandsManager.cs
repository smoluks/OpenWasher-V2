using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenWasherHardwareLibrary.HardwareLibrary;

namespace OpenWasherHardwareLibrary.Managers
{
    internal class CommandsManager : IDisposable
    {
        private IOManager io;
        private bool connected;
        private readonly MessageReceivedDelegate messageDelegate;
        private readonly ErrorReceivedDelegate errorDelegate;
        private readonly EventReceivedDelegate eventDelegate;

        internal static IEnumerable<string> AvaliableComPorts => IOManager.GetAvaliableComPorts();

        internal CommandsManager(
            MessageReceivedDelegate messageDelegate = null,
            ErrorReceivedDelegate errorDelegate = null,
            EventReceivedDelegate eventDelegate = null)
        {
            this.messageDelegate = messageDelegate;
            this.errorDelegate = errorDelegate;
            this.eventDelegate = eventDelegate;
        }

        internal async Task<string> ConnectAsync(string port = null)
        {
            if(io != null)
                io.Dispose();

            if (port != null)
            {
                io = new IOManager(port, messageDelegate, errorDelegate, eventDelegate);
                Thread.Sleep(3000);
                await SendPingAsync(10000);
                connected = true;

                return io.Port;
            }
            else
            {
                var ports = IOManager.GetAvaliableComPorts();
                io = ports.AsParallel().Select(x =>
                {
                    try
                    {
                        io = new IOManager(x, messageDelegate, errorDelegate, eventDelegate);
                        Thread.Sleep(3000);
                        SendPingAsync(10000).GetAwaiter().GetResult();
                        return io;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .FirstOrDefault();

                if (io != null)
                    connected = true;

                return io?.Port;
            }
        }

        internal void Disconnect()
        {
            if (io != null)
                io.Dispose();
        }

        public void Dispose()
        {
            if (io != null)
                io.Dispose();
        }

        internal async Task SendPingAsync(int timeout = 500)
        {
            await io.SendAsync(new byte[0], timeout);
        }

        internal async Task StartProgramAsync(Programs program, ProgramOptions options = null)
        {
            byte[] data;
            if (options == null)
            {
                data = new byte[2] { 1, (byte)program};
            }
            else
            {
                data = new byte[8] { 1, (byte)program, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
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
            }

            var result = await io.SendAsync(data, 500);
        }

        internal async Task StopProgramAsync()
        {
            var result = await io.SendAsync(new byte[1] { 2 }, 500);
        }

        internal async Task<Status> GetStatusAsync()
        {
            var result = await io.SendAsync(new byte[1] { 3 }, 3000);
            if (result.Length != 12)
                throw new CommandException($"Bad status answer length: {result.Length} instead of 12");

            var status = new Status();
            status.program = (Programs)result[1];
            status.stage = (Stage)result[2];
            status.temperature = result[3];
            status.timefull = result[7] << 24 + result[6] << 16 + result[5] << 8 + result[4];
            status.timepassed = result[11] << 24 + result[10] << 16 + result[9] << 8 + result[8];

            return status;
        }

        internal async Task GoToBootloaderAsync()
        {
            var result = await io.SendAsync(new byte[1] { 0x0A }, 5000);
        }
        
    }
}
