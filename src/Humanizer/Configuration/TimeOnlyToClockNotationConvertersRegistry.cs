#if NET6_0_OR_GREATER

using Humanizer.Localisation.TimeToClockNotation;

namespace Humanizer.Configuration
{
    internal class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
    {
        public TimeOnlyToClockNotationConvertersRegistry() : base(new DefaultTimeOnlyToClockNotationConverter())
        {
            Register("pt-BR", new BrazilianPortugueseTimeOnlyToClockNotationConverter());
            Register("fr", new FrTimeOnlyToClockNotationConverter());
            Register("es", new EsTimeOnlyToClockNotationConverter());
        }
    }
}

#endif
