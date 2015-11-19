using System;
using System.Globalization;
using System.Threading;

namespace Humanizer.Tests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
        Justification = "This is a test only class, and doesn't need a 'proper' IDisposable implementation.")]
    public class AmbientCulture : IDisposable
    {
        private readonly CultureInfo _culture;

        public AmbientCulture(CultureInfo culture)
        {
            _culture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public AmbientCulture(string cultureName)
            : this(new CultureInfo(cultureName))
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly",
            Justification="This is a test only class, and doesn't need a 'proper' IDisposable implementation.")]
        public void Dispose()
        {
            Thread.CurrentThread.CurrentUICulture = _culture;
            Thread.CurrentThread.CurrentCulture = _culture;
        }
    }
}