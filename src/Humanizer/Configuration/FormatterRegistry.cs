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
            Register("sr", new SerbianFormatter("sr"));
            Register("sr-Latn", new SerbianFormatter("sr-Latn"));
            RegisterCzechSlovakPolishFormatter("cs");
            RegisterCzechSlovakPolishFormatter("pl");
            RegisterCzechSlovakPolishFormatter("sk");
            RegisterDefaultFormatter("bg");
            RegisterDefaultFormatter("pt-BR");
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
            RegisterDefaultFormatter("fr");
            RegisterDefaultFormatter("fr-BE");
            RegisterDefaultFormatter("hu");
            RegisterDefaultFormatter("id");
            RegisterDefaultFormatter("ja");
            RegisterDefaultFormatter("nb");
            RegisterDefaultFormatter("nb-NO");
            RegisterDefaultFormatter("nl");
            RegisterDefaultFormatter("bn-BD");
            RegisterDefaultFormatter("it");
            RegisterDefaultFormatter("uz-Latn-UZ");
            RegisterDefaultFormatter("uz-Cyrl-UZ");
            RegisterDefaultFormatter("zh-CN");
            RegisterDefaultFormatter("zh-CHS");
            RegisterDefaultFormatter("zh-CHT");
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
