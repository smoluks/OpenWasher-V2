namespace OpenWasherHardwareLibrary.Enums
{
    public enum PacketType
    {
        Ping = 0x00,
        CommandStart = 0x01,
        CommandStop = 0x02,
        GetDefaultOptions = 0x03,
        EnterProgrammingMode = 0x0A,
        GetStatus = 0x10,
        GetSetConfig = 0x20,
        GetSetTime = 0x30,
        Event = 0x5E,
        Error = 0x5B,
        Message = 0x58,
    };
}
