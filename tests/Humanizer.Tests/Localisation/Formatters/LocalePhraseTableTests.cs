using System.Globalization;

namespace Humanizer.Tests.Localisation.Formatters;

public class LocalePhraseTableTests
{
    static LocalePhraseTable BuildMinimalTable(
        LocalizedDatePhrase?[]? datePast = null,
        LocalizedDatePhrase?[]? dateFuture = null,
        LocalizedTimeSpanPhrase?[]? timeSpanUnits = null,
        LocalizedUnitPhrase?[]? dataUnits = null,
        LocalizedUnitPhrase?[]? timeUnits = null)
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var dataUnitCount = Enum.GetValues<DataUnit>().Length;

        return new LocalePhraseTable(
            dateNow: "now",
            dateNever: "never",
            timeSpanZero: "no time",
            timeSpanAge: "{value} old",
            datePast: datePast ?? new LocalizedDatePhrase?[unitCount],
            dateFuture: dateFuture ?? new LocalizedDatePhrase?[unitCount],
            timeSpanUnits: timeSpanUnits ?? new LocalizedTimeSpanPhrase?[unitCount],
            dataUnits: dataUnits ?? new LocalizedUnitPhrase?[dataUnitCount],
            timeUnits: timeUnits ?? new LocalizedUnitPhrase?[unitCount]);
    }

    [Fact]
    public void TryGetDatePhrase_ReturnsTrueWhenPhraseExists()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var pastPhrases = new LocalizedDatePhrase?[unitCount];
        pastPhrases[(int)TimeUnit.Day] = new LocalizedDatePhrase(Single: "yesterday");

        var table = BuildMinimalTable(datePast: pastPhrases);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var phrase));
        Assert.Equal("yesterday", phrase.Single);
    }

    [Fact]
    public void TryGetDatePhrase_ReturnsFalseWhenPhraseIsNull()
    {
        var table = BuildMinimalTable();

        Assert.False(table.TryGetDatePhrase(TimeUnit.Day, Tense.Past, out var phrase));
        Assert.Equal(default, phrase);
    }

    [Fact]
    public void TryGetDatePhrase_FutureTenseSelectsFutureArray()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var futurePhrases = new LocalizedDatePhrase?[unitCount];
        futurePhrases[(int)TimeUnit.Day] = new LocalizedDatePhrase(Single: "tomorrow");

        var table = BuildMinimalTable(dateFuture: futurePhrases);

        Assert.True(table.TryGetDatePhrase(TimeUnit.Day, Tense.Future, out var phrase));
        Assert.Equal("tomorrow", phrase.Single);
    }

    [Fact]
    public void TryGetTimeSpanPhrase_ReturnsTrueWhenPhraseExists()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var timeSpanPhrases = new LocalizedTimeSpanPhrase?[unitCount];
        timeSpanPhrases[(int)TimeUnit.Hour] = new LocalizedTimeSpanPhrase(Single: "1 hour");

        var table = BuildMinimalTable(timeSpanUnits: timeSpanPhrases);

        Assert.True(table.TryGetTimeSpanPhrase(TimeUnit.Hour, out var phrase));
        Assert.Equal("1 hour", phrase.Single);
    }

    [Fact]
    public void TryGetTimeSpanPhrase_ReturnsFalseWhenPhraseIsNull()
    {
        var table = BuildMinimalTable();

        Assert.False(table.TryGetTimeSpanPhrase(TimeUnit.Hour, out var phrase));
        Assert.Equal(default, phrase);
    }

    [Fact]
    public void TryGetDataUnitPhrase_ReturnsTrueWhenPhraseExists()
    {
        var dataUnitCount = Enum.GetValues<DataUnit>().Length;
        var dataUnits = new LocalizedUnitPhrase?[dataUnitCount];
        dataUnits[(int)DataUnit.Byte] = new LocalizedUnitPhrase(
            Forms: new LocalizedPhraseForms("bytes", Singular: "byte"),
            Symbol: "B");

        var table = BuildMinimalTable(dataUnits: dataUnits);

        Assert.True(table.TryGetDataUnitPhrase(DataUnit.Byte, out var phrase));
        Assert.Equal("bytes", phrase.Forms?.Default);
        Assert.Equal("byte", phrase.Forms?.Singular);
        Assert.Equal("B", phrase.Symbol);
    }

    [Fact]
    public void TryGetDataUnitPhrase_ReturnsFalseWhenPhraseIsNull()
    {
        var table = BuildMinimalTable();

        Assert.False(table.TryGetDataUnitPhrase(DataUnit.Byte, out var phrase));
        Assert.Equal(default, phrase);
    }

    [Fact]
    public void TryGetTimeUnitPhrase_ReturnsTrueWhenPhraseExists()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var timeUnits = new LocalizedUnitPhrase?[unitCount];
        timeUnits[(int)TimeUnit.Day] = new LocalizedUnitPhrase(Symbol: "d");

        var table = BuildMinimalTable(timeUnits: timeUnits);

        Assert.True(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var phrase));
        Assert.Equal("d", phrase.Symbol);
    }

    [Fact]
    public void TryGetTimeUnitPhrase_ReturnsFalseWhenPhraseIsNull()
    {
        var table = BuildMinimalTable();

        Assert.False(table.TryGetTimeUnitPhrase(TimeUnit.Day, out var phrase));
        Assert.Equal(default, phrase);
    }

    [Fact]
    public void GetDateHumanize_ReturnsPhraseWhenFound()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var pastPhrases = new LocalizedDatePhrase?[unitCount];
        pastPhrases[(int)TimeUnit.Day] = new LocalizedDatePhrase(Single: "yesterday");

        var table = BuildMinimalTable(datePast: pastPhrases);

        var result = table.GetDateHumanize(TimeUnit.Day, Tense.Past);
        Assert.NotNull(result);
        Assert.Equal("yesterday", result!.Value.Single);
    }

    [Fact]
    public void GetDateHumanize_ReturnsNullWhenNotFound()
    {
        var table = BuildMinimalTable();

        Assert.Null(table.GetDateHumanize(TimeUnit.Day, Tense.Past));
    }

    [Fact]
    public void GetTimeSpan_ReturnsPhraseWhenFound()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var timeSpanPhrases = new LocalizedTimeSpanPhrase?[unitCount];
        timeSpanPhrases[(int)TimeUnit.Day] = new LocalizedTimeSpanPhrase(Single: "1 day");

        var table = BuildMinimalTable(timeSpanUnits: timeSpanPhrases);

        var result = table.GetTimeSpan(TimeUnit.Day);
        Assert.NotNull(result);
        Assert.Equal("1 day", result!.Value.Single);
    }

    [Fact]
    public void GetTimeSpan_ReturnsNullWhenNotFound()
    {
        var table = BuildMinimalTable();

        Assert.Null(table.GetTimeSpan(TimeUnit.Day));
    }

    [Fact]
    public void GetDataUnit_ReturnsPhraseWhenFound()
    {
        var dataUnitCount = Enum.GetValues<DataUnit>().Length;
        var dataUnits = new LocalizedUnitPhrase?[dataUnitCount];
        dataUnits[(int)DataUnit.Megabyte] = new LocalizedUnitPhrase(Symbol: "MB");

        var table = BuildMinimalTable(dataUnits: dataUnits);

        var result = table.GetDataUnit(DataUnit.Megabyte);
        Assert.NotNull(result);
        Assert.Equal("MB", result!.Value.Symbol);
    }

    [Fact]
    public void GetDataUnit_ReturnsNullWhenNotFound()
    {
        var table = BuildMinimalTable();

        Assert.Null(table.GetDataUnit(DataUnit.Megabyte));
    }

    [Fact]
    public void GetTimeUnit_ReturnsPhraseWhenFound()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var timeUnits = new LocalizedUnitPhrase?[unitCount];
        timeUnits[(int)TimeUnit.Minute] = new LocalizedUnitPhrase(Symbol: "min");

        var table = BuildMinimalTable(timeUnits: timeUnits);

        var result = table.GetTimeUnit(TimeUnit.Minute);
        Assert.NotNull(result);
        Assert.Equal("min", result!.Value.Symbol);
    }

    [Fact]
    public void GetTimeUnit_ReturnsNullWhenNotFound()
    {
        var table = BuildMinimalTable();

        Assert.Null(table.GetTimeUnit(TimeUnit.Minute));
    }

    [Fact]
    public void GetTimeUnitSymbol_ReturnsSymbolWhenFound()
    {
        var unitCount = Enum.GetValues<TimeUnit>().Length;
        var timeUnits = new LocalizedUnitPhrase?[unitCount];
        timeUnits[(int)TimeUnit.Second] = new LocalizedUnitPhrase(Symbol: "s");

        var table = BuildMinimalTable(timeUnits: timeUnits);

        Assert.Equal("s", table.GetTimeUnitSymbol(TimeUnit.Second));
    }

    [Fact]
    public void GetTimeUnitSymbol_ReturnsNullWhenNotFound()
    {
        var table = BuildMinimalTable();

        Assert.Null(table.GetTimeUnitSymbol(TimeUnit.Second));
    }

    [Fact]
    public void DateHumanizeNow_MirrorsDateNow()
    {
        var table = BuildMinimalTable();

        Assert.Equal(table.DateNow, table.DateHumanizeNow);
        Assert.Equal("now", table.DateHumanizeNow);
    }

    [Fact]
    public void DateHumanizeNever_MirrorsDateNever()
    {
        var table = BuildMinimalTable();

        Assert.Equal(table.DateNever, table.DateHumanizeNever);
        Assert.Equal("never", table.DateHumanizeNever);
    }

    [Fact]
    public void LocalizedPhraseForms_ResolvesAllFormArms()
    {
        var forms = new LocalizedPhraseForms(
            Default: "days",
            Singular: "day",
            Dual: "dwa dni",
            Paucal: "dni",
            Plural: "dni_p");

        Assert.Equal("day", forms.Resolve(FormatterNumberForm.Singular));
        Assert.Equal("dwa dni", forms.Resolve(FormatterNumberForm.Dual));
        Assert.Equal("dni", forms.Resolve(FormatterNumberForm.Paucal));
        Assert.Equal("dni_p", forms.Resolve(FormatterNumberForm.Plural));
        Assert.Equal("days", forms.Resolve((FormatterNumberForm)999));
    }

    [Fact]
    public void LocalizedPhraseForms_FallsBackToDefaultWhenFormIsNull()
    {
        var forms = new LocalizedPhraseForms(Default: "days");

        Assert.Equal("days", forms.Resolve(FormatterNumberForm.Singular));
        Assert.Equal("days", forms.Resolve(FormatterNumberForm.Dual));
        Assert.Equal("days", forms.Resolve(FormatterNumberForm.Paucal));
        Assert.Equal("days", forms.Resolve(FormatterNumberForm.Plural));
    }

    [Fact]
    public void Catalog_Resolve_FallsBackToEnglishForInvariantCulture()
    {
        var table = LocalePhraseTableCatalog.Resolve(CultureInfo.InvariantCulture);

        Assert.NotNull(table);
        Assert.Equal("now", table!.DateNow);
    }

    [Fact]
    public void Catalog_Resolve_WalksParentCultureChain()
    {
        // en-AU -> en -> found
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("en-AU"));

        Assert.NotNull(table);
        Assert.Equal("now", table!.DateNow);
    }

    [Fact]
    public void Catalog_Resolve_FallsBackToEnglishForUnknownCulture()
    {
        // Esperanto (eo) has no phrase table; should fall through to en
        var table = LocalePhraseTableCatalog.Resolve(new CultureInfo("eo"));

        Assert.NotNull(table);
        Assert.Equal("now", table!.DateNow);
    }
}