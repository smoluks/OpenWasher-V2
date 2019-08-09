using OpenWasherHardwareLibrary.Entity;
using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Managers
{
    public static class ParamsManager
    {
        public static ProgramOptions GetDefault(WashProgram program)
        {
            if ((int)program < 1 || (int)program > 11)
                return null;

            int number = (int)program - 1;
            return new ProgramOptions
            {
                temperature = washtemperature[number],
                duration = washdelay[number],
                spinningspeed = spinningrps[number],
                waterlevel = 10,
                rinsingCycles = 3
            };
        }

        static byte[] washtemperature = new byte[]{
        80, 80, 60, 40,
        30, 60, 50, 40,
        30, 40, 30
        };
        static byte[] washdelay = new byte[]{
        137, 120, 105, 72,
        60, 77, 73, 58,
        30, 50, 45
        };
        static byte[] spinningrps = new byte[]{
        20, 20, 20, 20,
        20, 5, 5, 5,
        5, 5, 0
        };
    }
}
