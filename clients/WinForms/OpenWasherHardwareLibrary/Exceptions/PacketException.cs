using System;

namespace OpenWasherHardwareLibrary.Exceptions
{
    public class PacketException : Exception
    {
        public PacketException()
        {
        }

        public PacketException(string message)
            : base(message)
        {
        }

        public PacketException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
