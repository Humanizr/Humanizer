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
        where TLocaliser : class
    {
        private readonly IDictionary<string, Func<CultureInfo, TLocaliser>> _localisers = new Dictionary<string, Func<CultureInfo, TLocaliser>>();
        private readonly Func<CultureInfo, TLocaliser> _defaultLocaliser;

        /// <summary>
        /// Creates a localiser registry with the default localiser set to the provided value
        /// </summary>
        /// <param name="defaultLocaliser"></param>
        public LocaliserRegistry(TLocaliser defaultLocaliser)
        {
            _defaultLocaliser = (culture) => defaultLocaliser;
        }

        /// <summary>
        /// Creates a localiser registry with the default localiser factory set to the provided value
        /// </summary>
        /// <param name="defaultLocaliser"></param>
        public LocaliserRegistry(Func<CultureInfo, TLocaliser> defaultLocaliser)
        {
            _defaultLocaliser = defaultLocaliser;
        }

        /// <summary>
        /// Gets the localiser for the current thread's UI culture 
        /// </summary>
        public TLocaliser ResolveForUiCulture()
        {
            return ResolveForCulture(null);
        }

        /// <summary>
        /// Gets the localiser for the specified culture 
        /// </summary>
        /// <param name="culture">The culture to retrieve localiser for. If not specified, current thread's UI culture is used.</param>
        public TLocaliser ResolveForCulture(CultureInfo culture)
        {
            return FindLocaliser(culture ?? CultureInfo.CurrentUICulture)(culture);
        }

        /// <summary>
        /// Registers the localiser for the culture provided 
        /// </summary>
        public void Register(string localeCode, TLocaliser localiser)
        {
            _localisers[localeCode] = (culture) => localiser;
        }

        /// <summary>
        /// Registers the localiser factory for the culture provided
        /// </summary>
        public void Register(string localeCode, Func<CultureInfo, TLocaliser> localiser)
        {
            _localisers[localeCode] = localiser;
        }

        private Func<CultureInfo, TLocaliser> FindLocaliser(CultureInfo culture)
        {
            Func<CultureInfo, TLocaliser> localiser;

            if (_localisers.TryGetValue(culture.Name, out localiser))
                return localiser;

            if (_localisers.TryGetValue(culture.TwoLetterISOLanguageName, out localiser))
                return localiser;

            return _defaultLocaliser;
        }
    }
}
