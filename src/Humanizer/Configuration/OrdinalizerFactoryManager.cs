using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    internal class OrdinalizerFactoryManager : FactoryManager<IOrdinalizer>
    {
        private static Lazy<EnglishOrdinalizer> _lazyEnglishOrdinalizer = new Lazy<EnglishOrdinalizer>();
        private static Lazy<SpanishOrdinalizer> _lazySpanishOrdinalizer = new Lazy<SpanishOrdinalizer>();
        private static Lazy<BrazilianPortugueseOrdinalizer> _lazyBrazilianPortugueseOrdinalizer = new Lazy<BrazilianPortugueseOrdinalizer>();
        private static Lazy<RussianOrdinalizer> _lazyRussianOrdinalizer = new Lazy<RussianOrdinalizer>();
        private static Lazy<DefaultOrdinalizer> _lazyDefaultOrdinalizer = new Lazy<DefaultOrdinalizer>();

        public OrdinalizerFactoryManager()
            : base(
                new Dictionary<string, Func<IOrdinalizer>>
            {
                {"en", () => _lazyEnglishOrdinalizer.Value},
                {"es", () => _lazySpanishOrdinalizer.Value},
                {"pt-BR", () => _lazyBrazilianPortugueseOrdinalizer.Value},
                {"ru", () => _lazyRussianOrdinalizer.Value}
            })
        {
            SetDefaultFactory(() => _lazyDefaultOrdinalizer.Value);
        }
    }
}
