using VerifyTests.DiffPlex;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyDiffPlex.Initialize(OutputType.Compact);
        VerifierSettings.InitializePlugins();
        Configurator.Formatters.Register("en-150", new LegacyFormatter());
        Configurator.Formatters.Register("en-001", new CapturingFractionalFormatter());
        Configurator.Formatters.Register("en-MY", new DerivedLegacyFormatter());
    }

    sealed class LegacyFormatter : IFormatter
    {
        readonly DefaultFormatter formatter = new("en");

        public string DateHumanize_Now() => formatter.DateHumanize_Now();
        public string DateHumanize_Never() => formatter.DateHumanize_Never();
        public string DateHumanize(TimeUnit timeUnit, Tense timeUnitTense, int unit) =>
            formatter.DateHumanize(timeUnit, timeUnitTense, unit);
        public string TimeSpanHumanize_Zero() => formatter.TimeSpanHumanize_Zero();
        public string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false) =>
            formatter.TimeSpanHumanize(timeUnit, unit, toWords);
        public string TimeSpanHumanize_Age() => formatter.TimeSpanHumanize_Age();
        public string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
            formatter.DataUnitHumanize(dataUnit, count, toSymbol);
        public string TimeUnitHumanize(TimeUnit timeUnit) => formatter.TimeUnitHumanize(timeUnit);
    }

    internal sealed class CapturingFractionalFormatter() : DefaultFormatter("en"), IFractionalTimeSpanFormatter
    {
        public override string TimeSpanHumanizeWithFractionalSeconds(decimal seconds, bool toSymbols) =>
            FormattableString.Invariant($"{seconds}|{toSymbols}");
    }

    sealed class DerivedLegacyFormatter() : DefaultFormatter("en")
    {
        public override string TimeSpanHumanize(TimeUnit timeUnit, int unit, bool toWords = false) =>
            $"derived legacy {unit}";
    }
}