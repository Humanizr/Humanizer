using System.ComponentModel.DataAnnotations;

namespace Humanizer;

static class EnumCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>
    where T : struct, Enum
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
    static readonly Type TypeOfT = typeof(T);
    static class DefaultInfo
    {
        public static readonly (
            T Zero,
            FrozenDictionary<T, (string Text, bool IsMetadata)> Humanized,
            FrozenDictionary<T, (string EnumName, DisplayAttribute? Display)> Sources,
            FrozenDictionary<string, T> Dehumanized,
            FrozenSet<T> Values,
            bool IsBitFieldEnum) Value = CreateInfo();
    }

    static class ExplicitInfo
    {
        public static readonly (
            T Zero,
            FrozenDictionary<T, (string EnumName, DisplayAttribute? Display)> Sources,
            FrozenSet<T> Values,
            bool IsBitFieldEnum) Value = CreateExplicitInfo();
    }

    private static (
        T Zero,
        FrozenDictionary<T, (string Text, bool IsMetadata)> Humanized,
        FrozenDictionary<T, (string EnumName, DisplayAttribute? Display)> Sources,
        FrozenDictionary<string, T> Dehumanized,
        FrozenSet<T> Values,
        bool IsBitFieldEnum) CreateInfo()
    {
        var valuesArray = Enum.GetValues<T>();
        var namesArray = Enum.GetNames(TypeOfT);
        var zero = (T)Convert.ChangeType(Enum.ToObject(TypeOfT, 0), TypeOfT);
        var count = valuesArray.Length;
        var humanized = new Dictionary<T, (string Text, bool IsMetadata)>(count);
        var sources = new Dictionary<T, (string EnumName, DisplayAttribute? Display)>(count);
        var dehumanized = new Dictionary<string, T>(count * 6, StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < namesArray.Length; i++)
        {
            AddAliases(dehumanized, namesArray[i], valuesArray[i]);
        }

        foreach (var value in valuesArray)
        {
            var description = GetDescription(value);
            humanized[value] = description;
            sources[value] = GetSources(value);
            dehumanized[description.Text] = value;
        }

        var isBitFieldEnum = TypeOfT.GetCustomAttribute<FlagsAttribute>() != null;
        return (
            zero,
            humanized.ToFrozenDictionary(),
            sources.ToFrozenDictionary(),
            dehumanized.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase),
            valuesArray.ToFrozenSet(),
            isBitFieldEnum);
    }

    static (
        T Zero,
        FrozenDictionary<T, (string EnumName, DisplayAttribute? Display)> Sources,
        FrozenSet<T> Values,
        bool IsBitFieldEnum) CreateExplicitInfo()
    {
        var valuesArray = Enum.GetValues<T>();
        var zero = (T)Convert.ChangeType(Enum.ToObject(TypeOfT, 0), TypeOfT);
        var sources = new Dictionary<T, (string EnumName, DisplayAttribute? Display)>(valuesArray.Length);
        foreach (var value in valuesArray)
        {
            sources[value] = GetSources(value);
        }

        var isBitFieldEnum = TypeOfT.GetCustomAttribute<FlagsAttribute>() != null;
        return (zero, sources.ToFrozenDictionary(), valuesArray.ToFrozenSet(), isBitFieldEnum);
    }

    static void AddAliases(Dictionary<string, T> dehumanized, string caseName, T value)
    {
        var member = TypeOfT.GetField(caseName)!;
        dehumanized[caseName] = value;
        dehumanized[caseName.Humanize()] = value;

        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            AddAlias(displayAttribute.GetName());
            AddAlias(displayAttribute.GetDescription());
            AddAlias(displayAttribute.GetShortName());
        }

        void AddAlias(string? alias)
        {
            if (alias != null)
            {
                dehumanized[alias] = value;
            }
        }
    }

    public static (T Zero, FrozenSet<T> Values) GetInfo(EnumHumanizeSource source) =>
        source == EnumHumanizeSource.Default
            ? (DefaultInfo.Value.Zero, DefaultInfo.Value.Values)
            : (ExplicitInfo.Value.Zero, ExplicitInfo.Value.Values);

    public static (string Text, bool IsMetadata) GetHumanized(T input, EnumHumanizeSource source)
    {
        if (source == EnumHumanizeSource.Default)
        {
            return DefaultInfo.Value.Humanized[input];
        }

        var (enumName, display) = ExplicitInfo.Value.Sources[input];
        var metadata = source switch
        {
            EnumHumanizeSource.EnumName => null,
            EnumHumanizeSource.DisplayName => display?.GetName(),
            EnumHumanizeSource.DisplayDescription => display?.GetDescription(),
            EnumHumanizeSource.DisplayShortName => display?.GetShortName(),
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        };

        return metadata is null ? (enumName, false) : (metadata, true);
    }

    public static FrozenDictionary<string, T> GetDehumanized() =>
        DefaultInfo.Value.Dehumanized;

    public static bool TreatAsFlags(T input, EnumHumanizeSource source)
    {
        var isBitFieldEnum = source == EnumHumanizeSource.Default
            ? DefaultInfo.Value.IsBitFieldEnum
            : ExplicitInfo.Value.IsBitFieldEnum;
        if (!isBitFieldEnum)
        {
            return false;
        }

        return !Enum.IsDefined(TypeOfT, input);
    }

    static (string Text, bool IsMetadata) GetDescription(T input)
    {
#if NET5_0_OR_GREATER
        var caseName = Enum.GetName(input)!;
#else
        var caseName = Enum.GetName(TypeOfT, input)!;
#endif
        var member = TypeOfT.GetField(caseName)!;

        if (TryGetDescription(member, out var description))
        {
            return (description, true);
        }

        return (caseName.Humanize(), false);
    }

    static (string EnumName, DisplayAttribute? Display) GetSources(T input)
    {
#if NET5_0_OR_GREATER
        var caseName = Enum.GetName(input)!;
#else
        var caseName = Enum.GetName(TypeOfT, input)!;
#endif
        var member = TypeOfT.GetField(caseName)!;
        return (caseName.Humanize(), member.GetCustomAttribute<DisplayAttribute>());
    }

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "Reflection over attribute properties is intentional and documented.")]
    static bool TryGetDescription(MemberInfo member, [NotNullWhen(true)] out string? description)
    {
        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            description = displayAttribute.GetDescription() ?? displayAttribute.GetName();

            return description != null;
        }

        foreach (var attr in member.GetCustomAttributes())
        {
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            var attrType = attr.GetType();
            foreach (var property in attrType.GetRuntimeProperties())
#pragma warning restore IL2072
            {
                if (property.PropertyType == typeof(string) &&
                    Configurator.EnumDescriptionPropertyLocator(property))
                {
                    description = (string)property.GetValue(attr, null)!;
                    return true;
                }
            }
        }

        description = null;
        return false;
    }
}