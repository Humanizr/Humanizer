## DefaultDateTimeHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultDateTimeHumanizeStrategy : Humanizer.IDateTimeHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultDateTimeHumanizeStrategy

Implements [IDateTimeHumanizeStrategy](Humanizer.IDateTimeHumanizeStrategy.md 'Humanizer\.IDateTimeHumanizeStrategy')
### Methods

<a name='Humanizer.DefaultDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo)'></a>

## DefaultDateTimeHumanizeStrategy\.Humanize\(DateTime, DateTime, CultureInfo\) Method

Calculates the distance of time in words between two provided dates

```csharp
public string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo? culture);
```
#### Parameters

<a name='Humanizer.DefaultDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DefaultDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DefaultDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateTime, DateTime, CultureInfo\)](Humanizer.IDateTimeHumanizeStrategy.md#Humanizer.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo) 'Humanizer\.IDateTimeHumanizeStrategy\.Humanize\(System\.DateTime, System\.DateTime, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')