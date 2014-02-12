using System.Reflection;
using System.Resources;

namespace Humanizer.Localisation
{
    /// <summary>
    /// Provides access to the resources of Humanizer
    /// </summary>
    public static class Resources
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(
        "Humanizer.Properties.Resources",
        Assembly.GetExecutingAssembly());

        /// <summary>
        /// Returns the value of the specified string resource
        /// </summary>
        public static string GetResource(string resourceKey)
        {
            return ResourceManager.GetString(resourceKey);
        }
    }
}
