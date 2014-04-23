using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterFactoryManager : FactoryManager<IFormatter>
    {
        private static readonly Lazy<RomanianFormatter> _lazyRomanianFormatter = new Lazy<RomanianFormatter>();
        private static readonly Lazy<RussianFormatter> _lazyRussianFormatter = new Lazy<RussianFormatter>();
        private static readonly Lazy<ArabicFormatter> _lazyArabicFormatter = new Lazy<ArabicFormatter>();
        private static readonly Lazy<HebrewFormatter> _lazyHebrewFormatter = new Lazy<HebrewFormatter>();
        private static readonly Lazy<CzechSlovakPolishFormatter> _lazyCzechSlovakPolishFormatter = new Lazy<CzechSlovakPolishFormatter>();
        private static readonly Lazy<DefaultFormatter> _defaultFormatter = new Lazy<DefaultFormatter>();

        public FormatterFactoryManager()
            : base(
                new Dictionary<string, Func<IFormatter>>
        {
            { "ro", () => _lazyRomanianFormatter.Value },
            { "ru", () => _lazyRussianFormatter.Value },
            { "ar", () => _lazyArabicFormatter.Value },
            { "he", () => _lazyHebrewFormatter.Value },
            { "sk", () => _lazyCzechSlovakPolishFormatter.Value },
            { "cs", () => _lazyCzechSlovakPolishFormatter.Value },
            { "pl", () => _lazyCzechSlovakPolishFormatter.Value }
        })
        {
            SetDefaultFactory(() => _defaultFormatter.Value);
        }
    }
}
