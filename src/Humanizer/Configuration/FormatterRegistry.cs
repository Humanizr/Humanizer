using Humanizer.Localisation;
using Humanizer.Localisation.Arabic;
using Humanizer.Localisation.Hebrew;
using Humanizer.Localisation.Romanian;
using Humanizer.Localisation.Russian;
using Humanizer.Localisation.Serbian;
using Humanizer.Localisation.Slovenian;

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