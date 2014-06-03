using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(new DefaultFormatter("en-US"))
        {
            RegisterDefaultFormatter("af");
            Register<ArabicFormatter>("ar");
            RegisterDefaultFormatter("bg");
            RegisterCzechSlovakPolishFormatter("cs");
            RegisterDefaultFormatter("da");
            RegisterDefaultFormatter("de");
            RegisterDefaultFormatter("el");
            RegisterDefaultFormatter("es");
            RegisterDefaultFormatter("fa");
            RegisterDefaultFormatter("fi-FI");
            RegisterDefaultFormatter("fr");
            RegisterDefaultFormatter("fr-BE");
            Register<HebrewFormatter>("he");
            RegisterDefaultFormatter("hu");
            RegisterDefaultFormatter("id");
            RegisterDefaultFormatter("ja");
            RegisterDefaultFormatter("nb");
            RegisterDefaultFormatter("nb-NO");
            RegisterDefaultFormatter("nl");
            RegisterCzechSlovakPolishFormatter("pl");
            RegisterDefaultFormatter("pt-BR");
            Register<RomanianFormatter>("ro");
            Register<RussianFormatter>("ru");
            RegisterCzechSlovakPolishFormatter("sk");
            Register<SlovenianFormatter>("sl");
            RegisterSerbianFormatter("sr");
            RegisterSerbianFormatter("sr-Latn");
            RegisterDefaultFormatter("sv");
            RegisterDefaultFormatter("tr");
            RegisterDefaultFormatter("vi");
        }

        private void RegisterDefaultFormatter(string localeCode)
        {
            Register(() => new DefaultFormatter(localeCode), localeCode);
        }

        private void RegisterCzechSlovakPolishFormatter(string localeCode)
        {
            Register(() => new CzechSlovakPolishFormatter(localeCode), localeCode);
        }

        private void RegisterSerbianFormatter(string localeCode)
        {
            Register(() => new SerbianFormatter(localeCode), localeCode);
        }
    }
}