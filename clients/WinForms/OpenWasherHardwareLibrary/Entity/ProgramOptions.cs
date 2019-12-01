namespace OpenWasherHardwareLibrary.Entity
{
    /// <summary>
    /// Washing program configuration
    /// </summary>
    public class ProgramOptions
    {
        /// <summary>
        /// Water temperature in main cycle, ºC
        /// </summary>
        public byte? temperature = null;

        /// <summary>
        /// Main washing cycle duration, min
        /// </summary>
        public byte? duration = null;

        /// <summary>
        /// max rotate speed in washing, RPM
        /// </summary>
        public byte? washingSpeed = null;

        /// <summary>
        /// max rotate speed in spinning, RPM
        /// </summary>
        public byte? spinningSpeed = null;

        /// <summary>
        /// water level, %
        /// </summary>
        public byte? waterLevel = null;

        /// <summary>
        /// rinsing cycle count
        /// </summary>
        public byte? rinsingCycles = null;
    }
}
