using OpenWasherHardwareLibrary.Enums;

namespace OpenWasherHardwareLibrary.Entity
{
    public class Status
    {
        public Programs program;

        public Stage stage;

        public byte temperature;

        /// <summary>
        /// Полная длительность программы, сек.
        /// </summary>
        public int timefull;

        /// <summary>
        /// Пройденное время программы, сек
        /// </summary>
        public int timepassed;
    }
}
