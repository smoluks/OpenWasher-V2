using System.IO;
using System.Linq;
using System.Threading;
using WindowsFormsClient.Properties;

namespace WindowsFormsClient.Managers
{
    internal class ResourceManager
    {
        internal ResourceManager()
        {
            var locLibraries = Directory.GetFiles(@"\", "Localization_*.dll");
            if (locLibraries.Length == 0)
                return;

            string path = null;
            if (locLibraries.Length > 1)
            {
                var currectCulture = Thread.CurrentThread.CurrentCulture.EnglishName;
                path = locLibraries.Where(x => x == $"Localization_{currectCulture}.dll").FirstOrDefault();
                if (path == null)
                    path = locLibraries[0];
            }
            else
                path = locLibraries[0];
        }

        internal static string GetString(string name, string _default = null)
        {
            var result = Resources.ResourceManager.GetString(name);
            if (result != null)
                return result;
            else return _default;
        }
    }
}
