using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Entity
{
    public class Status
    {
        public WashProgram program;

        public Stage stage;

        public byte temperature;

        public byte error;

        /// <summary>
        /// Spinning speed in RPS
        /// </summary>
        public double spinningSpeed;

        /// <summary>
        /// Full current program time, sec
        /// </summary>
        public int? timefull;

        /// <summary>
        /// Estimated current program time, sec
        /// </summary>
        public int? timepassed;
    }
}
