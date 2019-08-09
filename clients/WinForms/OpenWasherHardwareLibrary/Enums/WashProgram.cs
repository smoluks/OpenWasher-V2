using System.ComponentModel;

namespace OpenWasherHardwareLibrary.Enums
{
    public enum WashProgram
    {
        SelfTest = 0,

        [Description("1")]
        P1 = 1,
        [Description("2")]
        P2 = 2,
        [Description("3")]
        P3 = 3,
        [Description("4")]
        P4 = 4,
        [Description("5")]
        P5 = 5,
        [Description("6")]
        P6 = 6,
        [Description("7")]
        P7 = 7,
        [Description("8")]
        P8 = 8,
        [Description("9")]
        P9 = 9,
        [Description("10")]
        P10 = 10,
        [Description("11")]
        P11 = 11,
        [Description("Rinsing")]
        Rinsing = 12,
        [Description("Delicate rinsing")]
        DelicateRinsing = 13,
        [Description("Spinning")]
        Spinning = 14,
        [Description("Delicate spinning")]
        DelicateSpinning = 15,
        [Description("Sink")]
        Sink = 16,
        [Description("Sink pipes")]
        SinkPipes = 17,
        [Description("water heater")]
        waterHeater = 18,
        [Description("Grinding brushes")]
        GrindingBrushes = 19,
        [Description("Calibrate PID")]
        CalibratePID = 20,

        Nothing = 255
    }
}
