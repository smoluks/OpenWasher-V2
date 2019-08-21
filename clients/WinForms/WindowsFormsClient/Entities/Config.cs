using System;

namespace OpenWasherClient.Entities
{
    internal class Config
    {
        internal string Port = "AUTO";
        internal bool LogEnable;
        internal string Locale;

        public Config Clone()
        {
            return new Config
            {
                Port = this.Port,
                LogEnable = this.LogEnable,
                Locale = this.Locale,
            };
        }
    }
}
