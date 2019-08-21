using IniParser;
using IniParser.Model;
using OpenWasherClient.Entities;

namespace WindowsFormsClient.Managers
{
    internal class ConfigManager
    {
        readonly FileIniDataParser parser = new FileIniDataParser();

        internal Config CurrentConfig { get; private set; }

        internal ConfigManager()
        {
            CurrentConfig = new Config();
            try
            {
                IniData data = parser.ReadFile("config.ini");

                CurrentConfig.Locale = data["General"]["Language"];
                CurrentConfig.Port = data["Connect"]["Port"];
                CurrentConfig.LogEnable = bool.Parse(data["Log"]["Enable"]);
            }
            catch
            {
                UpdateConfig();
            }
        }

        internal void UpdateConfig(Config config = null)
        {
            if (config == null)
                config = CurrentConfig;
            else
                CurrentConfig = config;

            IniData data = new IniData();

            data["General"]["Language"] = config.Locale;
            data["Connect"]["Port"] = config.Port;
            data["Log"]["Enable"] = config.LogEnable.ToString();            

            parser.WriteFile("config.ini", data);
        }
    }
}
