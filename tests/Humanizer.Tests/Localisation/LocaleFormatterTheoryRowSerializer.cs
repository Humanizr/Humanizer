using System.Linq;
using System.Text;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(
    typeof(Humanizer.Tests.Localisation.LocaleFormatterTheoryRowSerializer),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.DateDayPluralExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.HeadingExpectationRow),
    typeof(Humanizer.Tests.Localisation.LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow))]

namespace Humanizer.Tests.Localisation;

public sealed class LocaleFormatterTheoryRowSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        var values = Decode(serializedValue);

        if (type == typeof(LocaleFormatterExactTheoryData.DateDayPluralExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.DateDayPluralExpectationRow(
                values[0], values[1], values[2], values[3], values[4], values[5],
                values[6], values[7], values[8], values[9], values[10], values[11]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow(values[0], values[1], values[2]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow(values[0], values[1], values[2], values[3]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow(values[0], values[1], values[2], values[3], values[4]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow(values[0], values[1], values[2]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.HeadingExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.HeadingExpectationRow(
                values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7],
                values[8], values[9], values[10], values[11], values[12], values[13], values[14], values[15],
                values[16], values[17], values[18], values[19], values[20], values[21], values[22], values[23],
                values[24], values[25], values[26], values[27], values[28], values[29], values[30], values[31]);
        }

        if (type == typeof(LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow))
        {
            return new LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow(values[0], values[1], values[2], values[3]);
        }

        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }

    public bool IsSerializable(Type type, object? value, out string failureReason)
    {
        failureReason = string.Empty;

        if (value is null)
        {
            failureReason = "Serializer does not support null values.";
            return false;
        }

        return type == typeof(LocaleFormatterExactTheoryData.DateDayPluralExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.HeadingExpectationRow) ||
            type == typeof(LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow);
    }

    public string Serialize(object value) =>
        value switch
        {
            LocaleFormatterExactTheoryData.DateDayPluralExpectationRow row => Encode(
                row.PastTwoDays, row.PastThreeDays, row.PastFourDays, row.PastFiveDays, row.PastElevenDays, row.PastTwentyOneDays,
                row.FutureTwoDays, row.FutureThreeDays, row.FutureFourDays, row.FutureFiveDays, row.FutureElevenDays, row.FutureTwentyOneDays),
            LocaleFormatterExactTheoryData.MultiPartTimeSpanExpectationRow row => Encode(row.LargestSpan, row.DenseSpan, row.DenseSpanWithEmptyUnits),
            LocaleFormatterExactTheoryData.TimeUnitSymbolExpectationRow row => Encode(row.Millisecond, row.Second, row.Minute, row.Hour, row.Day, row.Week, row.Month, row.Year),
            LocaleFormatterExactTheoryData.ByteSizeSymbolExpectationRow row => Encode(row.OneBit, row.TwoBytes, row.TwoThousandBytesAsKilobytes, row.TwoMegabytesAsKilobytes),
            LocaleFormatterExactTheoryData.ByteSizeFullWordExpectationRow row => Encode(row.OneBit, row.OneByte, row.TwoBytes, row.TwoKilobytes, row.TwoMegabytes),
            LocaleFormatterExactTheoryData.CollectionHumanizeExpectationRow row => Encode(row.Pair, row.Triple, row.Quadruple),
            LocaleFormatterExactTheoryData.HeadingExpectationRow row => Encode(
                row.AbbreviatedNorth, row.AbbreviatedNorthNortheast, row.AbbreviatedNortheast, row.AbbreviatedEastNortheast,
                row.AbbreviatedEast, row.AbbreviatedEastSoutheast, row.AbbreviatedSoutheast, row.AbbreviatedSouthSoutheast,
                row.AbbreviatedSouth, row.AbbreviatedSouthSouthwest, row.AbbreviatedSouthwest, row.AbbreviatedWestSouthwest,
                row.AbbreviatedWest, row.AbbreviatedWestNorthwest, row.AbbreviatedNorthwest, row.AbbreviatedNorthNorthwest,
                row.FullNorth, row.FullNorthNortheast, row.FullNortheast, row.FullEastNortheast,
                row.FullEast, row.FullEastSoutheast, row.FullSoutheast, row.FullSouthSoutheast,
                row.FullSouth, row.FullSouthSouthwest, row.FullSouthwest, row.FullWestSouthwest,
                row.FullWest, row.FullWestNorthwest, row.FullNorthwest, row.FullNorthNorthwest),
            LocaleFormatterExactTheoryData.CardinalHeadingExpectationRow row => Encode(row.North, row.East, row.South, row.West),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value.GetType(), null)
        };

    static string[] Decode(string serializedValue) =>
        serializedValue
            .Split(';')
            .Select(static part => Encoding.UTF8.GetString(Convert.FromBase64String(part)))
            .ToArray();

    static string Encode(params string[] values) =>
        string.Join(';', values.Select(static value => Convert.ToBase64String(Encoding.UTF8.GetBytes(value))));
}
