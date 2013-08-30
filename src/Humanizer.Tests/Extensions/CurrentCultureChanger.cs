using System;
using System.Globalization;
using System.Threading;

namespace Humanizer.Tests.Extensions
{
    /// <summary>
    /// Temporarily change the CurrentUICulture and restore it back
    /// </summary>
    internal class CurrentCultureChanger: IDisposable
    {
        private readonly CultureInfo initialCulture;
        private readonly Thread initialThread;

        public CurrentCultureChanger(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");

            initialThread = Thread.CurrentThread;
            initialCulture = initialThread.CurrentUICulture;

            initialThread.CurrentUICulture = cultureInfo;
        }

        public void Dispose()
        {
            initialThread.CurrentUICulture = initialCulture;
        }
    }
}