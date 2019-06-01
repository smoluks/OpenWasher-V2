using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWasherHardwareLibrary
{
    internal class Commands : IDisposable
    {
        readonly IO io;

        internal static IEnumerable<string> GetAvaliableComPorts => IO.GetAvaliableComPorts();

        internal Commands(string portname)
        {
            io = new IO(portname);
        }

        public void Dispose()
        {
            io.Dispose();
        }

        internal static void SendPing()
        {

        }

        private static void SendPackage(IEnumerable<byte> data)
        {
            var array = new byte[data.Count() + 3];
            array[0] = 0xAB;
            array[1] = data.Count();
            array[data.Count() + 2] = GetCrc(array);
        }

        private static byte GetCrc(byte[] data, int offset = 0, int count = -1)
        {
            byte crc = 0;
            if (count < 0)
                count = data.Count();

            for (int index = offset; index < count; index++)
            {
                var currentByte = data[index];
                for (byte bitCounter = 0; bitCounter < 8; bitCounter++)
                {
                    if (((crc ^ currentByte) & 0x01) != 0)
                    {
                        crc = (byte)((crc >> 1) ^ 0x8C);
                    }
                    else
                    {
                        crc >>= 1;
                    }
                    currentByte >>= 1;
                }
            }
            return crc;
        }
    }
}
