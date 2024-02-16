namespace Humanizer
{
    internal class IcelandicFormatter() :
        DefaultFormatter(LocaleCode)
    {
        private const string LocaleCode = "is";
        private readonly CultureInfo _localCulture = new(LocaleCode);

        public override string DataUnitHumanize(DataUnit dataUnit, double count, bool toSymbol = true) =>
            base.DataUnitHumanize(dataUnit, count, toSymbol)?.TrimEnd('s');

        protected override string Format(string resourceKey, int number, bool toWords = false)
        {
            var resourceString = Resources.GetResource(GetResourceKey(resourceKey, number), _localCulture);

            if (string.IsNullOrEmpty(resourceString))
            {
                throw new ArgumentException($@"The resource object with key '{resourceKey}' was not found", nameof(resourceKey));
            }
            var words = resourceString.Split(' ');

            var unitGender = words.Last() switch
            {
                var x when x.StartsWith("mán") => GrammaticalGender.Masculine,
                var x when x.StartsWith("dag") => GrammaticalGender.Masculine,
                var x when x.StartsWith("ár") => GrammaticalGender.Neuter,
                _ => GrammaticalGender.Feminine
            };

            return toWords ?
                resourceString.FormatWith(number.ToWords(unitGender, _localCulture)) :
                resourceString.FormatWith(number);
        }
    }
}
