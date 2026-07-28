using System.ComponentModel.DataAnnotations;

namespace Humanizer;

static class EnumCache<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>
    where T : struct, Enum
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
    static readonly Type TypeOfT = typeof(T);
    static readonly (T Zero, FrozenDictionary<T, string> Humanized, FrozenDictionary<string, T> Dehumanized, FrozenSet<T> Values, bool IsBitFieldEnum) Info = CreateInfo();

    private static (T Zero, FrozenDictionary<T, string> Humanized, FrozenDictionary<string, T> Dehumanized, FrozenSet<T> Values, bool IsBitFieldEnum) CreateInfo()
    {
        var valuesArray = Enum.GetValues<T>();
        var zero = (T)Convert.ChangeType(Enum.ToObject(TypeOfT, 0), TypeOfT);
        var count = valuesArray.Length;
        var humanized = new Dictionary<T, string>(count);
        var dehumanized = new Dictionary<string, T>(count, StringComparer.OrdinalIgnoreCase);
        foreach (var value in valuesArray)
        {
            var description = GetDescription(value);
            humanized[value] = description;
            dehumanized[description] = value;
        }

        var isBitFieldEnum = TypeOfT.GetCustomAttribute<FlagsAttribute>() != null;
        return (
            zero,
            humanized.ToFrozenDictionary(),
            dehumanized.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase),
            valuesArray.ToFrozenSet(),
            isBitFieldEnum);
    }

    public static (T Zero, FrozenDictionary<T, string> Humanized, FrozenSet<T> Values) GetInfo() =>
        (Info.Zero, Info.Humanized, Info.Values);

    public static FrozenDictionary<string, T> GetDehumanized() =>
        Info.Dehumanized;

    public static bool TreatAsFlags(T input)
    {
        if (!Info.IsBitFieldEnum)
        {
            return false;
        }

        return !Enum.IsDefined(TypeOfT, input);
    }

    static string GetDescription(T input)
    {
#if NET5_0_OR_GREATER
        var caseName = Enum.GetName(input)!;
#else
        var caseName = Enum.GetName(TypeOfT, input)!;
#endif
        var member = TypeOfT.GetField(caseName)!;

        if (TryGetDescription(member, out var description))
        {
            return description;
        }

        return caseName.Humanize();
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