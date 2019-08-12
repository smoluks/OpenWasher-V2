using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace WindowsFormsClient.Managers
{
    internal class Localizator
    {
        private static ResourceManager resourceManager;

        internal Localizator(string locale = null)
        {
            if (string.IsNullOrWhiteSpace(locale))
                locale = CultureInfo.CurrentCulture.Name;

            Assembly assembly;
            if (File.Exists(@"Localization_{locale}.dll"))
            {
                assembly = Assembly.LoadFrom(@"Localization_{locale}.dll");
            }
            else
            {
                assembly = this.GetType().Assembly;                
            }

            string name = assembly.GetName().Name;
            resourceManager = new ResourceManager(name + ".Properties.Resources", assembly);
        }

        internal string GetString(string name, string _default = null)
        {
            var result = resourceManager.GetString(name);
            if (result != null)
                return result;
            else return _default;
        }

        internal static IEnumerable<string> GetAvaliableLanguages()
        {
            return Directory.GetFiles("", "Localization_*.dll").Select(x => x.Remove(13, x.Length - 17));
        }
    }
}
