#if NET6_0_OR_GREATER

using Humanizer.Localisation.TimeToClockNotation;

namespace Humanizer.Configuration
{
    internal class TimeOnlyToClockNotationConvertersRegistry : LocaliserRegistry<ITimeOnlyToClockNotationConverter>
    {
        public TimeOnlyToClockNotationConvertersRegistry() : base(new TimeOnlyToClockNotationConverter())
        {
            Register("en-US", new TimeOnlyToClockNotationConverter());
            Register("en-UK", new TimeOnlyToClockNotationConverter());
            Register("de",    new TimeOnlyToClockNotationConverter());
            Register("pt-BR", new TimeOnlyToClockNotationConverter());
        }
    }
}

#endif
