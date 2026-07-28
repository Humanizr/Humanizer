## PrecisionDateTimeHumanizeStrategy Class

Precision\-based calculator for distance between two times

```csharp
public class PrecisionDateTimeHumanizeStrategy : Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrecisionDateTimeHumanizeStrategy

Implements [IDateTimeHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeHumanizeStrategy')
### Constructors

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy(double)'></a>

## PrecisionDateTimeHumanizeStrategy\(double\) Constructor

Constructs a precision\-based calculator for distance of time with default precision 0\.75\.

```csharp
public PrecisionDateTimeHumanizeStrategy(double precision=0.75);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy(double).precision'></a>

`precision` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

precision of approximation, if not provided  0\.75 will be used as a default precision\.
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo)'></a>

## PrecisionDateTimeHumanizeStrategy\.Humanize\(DateTime, DateTime, CultureInfo\) Method

Returns localized & humanized distance of time between two dates; given a specific precision\.

```csharp
public string Humanize(System.DateTime input, System.DateTime comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateTime, DateTime, CultureInfo\)](Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.md#Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.Humanize(System.DateTime,System.DateTime,System.Globalization.CultureInfo) 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeHumanizeStrategy\.Humanize\(System\.DateTime, System\.DateTime, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')