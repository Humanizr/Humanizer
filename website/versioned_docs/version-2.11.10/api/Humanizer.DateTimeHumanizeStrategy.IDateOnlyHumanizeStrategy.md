## IDateOnlyHumanizeStrategy Interface

Implement this interface to create a new strategy for DateOnly\.Humanize and hook it in the Configurator\.DateOnlyHumanizeStrategy

```csharp
public interface IDateOnlyHumanizeStrategy
```

Derived  
↳ [DefaultDateOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.DefaultDateOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.DefaultDateOnlyHumanizeStrategy')  
↳ [PrecisionDateOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.PrecisionDateOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.PrecisionDateOnlyHumanizeStrategy')
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo)'></a>

## IDateOnlyHumanizeStrategy\.Humanize\(DateOnly, DateOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided dates used for DateOnly\.Humanize

```csharp
string Humanize(System.DateOnly input, System.DateOnly comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')