using System;
using System.Collections.Generic;
using System.Globalization;
using Humanizer.Localisation.Ordinalizers;

namespace Humanizer.Configuration
{
    public class OrdinalizerFactoryCollection : FactoryCollection<IOrdinalizer>
    {
        private static Lazy<EnglishOrdinalizer> _lazyEnglishOrdinalizer = new Lazy<EnglishOrdinalizer>();
        private static Lazy<SpanishOrdinalizer> _lazySpanishOrdinalizer = new Lazy<SpanishOrdinalizer>();
        private static Lazy<BrazilianPortugueseOrdinalizer> _lazyBrazilianPortugueseOrdinalizer = new Lazy<BrazilianPortugueseOrdinalizer>();
        private static Lazy<RussianOrdinalizer> _lazyRussianOrdinalizer = new Lazy<RussianOrdinalizer>();
        private static Lazy<DefaultOrdinalizer> _lazyDefaultOrdinalizer = new Lazy<DefaultOrdinalizer>();

        public OrdinalizerFactoryCollection() : base(
            new Dictionary<string, Func<IOrdinalizer>>(StringComparer.OrdinalIgnoreCase)
            {
                {"en", () => _lazyEnglishOrdinalizer.Value},
                {"es", () => _lazySpanishOrdinalizer.Value},
                {"pt-BR", () => _lazyBrazilianPortugueseOrdinalizer.Value},
                {"ru", () => _lazyRussianOrdinalizer.Value},
                {"default", () => _lazyDefaultOrdinalizer.Value}
            }) { }
    }
}
