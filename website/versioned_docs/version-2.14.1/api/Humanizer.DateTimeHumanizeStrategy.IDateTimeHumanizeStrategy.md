## IDateTimeHumanizeStrategy Interface

Implement this interface to create a new strategy for DateTime\.Humanize and hook it in the Configurator\.DateTimeHumanizeStrategy

```csharp
public interface IDateTimeHumanizeStrategy
```

Derived  
↳ [DefaultDateTimeHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.DefaultDateTimeHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.DefaultDateTimeHumanizeStrategy')  
↳ [PrecisionDateTimeHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.PrecisionDateTimeHumanizeStrategy')
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo)'></a>

## IDateTimeHumanizeStrategy\.Humanize\(DateTime, DateTime, CultureInfo\) Method

Calculates the distance of time in words between two provided dates used for DateTime\.Humanize

```csharp
string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')