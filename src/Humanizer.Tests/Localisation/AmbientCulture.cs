using System;
using System.Globalization;
using System.Threading;

namespace Humanizer.Tests.Localisation
{
    public class AmbientCulture : IDisposable
    {
        private readonly CultureInfo _culture;

        public AmbientCulture(CultureInfo culture)
        {
            _culture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public AmbientCulture(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentUICulture = _culture;
        }
    }
}