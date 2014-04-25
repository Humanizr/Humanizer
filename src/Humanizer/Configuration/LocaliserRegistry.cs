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
        private readonly IDictionary<string, Lazy<T>> _localisers;

        private Lazy<T> _defaultLocaliser;

        internal LocaliserRegistry(Func<T> defaultLocaliser, params Localiser<T>[] localisers)
        {
            _defaultLocaliser = MakeLazy(defaultLocaliser);
            _localisers = new Dictionary<string, Lazy<T>>();
            foreach (var localiser in localisers)
                _localisers.Add(localiser.LocaleCode, MakeLazy(localiser.LocaliserFactory));
        }

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
        /// Set the localiser for the culture provided or the default localiser if the culture is not provided
        /// </summary>
        public void Register(Func<T> localiser, CultureInfo culture = null)
        {
            var lazyLocaliser = new Lazy<T>(localiser);

            if (culture == null)
                _defaultLocaliser = lazyLocaliser;
            else
                _localisers[culture.Name] = lazyLocaliser;
        }
    }
}
