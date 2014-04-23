using System;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Humanizer.Configuration
{
    public class FactoryManager<T>
    {
        private IDictionary<string, Func<T>> _factories;

        public FactoryManager(IDictionary<string, Func<T>> factories)
        {
            _factories = factories;
        }

        /// <summary>
        /// Gets the factory for the CultureInfo provided.
        /// If no culture is provided, CultureInfo.CurrentUICulture will be used
        /// </summary>
        public Func<T> GetFactory(CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;

            Func<T> factory;

            if (_factories.TryGetValue(culture.Name, out factory))
                return factory;

            if (_factories.TryGetValue(culture.TwoLetterISOLanguageName, out factory))
                return factory;

            return _factories["default"];
        }

        /// <summary>
        /// Set the factory for the culture provided.
        /// </summary>
        public void SetFactory(Func<T> factory, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;

            _factories[culture.Name] = factory;
        }

        /// <summary>
        /// Set the default factory for when a culture does not have a
        /// specific factory set.
        /// </summary>
        public void SetDefaultFactory(Func<T> factory)
        {
            _factories["default"] = factory;
        }

    }
}
