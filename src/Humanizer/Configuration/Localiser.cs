using System;

namespace Humanizer.Configuration
{
    internal class Localiser<T>
    {
        public Localiser(string localeCode, Func<T> localiserFactory)
        {
            LocaleCode = localeCode;
            LocaliserFactory = localiserFactory;
        }

        public string LocaleCode { get; private set; }
        public Func<T> LocaliserFactory { get; private set; }
    }
}