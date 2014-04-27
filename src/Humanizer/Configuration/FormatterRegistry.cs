using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(new DefaultFormatter())
        {
            Register<RomanianFormatter>("ro");
            Register<RussianFormatter>("ru");
            Register<ArabicFormatter>("ar");
            Register<HebrewFormatter>("he");
            Register<CzechSlovakPolishFormatter>("sk");
            Register<CzechSlovakPolishFormatter>("cs");
            Register<CzechSlovakPolishFormatter>("pl");
            Register<SerbianFormatter>("sr");
            Register<SlovenianFormatter>("sl");
        }
    }
}