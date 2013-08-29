using System;
using System.Globalization;
using System.Threading;

namespace Humanizer.Tests.Extensions.Localisation
{
    public abstract class CultureSpecificTestBase : IDisposable
    {
        private readonly CultureInfo _culture;

        protected CultureSpecificTestBase(CultureInfo culture)
        {
            _culture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected CultureSpecificTestBase(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentUICulture = _culture;
        }
    }
}