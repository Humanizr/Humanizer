## DefaultDateTimeOffsetHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultDateTimeOffsetHumanizeStrategy : Humanizer.IDateTimeOffsetHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultDateTimeOffsetHumanizeStrategy

Implements [IDateTimeOffsetHumanizeStrategy](Humanizer.IDateTimeOffsetHumanizeStrategy.md 'Humanizer\.IDateTimeOffsetHumanizeStrategy')
### Methods

<a name='Humanizer.DefaultDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo)'></a>

## DefaultDateTimeOffsetHumanizeStrategy\.Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\) Method

Calculates the distance of time in words between two provided dates

```csharp
public string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo? culture);
```
#### Parameters

<a name='Humanizer.DefaultDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.DefaultDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.DefaultDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\)](Humanizer.IDateTimeOffsetHumanizeStrategy.md#Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo) 'Humanizer\.IDateTimeOffsetHumanizeStrategy\.Humanize\(System\.DateTimeOffset, System\.DateTimeOffset, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')