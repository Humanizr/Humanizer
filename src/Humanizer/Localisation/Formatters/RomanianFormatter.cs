namespace Humanizer.Localisation.Formatters
{
    using System;
    using System.Globalization;

    internal class RomanianFormatter : DefaultFormatter
    {
        private const int PrepositionIndicatingDecimals = 2;
        private const int MaxNumeralWithNoPreposition = 19;
        private const int MinNumeralWithNoPreposition = 1;
        private const string UnitPreposition = " de";
        private const string RomanianCultureCode = "ro";

        private static readonly double Divider = Math.Pow(10, PrepositionIndicatingDecimals);

        private readonly CultureInfo _romanianCulture;

        public RomanianFormatter()
            : base(RomanianCultureCode)
        {
            _romanianCulture = new CultureInfo(RomanianCultureCode);
        }

        protected override string Format(string resourceKey, int number)
        {
            var format = Resources.GetResource(GetResourceKey(resourceKey, number), _romanianCulture);
            var preposition = ShouldUsePreposition(number)
                                     ? UnitPreposition
                                     : string.Empty;

            return format.FormatWith(number, preposition);
        }

        private static bool ShouldUsePreposition(int number)
        {
            var prepositionIndicatingNumeral = Math.Abs(number % Divider);
            return prepositionIndicatingNumeral < MinNumeralWithNoPreposition
                   || prepositionIndicatingNumeral > MaxNumeralWithNoPreposition;
        }
    }
}
