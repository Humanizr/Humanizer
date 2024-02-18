using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;

namespace Humanizer;

static class EnumCache<T>
    where T : struct, Enum
{
    static Type type;
    public static FrozenSet<T> Values { get; }
    public static FrozenDictionary<T, string> Humanized { get; }
    static bool isBitFieldEnum;
    public static T ZeroValue { get; }

    static EnumCache()
    {
        Values = EnumPolyfill.GetValues<T>().ToFrozenSet();
        type = typeof(T);
        ZeroValue = (T)Convert.ChangeType(Enum.ToObject(type, 0), type);
        isBitFieldEnum = type.GetCustomAttribute(typeof(FlagsAttribute)) != null;
        var dictionary = new Dictionary<T, string>();
        foreach (var value in Values)
        {
            dictionary[value] = GetDescription(value);
        }

        Humanized = dictionary.ToFrozenDictionary();
    }

    public static bool TreatAsFlags(T input)
    {
        if (!isBitFieldEnum)
        {
            return false;
        }

        return !Enum.IsDefined(type, input);
    }

    static string GetDescription(T input)
    {
        var caseName = input.ToString();
        var member = type.GetField(caseName)!;

        if (TryGetDescription(member, out var description))
        {
            return description;
        }

        return caseName.Humanize();
    }

    static bool TryGetDescription(MemberInfo member, out string description)
    {
        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
        if (displayAttribute != null)
        {
            description = displayAttribute.GetDescription() ?? displayAttribute.GetName();

            return true;
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