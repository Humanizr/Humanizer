## IDateTimeOffsetHumanizeStrategy Interface

Implement this interface to create a new strategy for DateTime\.Humanize and hook it in the Configurator\.DateTimeOffsetHumanizeStrategy

```csharp
public interface IDateTimeOffsetHumanizeStrategy
```

Derived  
↳ [DefaultDateTimeOffsetHumanizeStrategy](Humanizer.DefaultDateTimeOffsetHumanizeStrategy.md 'Humanizer\.DefaultDateTimeOffsetHumanizeStrategy')  
↳ [PrecisionDateTimeOffsetHumanizeStrategy](Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.md 'Humanizer\.PrecisionDateTimeOffsetHumanizeStrategy')
### Methods

<a name='Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo)'></a>

## IDateTimeOffsetHumanizeStrategy\.Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\) Method

Calculates the distance of time in words between two provided dates used for DateTimeOffset\.Humanize

```csharp
string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo? culture);
```
#### Parameters

<a name='Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')