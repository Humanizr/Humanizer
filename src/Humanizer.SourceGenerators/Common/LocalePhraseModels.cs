using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    static readonly ImmutableDictionary<string, DateHumanizePhrase> EmptyDateHumanizeUnits =
        ImmutableDictionary<string, DateHumanizePhrase>.Empty.WithComparers(StringComparer.Ordinal);

    internal sealed class LocalePhraseCatalog(
        string localeCode,
        DateHumanizePhraseSet? dateHumanize,
        TimeSpanPhraseSet? timeSpan,
        DataUnitPhraseSet? dataUnit,
        TimeUnitPhraseSet? timeUnit)
    {
        public string LocaleCode { get; } = localeCode;
        public DateHumanizePhraseSet DateHumanize { get; } = dateHumanize ?? DateHumanizePhraseSet.Empty;
        public TimeSpanPhraseSet TimeSpan { get; } = timeSpan ?? TimeSpanPhraseSet.Empty;
        public DataUnitPhraseSet DataUnit { get; } = dataUnit ?? DataUnitPhraseSet.Empty;
        public TimeUnitPhraseSet TimeUnit { get; } = timeUnit ?? TimeUnitPhraseSet.Empty;

        public static LocalePhraseCatalog Empty(string localeCode) =>
            new(localeCode, null, null, null, null);
    }

    internal sealed class DateHumanizePhraseSet(
        string? now,
        string? never,
        ImmutableDictionary<string, DateHumanizePhrase> past,
        ImmutableDictionary<string, DateHumanizePhrase> future)
    {
        public string? Now { get; } = now;
        public string? Never { get; } = never;
        public ImmutableDictionary<string, DateHumanizePhrase> Past { get; } = past;
        public ImmutableDictionary<string, DateHumanizePhrase> Future { get; } = future;

        public static DateHumanizePhraseSet Empty { get; } = new(
            null,
            null,
            EmptyDateHumanizeUnits,
            EmptyDateHumanizeUnits);
    }

    internal sealed class TimeSpanPhraseSet(
        string? zero,
        string? age,
        ImmutableDictionary<string, TimeSpanPhrase> units)
    {
        public string? Zero { get; } = zero;
        public string? Age { get; } = age;
        public ImmutableDictionary<string, TimeSpanPhrase> Units { get; } = units;

        public static TimeSpanPhraseSet Empty { get; } = new(
            null,
            null,
            ImmutableDictionary<string, TimeSpanPhrase>.Empty.WithComparers(StringComparer.Ordinal));
    }

    internal sealed class DataUnitPhraseSet(ImmutableDictionary<string, DataUnitPhrase> units)
    {
        public ImmutableDictionary<string, DataUnitPhrase> Units { get; } = units;

        public static DataUnitPhraseSet Empty { get; } = new(
            ImmutableDictionary<string, DataUnitPhrase>.Empty.WithComparers(StringComparer.Ordinal));
    }

    internal sealed class TimeUnitPhraseSet(ImmutableDictionary<string, TimeUnitPhrase> units)
    {
        public ImmutableDictionary<string, TimeUnitPhrase> Units { get; } = units;

        public static TimeUnitPhraseSet Empty { get; } = new(
            ImmutableDictionary<string, TimeUnitPhrase>.Empty.WithComparers(StringComparer.Ordinal));
    }

    internal sealed class HeadingSet(ImmutableArray<string> full, ImmutableArray<string> shortForms)
    {
        public ImmutableArray<string> Full { get; } = full;
        public ImmutableArray<string> Short { get; } = shortForms;
    }

    internal sealed class NamedTemplatePhrase
    {
        public NamedTemplatePhrase(string name, string template)
            : this(name, template, ImmutableArray<string>.Empty)
        {
        }

        public NamedTemplatePhrase(string template, ImmutableArray<string> placeholderNames)
            : this(null, template, placeholderNames)
        {
        }

        public NamedTemplatePhrase(string? name, string template, ImmutableArray<string> placeholderNames)
        {
            Name = name;
            Template = template;
            PlaceholderNames = placeholderNames.IsDefault ? ImmutableArray<string>.Empty : placeholderNames;
        }

        public string? Name { get; }
        public string Template { get; }
        public ImmutableArray<string> PlaceholderNames { get; }
    }

    internal sealed class PhraseForms(
        string defaultForm,
        string? singular = null,
        string? dual = null,
        string? paucal = null,
        string? plural = null)
    {
        public string Default { get; } = defaultForm;
        public string? Singular { get; } = singular;
        public string? Dual { get; } = dual;
        public string? Paucal { get; } = paucal;
        public string? Plural { get; } = plural;

        public static PhraseForms FromScalar(string value) => new(value, value);

        public PhraseForms CollapseDuplicates() =>
            new(
                Default,
                CollapseDuplicate(Default, Singular),
                CollapseDuplicate(Default, Dual),
                CollapseDuplicate(Default, Paucal),
                CollapseDuplicate(Default, Plural));

        static string? CollapseDuplicate(string baseline, string? candidate) =>
            string.Equals(baseline, candidate, StringComparison.Ordinal)
                ? null
                : candidate;
    }

    internal enum CountPlacement
    {
        None,
        BeforeForm,
        AfterForm,
        BeforeText = BeforeForm,
        AfterText = AfterForm
    }

    internal sealed class CountedPhrase(
        PhraseForms? Forms = null,
        CountPlacement CountPlacement = CountPlacement.BeforeForm,
        string? BeforeCountText = null,
        string? AfterCountText = null,
        NamedTemplatePhrase? NamedTemplate = null)
    {
        public PhraseForms? Forms { get; } = Forms;
        public CountPlacement CountPlacement { get; } = CountPlacement;
        public string? BeforeCountText { get; } = BeforeCountText;
        public string? AfterCountText { get; } = AfterCountText;
        public NamedTemplatePhrase? NamedTemplate { get; } = NamedTemplate;
    }

    internal sealed class DateHumanizePhrase(string? Single = null, CountedPhrase? Multiple = null, NamedTemplatePhrase? NamedTemplate = null)
    {
        public string? Single { get; } = Single;
        public CountedPhrase? Multiple { get; } = Multiple;
        public NamedTemplatePhrase? NamedTemplate { get; } = NamedTemplate;
    }

    internal sealed class TimeSpanPhrase(
        string? Single = null,
        string? SingleWordsVariant = null,
        CountedPhrase? Multiple = null,
        CountedPhrase? MultipleWordsVariant = null,
        NamedTemplatePhrase? NamedTemplate = null)
    {
        public string? Single { get; } = Single;
        public string? SingleWordsVariant { get; } = SingleWordsVariant;
        public CountedPhrase? Multiple { get; } = Multiple;
        public CountedPhrase? MultipleWordsVariant { get; } = MultipleWordsVariant;
        public NamedTemplatePhrase? NamedTemplate { get; } = NamedTemplate;
    }

    internal sealed class DataUnitPhrase(PhraseForms? Forms = null, string? Symbol = null, NamedTemplatePhrase? NamedTemplate = null)
    {
        public PhraseForms? Forms { get; } = Forms;
        public string? Symbol { get; } = Symbol;
        public NamedTemplatePhrase? NamedTemplate { get; } = NamedTemplate;
    }

    internal sealed class TimeUnitPhrase(PhraseForms? Forms = null, string? Symbol = null, NamedTemplatePhrase? NamedTemplate = null)
    {
        public PhraseForms? Forms { get; } = Forms;
        public string? Symbol { get; } = Symbol;
        public NamedTemplatePhrase? NamedTemplate { get; } = NamedTemplate;
    }
}