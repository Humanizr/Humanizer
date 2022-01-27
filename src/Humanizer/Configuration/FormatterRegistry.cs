using System.Globalization;

using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(new DefaultFormatter("en-US"))
        {
            Register("ar", new ArabicFormatter());
            Register("de", new GermanFormatter());
            Register("he", new HebrewFormatter());
            Register("ro", new RomanianFormatter());
            Register("ru", new RussianFormatter());
            Register("sl", new SlovenianFormatter());
            Register("hr", new CroatianFormatter());
            Register("sr", new SerbianFormatter("sr"));
            Register("sr-Latn", new SerbianFormatter("sr-Latn"));
            Register("uk", new UkrainianFormatter());
            Register("fr", new FrenchFormatter("fr"));
            Register("fr-BE", new FrenchFormatter("fr-BE"));
            RegisterCzechSlovakPolishFormatter("cs");
            RegisterCzechSlovakPolishFormatter("pl");
            RegisterCzechSlovakPolishFormatter("sk");
            RegisterDefaultFormatter("bg");
            RegisterDefaultFormatter("ku");
            RegisterDefaultFormatter("pt");
            RegisterDefaultFormatter("sv");
            RegisterDefaultFormatter("tr");
            RegisterDefaultFormatter("vi");
            RegisterDefaultFormatter("en-US");
            RegisterDefaultFormatter("af");
            RegisterDefaultFormatter("az");
            RegisterDefaultFormatter("da");
            RegisterDefaultFormatter("el");
            RegisterDefaultFormatter("es");
            RegisterDefaultFormatter("fa");
            RegisterDefaultFormatter("fi-FI");
            RegisterDefaultFormatter("fil-PH");
            RegisterDefaultFormatter("hu");
            RegisterDefaultFormatter("hy");
            RegisterDefaultFormatter("id");
            Register("is", new IcelandicFormatter());
            RegisterDefaultFormatter("ja");
            RegisterDefaultFormatter("ko-KR");
            RegisterDefaultFormatter("lv");
            Register("mt", new MalteseFormatter("mt"));
            RegisterDefaultFormatter("ms-MY");
            RegisterDefaultFormatter("nb");
            RegisterDefaultFormatter("nb-NO");
            RegisterDefaultFormatter("nl");
            RegisterDefaultFormatter("bn-BD");
            RegisterDefaultFormatter("it");
            RegisterDefaultFormatter("ta");
            RegisterDefaultFormatter("uz-Latn-UZ");
            RegisterDefaultFormatter("uz-Cyrl-UZ");
            RegisterDefaultFormatter("zh-CN");
            RegisterDefaultFormatter("zh-Hans");
            RegisterDefaultFormatter("zh-Hant");
            RegisterDefaultFormatter("th-TH");
            RegisterDefaultFormatter("en-IN");
        }

        private void RegisterDefaultFormatter(string localeCode)
        {
            try
            {
                Register(localeCode, new DefaultFormatter(localeCode));
            }
            catch (CultureNotFoundException)
            {
                // Some OS's may not support the particular culture. Not much we can do for those.
            }
        }

        private void RegisterCzechSlovakPolishFormatter(string localeCode)
        {
            Register(localeCode, new CzechSlovakPolishFormatter(localeCode));
        }
    }
}
