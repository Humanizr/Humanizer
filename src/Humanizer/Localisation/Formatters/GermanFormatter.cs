namespace Humanizer.Localisation.Formatters
{
    internal class GermanFormatter : DefaultFormatter
    {
        public GermanFormatter()
            : base("de")
        {
        }

        /// <inheritdoc />
        public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');
        }
    }
}
