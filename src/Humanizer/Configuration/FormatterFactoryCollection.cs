using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    public class FormatterFactoryCollection : FactoryCollection<IFormatter>
    {
        private static Lazy<RomanianFormatter> _lazyRomanianFormatter = new Lazy<RomanianFormatter>();
        private static Lazy<RussianFormatter> _lazyRussianFormatter = new Lazy<RussianFormatter>();
        private static Lazy<ArabicFormatter> _lazyArabicFormatter = new Lazy<ArabicFormatter>();
        private static Lazy<HebrewFormatter> _lazyHebrewFormatter = new Lazy<HebrewFormatter>();
        private static Lazy<CzechSlovakPolishFormatter> _lazyCzechSlovakPolishFormatter = new Lazy<CzechSlovakPolishFormatter>();
        private static Lazy<DefaultFormatter> _defaultFormatter = new Lazy<DefaultFormatter>();

        public FormatterFactoryCollection() : base(
            new Dictionary<string, Func<IFormatter>>(StringComparer.OrdinalIgnoreCase)
        {
            { "ro", () => _lazyRomanianFormatter.Value },
            { "ru", () => _lazyRussianFormatter.Value },
            { "ar", () => _lazyArabicFormatter.Value },
            { "he", () => _lazyHebrewFormatter.Value },
            { "sk", () => _lazyCzechSlovakPolishFormatter.Value },
            { "cs", () => _lazyCzechSlovakPolishFormatter.Value },
            { "pl", () => _lazyCzechSlovakPolishFormatter.Value },
            { "default", () => _defaultFormatter.Value }
        }) { }
    }
}
