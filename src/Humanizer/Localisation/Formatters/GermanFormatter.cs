namespace Humanizer
{
    internal class GermanFormatter() :
        DefaultFormatter("de")
    {
        /// <inheritdoc />
        public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true)
        {
            return base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');
        }
    }
}
