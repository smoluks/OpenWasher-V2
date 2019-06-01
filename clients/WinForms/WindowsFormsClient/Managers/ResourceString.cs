using WindowsFormsClient.Properties;

namespace WindowsFormsClient.Managers
{
    internal static class ResourceString
    {
        internal static string GetString(string name, string _default = null)
        {
            var result = Resources.ResourceManager.GetString(name);
            if (result != null)
                return result;
            else return _default;
        }
    }
}
