using OpenWasherHardwareLibrary.Enums;
using OpenWasherHardwareLibrary.Exceptions;
using System.Collections.Generic;

namespace OpenWasherHardwareLibrary.Commands
{
    internal class SetConfig : IWasherCommand
    {
        private readonly byte[] config;

        public SetConfig(byte[] config)
        {
            if (config == null)
                throw new CommandException($"No config");

            if (config.Length > 254)
                throw new CommandException($"Too long config: {config.Length}");

            this.config = config;
        }

        public byte[] GetRequest()
        {
            var data = new List<byte>() { (byte)PacketType.GetSetConfig };
            data.AddRange(config);

            return data.ToArray();
        }
    }
}
