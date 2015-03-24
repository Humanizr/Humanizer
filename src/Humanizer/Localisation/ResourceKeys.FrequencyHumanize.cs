namespace Humanizer.Localisation
{
    /// <summary>
    /// Encapsulates the logic required to get the resource keys for DateTime.Humanize
    /// </summary>
    public partial class ResourceKeys
    {
        public static class FrequencyHumanize
        {
            private const string Single = "Single";
            private const string Multiple = "Multiple";
            private const string Standard = "Standard";
            private const string Approximate = "Approximate";
            private const string FrequencyFormat = "FrequencyHumanize_{0}{1}{2}";
            private const string Half = "FrequencyHumanize_Half";
            private const string From = "FrequencyHumanize_From";
            private const string Till = "FrequencyHumanize_Till";
            private const string On = "FrequencyHumanize_On";

            public static string GetResourceKey(Frequencies frequency, int count = 1, bool isStandard = true)
            {
                if (count < 1) return Half;
                string number = count == 1 ? Single : Multiple;
                string precision = isStandard ? Standard : Approximate;
                if (frequency == Frequencies.Never || frequency == Frequencies.OnceOff)
                {
                    return FrequencyFormat.FormatWith(frequency.GetStringValue(), string.Empty, string.Empty);
                }
                return FrequencyFormat.FormatWith(number, frequency.GetStringValue(), precision);
            }

            public static string GetResourceKeyFrom()
            {
                return From;
            }

            public static string GetResourceKeyTill()
            {
                return Till;
            }

            public static string GetResourceKeyOn()
            {
                return On;
            }
        }
    }
}
