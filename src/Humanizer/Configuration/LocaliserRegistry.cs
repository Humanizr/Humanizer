using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Configuration
{
    /// <summary>
    /// A registry of localised system components with their associated locales
    /// </summary>
    /// <typeparam name="TLocaliser"></typeparam>
    public class LocaliserRegistry<TLocaliser> 
    {
        private readonly IDictionary<string, Lazy<TLocaliser>> _localisers = new Dictionary<string, Lazy<TLocaliser>>();
        private TLocaliser _defaultLocaliser;

        /// <summary>
        /// Creates a localiser registry with the default localiser set to the provided value
        /// </summary>
        /// <param name="defaultLocaliser"></param>
        public LocaliserRegistry(TLocaliser defaultLocaliser)
        {
            _defaultLocaliser = defaultLocaliser;
        }

        /// <summary>
        /// Gets the localiser for the current thread's UI culture 
        /// </summary>
        public TLocaliser ResolveForUiCulture()
        {
            return ResolveForCulture();
        }

        /// <summary>
        /// Gets the localiser for the specified culture 
        /// </summary>
        /// <param name="culture">The culture to retrieve localiser for. If not specified, current thread's UI culture is used.</param>
        public TLocaliser ResolveForCulture(CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;

            Lazy<TLocaliser> factory;

            if (_localisers.TryGetValue(culture.Name, out factory))
                return factory.Value;

            if (_localisers.TryGetValue(culture.TwoLetterISOLanguageName, out factory))
                return factory.Value;

            return _defaultLocaliser;
        }

        /// <summary>
        /// Registers the localiser for the culture provided 
        /// </summary>
        public void Register<T>(string localeCode)
            where T: TLocaliser, new()
        {
            _localisers[localeCode] = new Lazy<TLocaliser>(() => new T());
        }

        /// <summary>
        /// Registers the localiser for the culture provided 
        /// </summary>
        public void Register(Func<TLocaliser> localiserFactory, string localeCode)
        {
            _localisers[localeCode] = new Lazy<TLocaliser>(localiserFactory);
        }

        /// <summary>
        /// Registers the localiser as the catch all 
        /// </summary>
        public void RegisterDefault(TLocaliser defaultLocaliser)
        {
            _defaultLocaliser = defaultLocaliser;
        }
    }
}
