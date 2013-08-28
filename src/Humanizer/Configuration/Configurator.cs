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
        public static IFormatter Formatter 
        {
            get
            {
                // ToDo: when other formatters are created we should change this implementation to resolve the Formatter based on the current culture
                return new DefaultFormatter();
            }
        }
    }
}
