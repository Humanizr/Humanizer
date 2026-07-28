## DefaultDateOnlyHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultDateOnlyHumanizeStrategy : Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultDateOnlyHumanizeStrategy

Implements [IDateOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateOnlyHumanizeStrategy')
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo)'></a>

## DefaultDateOnlyHumanizeStrategy\.Humanize\(DateOnly, DateOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided dates

```csharp
public string Humanize(System.DateOnly input, System.DateOnly comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.md#Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.DateTimeHumanizeStrategy\.IDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')