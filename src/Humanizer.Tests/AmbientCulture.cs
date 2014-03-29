using System;
using System.Globalization;
using System.Threading;

namespace Humanizer.Tests
{
    public class AmbientCulture : IDisposable
    {
        private readonly CultureInfo _callerCulture;

        public AmbientCulture(CultureInfo culture)
        {
            _callerCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public AmbientCulture(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = _callerCulture;
            Thread.CurrentThread.CurrentUICulture = _callerCulture;
        }
    }
}