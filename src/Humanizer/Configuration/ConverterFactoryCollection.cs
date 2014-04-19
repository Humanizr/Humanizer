using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    public class ConverterFactoryCollection
    {
        private static Lazy<EnglishNumberToWordsConverter> _lazyEnglishNumberToWordsConverter = new Lazy<EnglishNumberToWordsConverter>();
        private static Lazy<ArabicNumberToWordsConverter> _lazyArabicNumberToWordsConverter = new Lazy<ArabicNumberToWordsConverter>();
        private static Lazy<FarsiNumberToWordsConverter> _lazyFarsiNumberToWordsConverter = new Lazy<FarsiNumberToWordsConverter>();
        private static Lazy<SpanishNumberToWordsConverter> _lazySpanishNumberToWordsConverter = new Lazy<SpanishNumberToWordsConverter>();
        private static Lazy<PolishNumberToWordsConverter> _lazyPolishNumberToWordsConverter = new Lazy<PolishNumberToWordsConverter>();
        private static Lazy<BrazilianPortugueseNumberToWordsConverter> _lazyBrazilianPortugueseNumberToWordsConverter = new Lazy<BrazilianPortugueseNumberToWordsConverter>();
        private static Lazy<RussianNumberToWordsConverter> _lazyRussianNumberToWordsConverter = new Lazy<RussianNumberToWordsConverter>();
        private static Lazy<FrenchNumberToWordsConverter> _lazyFrenchNumberToWordsConverter = new Lazy<FrenchNumberToWordsConverter>();
        private static Lazy<DutchNumberToWordsConverter> _lazyDutchNumberToWordsConverter = new Lazy<DutchNumberToWordsConverter>();
        private static Lazy<HebrewNumberToWordsConverter> _lazyHebrewNumberToWordsConverter = new Lazy<HebrewNumberToWordsConverter>();
        private static Lazy<DefaultNumberToWordsConverter> _lazyDefaultNumberToWordsConverter = new Lazy<DefaultNumberToWordsConverter>();

        private static readonly IDictionary<string, Func<DefaultNumberToWordsConverter>> _converterFactories =
            new Dictionary<string, Func<DefaultNumberToWordsConverter>>
            {
                {"en", () => _lazyEnglishNumberToWordsConverter.Value},
                {"ar", () => _lazyArabicNumberToWordsConverter.Value},
                {"fa", () => _lazyFarsiNumberToWordsConverter.Value},
                {"es", () => _lazySpanishNumberToWordsConverter.Value},
                {"pl", () => _lazyPolishNumberToWordsConverter.Value},
                {"pt-BR", () => _lazyBrazilianPortugueseNumberToWordsConverter.Value},
                {"ru", () => _lazyRussianNumberToWordsConverter.Value},
                {"fr", () => _lazyFrenchNumberToWordsConverter.Value},
                {"nl", () => _lazyDutchNumberToWordsConverter.Value},
                {"he", () => _lazyHebrewNumberToWordsConverter.Value},
                {"default", () => _lazyDefaultNumberToWordsConverter.Value}
            };

        public Func<DefaultNumberToWordsConverter> GetFactory(CultureInfo culture)
        {
            Func<DefaultNumberToWordsConverter> converterFactory;

            if (_converterFactories.TryGetValue(culture.Name, out converterFactory))
                return converterFactory;

            if (_converterFactories.TryGetValue(culture.TwoLetterISOLanguageName, out converterFactory))
                return converterFactory;

            return _converterFactories["default"];
        }

        public void SetFactory(CultureInfo culture, Func<DefaultNumberToWordsConverter> factory)
        {
            _converterFactories[culture.Name] = factory;
        }

        public void SetDefaultFactory(Func<DefaultNumberToWordsConverter> factory)
        {
            _converterFactories["default"] = factory;
        }
    }
}
