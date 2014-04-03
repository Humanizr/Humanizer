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
        internal static DefaultQuantifier GetQuantifier(CultureInfo culture)
        {
            return GetQuantifier(culture.TwoLetterISOLanguageName);
        }

        internal static DefaultQuantifier GetQuantifier()
        {
            return GetQuantifier(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
        }

        private static DefaultQuantifier GetQuantifier(string twoLetterISOLanguageName)
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
