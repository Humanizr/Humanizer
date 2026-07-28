## DefaultTimeOnlyHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultTimeOnlyHumanizeStrategy : Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultTimeOnlyHumanizeStrategy

Implements [ITimeOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.ITimeOnlyHumanizeStrategy')
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo)'></a>

## DefaultTimeOnlyHumanizeStrategy\.Humanize\(TimeOnly, TimeOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided times

```csharp
public string Humanize(System.TimeOnly input, System.TimeOnly comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(TimeOnly, TimeOnly, CultureInfo\)](Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy.md#Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo) 'Humanizer\.DateTimeHumanizeStrategy\.ITimeOnlyHumanizeStrategy\.Humanize\(System\.TimeOnly, System\.TimeOnly, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')