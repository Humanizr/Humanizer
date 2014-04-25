using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Configuration
{
    /// <summary>
    /// A registry of localised system components with their associated locales
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocaliserRegistry<T> 
    {
        private readonly IDictionary<string, Lazy<T>> _localisers = new Dictionary<string, Lazy<T>>();
        private Lazy<T> _defaultLocaliser;

        Lazy<T> MakeLazy(Func<T> factoryMethod)
        {
            return new Lazy<T>(factoryMethod);
        }

        /// <summary>
        /// Gets the localiser for the current UI culture 
        /// </summary>
        public T ResolveForUiCulture()
        {
            var culture = CultureInfo.CurrentUICulture;

            Lazy<T> factory;

            if (_localisers.TryGetValue(culture.Name, out factory))
                return factory.Value;

            if (_localisers.TryGetValue(culture.TwoLetterISOLanguageName, out factory))
                return factory.Value;

            return _defaultLocaliser.Value;
        }

        /// <summary>
        /// Registers the localiser for the culture provided 
        /// </summary>
        public void Register<TLocaliser>(string localeCode)
            where TLocaliser: T, new()
        {
            _localisers[localeCode] = MakeLazy(() => new TLocaliser());
        }

        /// <summary>
        /// Registers the localiser as the catch all 
        /// </summary>
        public void RegisterDefault<TLocaliser>()
            where TLocaliser: T, new ()
        {
            _defaultLocaliser = MakeLazy(() => new TLocaliser());
        }
    }
}
