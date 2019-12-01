using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Entity
{
    public class Status
    {
        public WashProgram program;

        public Stage stage;

        public byte temperature;

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
