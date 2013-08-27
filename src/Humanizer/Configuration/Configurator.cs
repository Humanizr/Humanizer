namespace Humanizer.Configuration
{
    /// <summary>
    /// Provides a configuration point for Humanizer
    /// </summary>
    public class Configurator
    {
        /// <summary>
        /// The formatter to be used 
        /// </summary>
        public static ILocaleAwareFormatter Formatter { get; set; }

        static Configurator()
        {
            // ToDo: when other formatters are created we should change this implementation to set the Formatter based on the current locale
            Formatter = new DefaultFormatter();
        }
    }
}
