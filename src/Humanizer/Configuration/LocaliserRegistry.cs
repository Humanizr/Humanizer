namespace Humanizer
{
    /// <summary>
    /// A registry of localised system components with their associated locales
    /// </summary>
    public class LocaliserRegistry<TLocaliser>
        where TLocaliser : class
    {
        readonly IDictionary<string, Func<CultureInfo, TLocaliser>> _localisers = new Dictionary<string, Func<CultureInfo, TLocaliser>>();
        readonly Func<CultureInfo, TLocaliser> _defaultLocaliser;

        /// <summary>
        /// Creates a localiser registry with the default localiser set to the provided value
        /// </summary>
        public LocaliserRegistry(TLocaliser defaultLocaliser) =>
            _defaultLocaliser = _ => defaultLocaliser;

        /// <summary>
        /// Creates a localiser registry with the default localiser factory set to the provided value
        /// </summary>
        public LocaliserRegistry(Func<CultureInfo, TLocaliser> defaultLocaliser) =>
            _defaultLocaliser = defaultLocaliser;

        /// <summary>
        /// Gets the localiser for the current thread's UI culture
        /// </summary>
        public TLocaliser ResolveForUiCulture() =>
            ResolveForCulture(null);

        /// <summary>
        /// Gets the localiser for the specified culture
        /// </summary>
        /// <param name="culture">The culture to retrieve localiser for. If not specified, current thread's UI culture is used.</param>
        public TLocaliser ResolveForCulture(CultureInfo culture) =>
            FindLocaliser(culture ?? CultureInfo.CurrentUICulture)(culture);

        /// <summary>
        /// Registers the localiser for the culture provided
        /// </summary>
        public void Register(string localeCode, TLocaliser localiser) =>
            _localisers[localeCode] = _ => localiser;

        /// <summary>
        /// Registers the localiser factory for the culture provided
        /// </summary>
        public void Register(string localeCode, Func<CultureInfo, TLocaliser> localiser) =>
            _localisers[localeCode] = localiser;

        Func<CultureInfo, TLocaliser> FindLocaliser(CultureInfo culture)
        {
            for (var c = culture; !string.IsNullOrEmpty(c?.Name); c = c.Parent)
            {
                if (_localisers.TryGetValue(c.Name, out var localiser))
                {
                    return localiser;
                }
            }

            return _defaultLocaliser;
        }
    }
}
