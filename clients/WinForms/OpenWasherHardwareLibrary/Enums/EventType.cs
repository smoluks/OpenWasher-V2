namespace OpenWasherHardwareLibrary.Enums
{
    public enum EventType
    {
        PowerOn = 0x01,
        StartProgram = 0x10,
        StopProgram = 0x11,
        BreakProgram = 0x12,
        GoToBootloader = 0x8A,
    }
}
