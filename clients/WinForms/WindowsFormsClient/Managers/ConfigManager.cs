using IniParser;
using IniParser.Model;
using System;

namespace WindowsFormsClient.Managers
{
    internal class ConfigManager
    {
        FileIniDataParser parser = new FileIniDataParser();
        internal string Port = "AUTO";

        internal ConfigManager()
        {
            try
            {
                IniData data = parser.ReadFile("config.ini");
                Port = data["Connect"]["Port"];
            }
            catch(Exception e)
            {
                Save();
            }
        }

        internal void Save()
        {
            IniData data = new IniData();
            data["Connect"]["Port"] = Port;
            parser.WriteFile("config.ini", data);
        }
    }
}
