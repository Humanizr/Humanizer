#if NET6_0_OR_GREATER

using Humanizer.Localisation.TimeToClockNotation;

namespace Humanizer.Configuration
{
    internal class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
    {
        public TimeOnlyToClockNotationConvertersRegistry() : base(new DefaultTimeOnlyToClockNotationConverter())
        {
            Register("en-US", new DefaultTimeOnlyToClockNotationConverter());
            Register("en-UK", new DefaultTimeOnlyToClockNotationConverter());
            Register("de",    new DefaultTimeOnlyToClockNotationConverter());
            Register("pt-BR", new PtBrTimeOnlyToClockNotationConverter());
        }
    }
}

#endif
