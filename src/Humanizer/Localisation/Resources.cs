using System.Globalization;
using System.Reflection;

namespace Humanizer.Localisation
{
    public class Resources
    {
        private static System.Resources.ResourceManager _resourceMan;

        public static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (ReferenceEquals(_resourceMan, null))
                {
                    var temp = new System.Resources.ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);
                    _resourceMan = temp;
                }
                return _resourceMan;
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
