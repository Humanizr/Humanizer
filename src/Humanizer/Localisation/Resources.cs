using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Humanizer.Localisation
{
    public class Resources
    {
        private static ResourceManager resourceMan;

        public static ResourceManager ResourceManager
        {
            get
            {
                return resourceMan ?? new ResourceManager("Humanizer.Properties.Resources", Assembly.GetExecutingAssembly());
            }
        }

        /// <summary>
        /// Returns a resource value from a culture specific resource
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns></returns>
        public static string GetResource(string resourceKey)
        {
            return ResourceManager.GetString(resourceKey, CultureInfo.CurrentUICulture);
        }
    }
}
