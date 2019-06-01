using System.ComponentModel;

namespace OpenWasherHardwareLibrary.Enums
{
    public enum Stage
    {
        [Description("Stop")]
        Stop = 0,
        [Description("Lock door")]
        LockDoor = 1,
        [Description("Unlock door")]
        UnlockDoor = 2,
        [Description("Rinsing")]
        Rinsing = 3,
        [Description("Spinning")]
        Spinning = 4,
        [Description("Prewashing")]
        Prewashing = 5,
        [Description("Washing")]
        Washing = 6,
        [Description("Draw water")]
        DrawWater = 7,
        [Description("Sink")]
        Sink = 8,
    }
}
