using System.IO;
using System.Reflection;

namespace Revida.Sitecore.Assurance.Console
{   
    public static class ConfigurationFileHelper
    {
        public static bool ConfigurationFileExists()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            
            return File.Exists(assembly.Location + ".config");
        }
    }
}
