using OpenWasherHardwareLibrary.Enums;

namespace WindowsFormsClient.Entities
{
    /// <summary>
    /// WashProgram wrapper for ListBox
    /// </summary>
    internal class WashingProgram
    {
        internal string Name { get; }
        internal WashProgram Program { get; }

        internal WashingProgram(WashProgram program, string name)
        {
            Name = name;
            Program = program;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
