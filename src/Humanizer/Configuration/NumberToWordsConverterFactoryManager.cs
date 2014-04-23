using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.NumberToWords;

namespace Humanizer.Configuration
{
    internal class NumberToWordsConverterFactoryManager : FactoryManager<INumberToWordsConverter>
    {
        private static readonly Lazy<EnglishNumberToWordsConverter> _lazyEnglishNumberToWordsConverter = new Lazy<EnglishNumberToWordsConverter>();
        private static readonly Lazy<ArabicNumberToWordsConverter> _lazyArabicNumberToWordsConverter = new Lazy<ArabicNumberToWordsConverter>();
        private static readonly Lazy<FarsiNumberToWordsConverter> _lazyFarsiNumberToWordsConverter = new Lazy<FarsiNumberToWordsConverter>();
        private static readonly Lazy<SpanishNumberToWordsConverter> _lazySpanishNumberToWordsConverter = new Lazy<SpanishNumberToWordsConverter>();
        private static readonly Lazy<PolishNumberToWordsConverter> _lazyPolishNumberToWordsConverter = new Lazy<PolishNumberToWordsConverter>();
        private static readonly Lazy<BrazilianPortugueseNumberToWordsConverter> _lazyBrazilianPortugueseNumberToWordsConverter = new Lazy<BrazilianPortugueseNumberToWordsConverter>();
        private static readonly Lazy<RussianNumberToWordsConverter> _lazyRussianNumberToWordsConverter = new Lazy<RussianNumberToWordsConverter>();
        private static readonly Lazy<FrenchNumberToWordsConverter> _lazyFrenchNumberToWordsConverter = new Lazy<FrenchNumberToWordsConverter>();
        private static readonly Lazy<DutchNumberToWordsConverter> _lazyDutchNumberToWordsConverter = new Lazy<DutchNumberToWordsConverter>();
        private static readonly Lazy<HebrewNumberToWordsConverter> _lazyHebrewNumberToWordsConverter = new Lazy<HebrewNumberToWordsConverter>();
        private static readonly Lazy<DefaultNumberToWordsConverter> _lazyDefaultNumberToWordsConverter = new Lazy<DefaultNumberToWordsConverter>();

        public NumberToWordsConverterFactoryManager()
            : base(
                new Dictionary<string, Func<INumberToWordsConverter>>
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
                {"he", () => _lazyHebrewNumberToWordsConverter.Value}
            })
        {
            SetDefaultFactory(() => _lazyDefaultNumberToWordsConverter.Value);
        }
    }
}
