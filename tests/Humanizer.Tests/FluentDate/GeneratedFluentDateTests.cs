using System.Diagnostics.CodeAnalysis;
using System.Reflection;

[UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Tests reflect over known public FluentDate API types in the referenced Humanizer assembly.")]
[UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Tests reflect over known public FluentDate API types in the referenced Humanizer assembly.")]
public class GeneratedFluentDateTests
{
    [Fact]
    public void InRelativeDatePropertiesReturnExpectedUtcOffsets() =>
        AssertDateTimeRelativeProperties(typeof(In));

    [Fact]
    public void InRelativeDateMethodsReturnExpectedOffsetsFromProvidedDate() =>
        AssertDateTimeRelativeMethods(typeof(In));

    [Fact]
    public void OnDayPropertiesCoverAllGeneratedDayAccessors() =>
        AssertMonthDayProperties(typeof(On), typeof(DateTime));

    [Fact]
    public void OnTheMethodsCoverAllGeneratedMonthFactories() =>
        AssertMonthTheMethods(typeof(On), typeof(DateTime));

#if NET6_0_OR_GREATER
    [Fact]
    public void InDateMonthPropertiesReturnExpectedDates() =>
        AssertMonthProperties(typeof(InDate), typeof(DateOnly));

    [Fact]
    public void InDateMonthMethodsReturnExpectedDatesForProvidedYear() =>
        AssertMonthOfMethods(typeof(InDate), typeof(DateOnly));

    [Fact]
    public void InDateRelativeDatePropertiesReturnExpectedUtcOffsets() =>
        AssertDateOnlyRelativeProperties(typeof(InDate));

    [Fact]
    public void InDateRelativeDateOnlyMethodsReturnExpectedOffsetsFromProvidedDate() =>
        AssertDateOnlyRelativeMethods(typeof(InDate), typeof(DateOnly));

    [Fact]
    public void InDateRelativeDateTimeMethodsReturnExpectedOffsetsFromProvidedDateTime() =>
        AssertDateOnlyRelativeMethods(typeof(InDate), typeof(DateTime));

    [Fact]
    public void OnDateDayPropertiesCoverAllGeneratedDayAccessors() =>
        AssertMonthDayProperties(typeof(OnDate), typeof(DateOnly));

    [Fact]
    public void OnDateTheMethodsCoverAllGeneratedMonthFactories() =>
        AssertMonthTheMethods(typeof(OnDate), typeof(DateOnly));
#endif

    [Fact]
    public void InMonthPropertiesReturnExpectedDates() =>
        AssertMonthProperties(typeof(In), typeof(DateTime));

    [Fact]
    public void InMonthMethodsReturnExpectedDatesForProvidedYear() =>
        AssertMonthOfMethods(typeof(In), typeof(DateTime));

    static void AssertDateTimeRelativeProperties(Type rootType)
    {
        foreach (var nestedType in GetRelativeNestedTypes(rootType))
        {
            var amount = AmountFor(nestedType);

            foreach (var property in nestedType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                var unit = RelativeDateUnitFor(property.Name);
                var before = DateTime.UtcNow;
                var actual = Assert.IsType<DateTime>(property.GetValue(null));
                var after = DateTime.UtcNow;

                Assert.InRange(actual, Add(before, amount, unit), Add(after, amount, unit));
            }
        }
    }

    static void AssertDateTimeRelativeMethods(Type rootType)
    {
        var date = new DateTime(2024, 2, 29, 10, 20, 30, DateTimeKind.Utc);

        foreach (var nestedType in GetRelativeNestedTypes(rootType))
        {
            var amount = AmountFor(nestedType);

            foreach (var method in GetFromMethods(nestedType, typeof(DateTime)))
            {
                var unit = RelativeDateUnitFor(method.Name[..^"From".Length]);
                var actual = Assert.IsType<DateTime>(method.Invoke(null, [date]));

                Assert.Equal(Add(date, amount, unit), actual);
            }
        }
    }

#if NET6_0_OR_GREATER
    static void AssertDateOnlyRelativeProperties(Type rootType)
    {
        foreach (var nestedType in GetRelativeNestedTypes(rootType))
        {
            var amount = AmountFor(nestedType);

            foreach (var property in nestedType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                var unit = RelativeDateUnitFor(property.Name);
                var before = DateTime.UtcNow;
                var actual = Assert.IsType<DateOnly>(property.GetValue(null));
                var after = DateTime.UtcNow;

                Assert.InRange(actual, DateOnly.FromDateTime(Add(before, amount, unit)), DateOnly.FromDateTime(Add(after, amount, unit)));
            }
        }
    }

    static void AssertDateOnlyRelativeMethods(Type rootType, Type parameterType)
    {
        var dateTime = new DateTime(2024, 2, 29, 10, 20, 30, DateTimeKind.Utc);
        var dateOnly = DateOnly.FromDateTime(dateTime);
        var argument = parameterType == typeof(DateOnly) ? dateOnly : (object)dateTime;

        foreach (var nestedType in GetRelativeNestedTypes(rootType))
        {
            var amount = AmountFor(nestedType);

            foreach (var method in GetFromMethods(nestedType, parameterType))
            {
                var unit = RelativeDateUnitFor(method.Name[..^"From".Length]);
                var actual = Assert.IsType<DateOnly>(method.Invoke(null, [argument]));
                var expected = parameterType == typeof(DateOnly)
                    ? Add(dateOnly, amount, unit)
                    : DateOnly.FromDateTime(Add(dateTime, amount, unit));

                Assert.Equal(expected, actual);
            }
        }
    }
#endif

    static void AssertMonthProperties(Type rootType, Type expectedType)
    {
        foreach (var (monthName, month) in Months)
        {
            var property = rootType.GetProperty(monthName, BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull(property);

            var value = property.GetValue(null);
            AssertMonthValue(expectedType, value, GetYear(value), month, 1);
        }
    }

    static void AssertMonthOfMethods(Type rootType, Type expectedType)
    {
        const int year = 2032;

        foreach (var (monthName, month) in Months)
        {
            var method = rootType.GetMethod(
                $"{monthName}Of",
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(int)],
                null);
            Assert.NotNull(method);

            AssertMonthValue(expectedType, method.Invoke(null, [year]), year, month, 1);
        }
    }

    static void AssertMonthDayProperties(Type rootType, Type expectedType)
    {
        foreach (var monthType in GetMonthNestedTypes(rootType))
        {
            var month = MonthNumberFor(monthType.Name);

            foreach (var property in monthType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                if (!TryParseDayProperty(property.Name, out var day))
                    continue;

                try
                {
                    var value = property.GetValue(null);
                    AssertMonthValue(expectedType, value, GetYear(value), month, day);
                }
                catch (TargetInvocationException exception)
                {
                    Assert.IsType<ArgumentOutOfRangeException>(exception.InnerException);
                }
            }
        }
    }

    static void AssertMonthTheMethods(Type rootType, Type expectedType)
    {
        foreach (var monthType in GetMonthNestedTypes(rootType))
        {
            var month = MonthNumberFor(monthType.Name);
            var method = monthType.GetMethod(
                "The",
                BindingFlags.Public | BindingFlags.Static,
                null,
                [typeof(int)],
                null);
            Assert.NotNull(method);

            var value = method.Invoke(null, [1]);
            AssertMonthValue(expectedType, value, GetYear(value), month, 1);
        }
    }

    static IEnumerable<Type> GetRelativeNestedTypes(Type rootType) =>
        rootType.GetNestedTypes(BindingFlags.Public)
            .Where(type => RelativeAmounts.ContainsKey(type.Name))
            .OrderBy(AmountFor);

    static IEnumerable<Type> GetMonthNestedTypes(Type rootType) =>
        rootType.GetNestedTypes(BindingFlags.Public)
            .Where(type => MonthNumbers.ContainsKey(type.Name))
            .OrderBy(type => MonthNumberFor(type.Name));

    static IEnumerable<MethodInfo> GetFromMethods(Type type, Type parameterType) =>
        type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.Name.EndsWith("From", StringComparison.Ordinal))
            .Where(method =>
            {
                var parameters = method.GetParameters();
                return parameters.Length == 1 && parameters[0].ParameterType == parameterType;
            })
            .OrderBy(method => method.Name);

    static void AssertMonthValue(Type expectedType, object? actual, int year, int month, int day)
    {
        if (expectedType == typeof(DateTime))
        {
            Assert.Equal(new DateTime(year, month, day), Assert.IsType<DateTime>(actual));
            return;
        }

#if NET6_0_OR_GREATER
        Assert.Equal(new DateOnly(year, month, day), Assert.IsType<DateOnly>(actual));
#else
        throw new InvalidOperationException("DateOnly assertions require .NET 6 or later.");
#endif
    }

    static int GetYear(object? value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.Year;
        }

#if NET6_0_OR_GREATER
        if (value is DateOnly dateOnly)
        {
            return dateOnly.Year;
        }
#endif

        throw new InvalidOperationException($"Unsupported date value '{value?.GetType().FullName ?? "<null>"}'.");
    }

    static DateTime Add(DateTime date, int amount, RelativeDateUnit unit) =>
        unit switch
        {
            RelativeDateUnit.Second => date.AddSeconds(amount),
            RelativeDateUnit.Minute => date.AddMinutes(amount),
            RelativeDateUnit.Hour => date.AddHours(amount),
            RelativeDateUnit.Day => date.AddDays(amount),
            RelativeDateUnit.Week => date.AddDays(amount * 7),
            RelativeDateUnit.Month => date.AddMonths(amount),
            RelativeDateUnit.Year => date.AddYears(amount),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };

#if NET6_0_OR_GREATER
    static DateOnly Add(DateOnly date, int amount, RelativeDateUnit unit) =>
        unit switch
        {
            RelativeDateUnit.Day => date.AddDays(amount),
            RelativeDateUnit.Week => date.AddDays(amount * 7),
            RelativeDateUnit.Month => date.AddMonths(amount),
            RelativeDateUnit.Year => date.AddYears(amount),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
#endif

    static int AmountFor(Type type) => RelativeAmounts[type.Name];

    static int MonthNumberFor(string monthName) => MonthNumbers[monthName];

    static RelativeDateUnit RelativeDateUnitFor(string memberName)
    {
#if NET5_0_OR_GREATER
        var singular = memberName.EndsWith('s') ? memberName[..^1] : memberName;

        return Enum.Parse<RelativeDateUnit>(singular);
#else
        var singular = memberName.EndsWith("s", StringComparison.Ordinal) ? memberName[..^1] : memberName;

        return (RelativeDateUnit)Enum.Parse(typeof(RelativeDateUnit), singular);
#endif
    }

    static bool TryParseDayProperty(string propertyName, out int day)
    {
        day = 0;

        if (!propertyName.StartsWith("The", StringComparison.Ordinal))
            return false;

        var suffixStart = propertyName.IndexOfAny(['s', 'n', 'r', 't'], 3);
        return suffixStart > 3 && int.TryParse(propertyName[3..suffixStart], NumberStyles.None, CultureInfo.InvariantCulture, out day);
    }

    enum RelativeDateUnit
    {
        Second,
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year
    }

    static readonly Dictionary<string, int> RelativeAmounts = new()
    {
        ["One"] = 1,
        ["Two"] = 2,
        ["Three"] = 3,
        ["Four"] = 4,
        ["Five"] = 5,
        ["Six"] = 6,
        ["Seven"] = 7,
        ["Eight"] = 8,
        ["Nine"] = 9,
        ["Ten"] = 10
    };

    static readonly (string Name, int Number)[] Months =
    [
        ("January", 1),
        ("February", 2),
        ("March", 3),
        ("April", 4),
        ("May", 5),
        ("June", 6),
        ("July", 7),
        ("August", 8),
        ("September", 9),
        ("October", 10),
        ("November", 11),
        ("December", 12)
    ];

    static readonly Dictionary<string, int> MonthNumbers = Months.ToDictionary(static month => month.Name, static month => month.Number);
}