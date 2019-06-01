namespace OpenWasherHardwareLibrary.Enums
{
    public enum AnswerType
    {
        Ping = 0x00,
        CommandStart = 0x01,
        CommandBreak = 0x02,
        EnterProgrammingMode = 0x0A,
        Event = 0x5E,
        Error = 0x5B,
        Message = 0x58,
    };
}
