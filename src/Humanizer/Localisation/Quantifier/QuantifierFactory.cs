using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Humanizer.Localisation.Quantifier
{
    internal class QuantifierFactory
    {
        internal static IQuantifier GetQuantifier(CultureInfo culture)
        {
            return GetQuantifier(culture.TwoLetterISOLanguageName);
        }

        internal static IQuantifier GetQuantifier()
        {
            return GetQuantifier(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
        }

        private static IQuantifier GetQuantifier(string twoLetterISOLanguageName)
        {
            switch (twoLetterISOLanguageName)
            {
                case "fa":
                    return new FarsiQuantifier();
                default:
                    return new DefaultQuantifier();
            }
        }

    }
}
