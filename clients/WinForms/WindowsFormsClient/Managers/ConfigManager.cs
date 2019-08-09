using IniParser;
using IniParser.Model;
using System;

namespace WindowsFormsClient.Managers
{
    internal class ConfigManager
    {
        FileIniDataParser parser = new FileIniDataParser();

        internal string Port = "AUTO";

        internal bool LogEnable;

        internal ConfigManager()
        {
            try
            {
                IniData data = parser.ReadFile("config.ini");

                Port = data["Connect"]["Port"];

                LogEnable = bool.Parse(data["Log"]["Enable"]);
            }
            catch
            {
                Save();
            }
        }

        internal void Save()
        {
            IniData data = new IniData();

            data["Connect"]["Port"] = Port;

            data["Log"]["Enable"] = LogEnable.ToString();

            parser.WriteFile("config.ini", data);
        }
    }
}
