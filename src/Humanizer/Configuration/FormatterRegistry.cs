using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(new DefaultFormatter("en-US"))
        {
            Register("ar", new ArabicFormatter());
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
            RegisterDefaultFormatter("pt");
            RegisterDefaultFormatter("sv");
            RegisterDefaultFormatter("tr");
            RegisterDefaultFormatter("vi");
            RegisterDefaultFormatter("en-US");
            RegisterDefaultFormatter("af");
            RegisterDefaultFormatter("da");
            RegisterDefaultFormatter("de");
            RegisterDefaultFormatter("el");
            RegisterDefaultFormatter("es");
            RegisterDefaultFormatter("fa");
            RegisterDefaultFormatter("fi-FI");
            RegisterDefaultFormatter("hu");
            RegisterDefaultFormatter("id");
            RegisterDefaultFormatter("ja");
            Register("mt", new MalteseFormatter("mt"));
            RegisterDefaultFormatter("nb");
            RegisterDefaultFormatter("nb-NO");
            RegisterDefaultFormatter("nl");
            RegisterDefaultFormatter("bn-BD");
            RegisterDefaultFormatter("it");
            RegisterDefaultFormatter("uz-Latn-UZ");
            RegisterDefaultFormatter("uz-Cyrl-UZ");
            RegisterDefaultFormatter("zh-CN");
            RegisterDefaultFormatter("zh-Hans");
            RegisterDefaultFormatter("zh-Hant");
        }

        private void RegisterDefaultFormatter(string localeCode)
        {
            Register(localeCode, new DefaultFormatter(localeCode));
        }

        private void RegisterCzechSlovakPolishFormatter(string localeCode)
        {
            Register(localeCode, new CzechSlovakPolishFormatter(localeCode));
        }
    }
}
