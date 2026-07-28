## ITimeOnlyHumanizeStrategy Interface

Implement this interface to create a new strategy for TimeOnly\.Humanize and hook it in the Configurator\.TimeOnlyHumanizeStrategy

```csharp
public interface ITimeOnlyHumanizeStrategy
```

Derived  
↳ [DefaultTimeOnlyHumanizeStrategy](Humanizer.DefaultTimeOnlyHumanizeStrategy.md 'Humanizer\.DefaultTimeOnlyHumanizeStrategy')  
↳ [PrecisionTimeOnlyHumanizeStrategy](Humanizer.PrecisionTimeOnlyHumanizeStrategy.md 'Humanizer\.PrecisionTimeOnlyHumanizeStrategy')
### Methods

<a name='Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo)'></a>

## ITimeOnlyHumanizeStrategy\.Humanize\(TimeOnly, TimeOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided dates used for TimeOnly\.Humanize

```csharp
string Humanize(System.TimeOnly input, System.TimeOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
#### Parameters

<a name='Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')