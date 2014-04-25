using Humanizer.Localisation.Formatters;

namespace Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(
            () => new DefaultFormatter(),
            new Localiser<IFormatter>("ro", () => new RomanianFormatter()),
            new Localiser<IFormatter>("ru", () => new RussianFormatter()),
            new Localiser<IFormatter>("ar", () => new ArabicFormatter()),
            new Localiser<IFormatter>("he", () => new HebrewFormatter()),
            new Localiser<IFormatter>("sk", () => new CzechSlovakPolishFormatter()),
            new Localiser<IFormatter>("cs", () => new CzechSlovakPolishFormatter()),
            new Localiser<IFormatter>("pl", () => new CzechSlovakPolishFormatter()),
            new Localiser<IFormatter>("sr", () => new SerbianFormatter()))
        {
        }
    }
}