using OpenWasherHardwareLibrary.Enums;
using WindowsFormsClient.Managers;

namespace WindowsFormsClient.Entities
{
    /// <summary>
    /// WashProgram wrapper for ListBox
    /// </summary>
    internal class WashingProgram
    {
        internal WashProgram Program { get; set; }

        public override string ToString()
        {
            return Localizator.GetString($"Program_{(byte)Program}", $"{Program}");
        }
    }
}
