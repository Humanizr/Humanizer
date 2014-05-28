using System.Globalization;
using System.Resources;

namespace Humanizer.Localisation
{
    /// <summary>
    /// Provides access to the resources of Humanizer
    /// </summary>
    public static class Resources
    {
        static readonly ResourceManager ResourceManager = new ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);

        /// <summary>
        /// Returns the value of the specified string resource
        /// </summary>
        /// <param name="resourceKey">The name of the resource to retrieve.</param>
        /// <param name="culture">The culture of the resource to retrieve.</param>
        /// <returns>The value of the resource localized for the caller's current UI culture.</returns>
        public static string GetResource(string resourceKey, CultureInfo culture = null)
        {
            return ResourceManager.GetString(resourceKey, culture);
        }
    }
}
