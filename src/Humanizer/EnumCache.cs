using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;

namespace Humanizer;

static class EnumCache<T>
    where T : struct, Enum
{
    static (T Zero, FrozenDictionary<T, string> Humanized, Dictionary<string, T> Dehumanized, FrozenSet<T> Values, bool IsBitFieldEnum) info;

    static EnumCache()
    {
        var values = EnumPolyfill.GetValues<T>().ToFrozenSet();
        var type = typeof(T);
        var zero = (T)Convert.ChangeType(Enum.ToObject(type, 0), type);
        var humanized = new Dictionary<T, string>();
        var dehumanized = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        foreach (var value in values)
        {
            var description = GetDescription(value);
            humanized[value] = description;
            dehumanized[description] = value;
        }

        var isBitFieldEnum = type.GetCustomAttribute(typeof(FlagsAttribute)) != null;
        info = (
            zero,
            humanized.ToFrozenDictionary(),
            dehumanized,
            values,
            isBitFieldEnum);
    }

    public static (T Zero, FrozenDictionary<T, string> Humanized, FrozenSet<T> Values) GetInfo() =>
        (info.Zero, info.Humanized, info.Values);

    public static Dictionary<string, T> GetDehumanized() =>
        info.Dehumanized;

    public static bool TreatAsFlags(T input)
    {
        var type = typeof(T);
        if (!info.IsBitFieldEnum)
        {
            return false;
        }

        return !Enum.IsDefined(type, input);
    }

    static string GetDescription(T input)
    {
        var type = typeof(T);
        var caseName = input.ToString();
        var member = type.GetField(caseName)!;

        if (TryGetDescription(member, out var description))
        {
            return description;
        }

        return caseName.Humanize();
    }

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
            var attrType = attr.GetType();
            foreach (var property in attrType.GetRuntimeProperties())
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