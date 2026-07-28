## PrecisionDateTimeOffsetHumanizeStrategy Class

Precision\-based calculator for distance between two times

```csharp
public class PrecisionDateTimeOffsetHumanizeStrategy : Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrecisionDateTimeOffsetHumanizeStrategy

Implements [IDateTimeOffsetHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeOffsetHumanizeStrategy')
### Constructors

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy(double)'></a>

## PrecisionDateTimeOffsetHumanizeStrategy\(double\) Constructor

Constructs a precision\-based calculator for distance of time with default precision 0\.75\.

```csharp
public PrecisionDateTimeOffsetHumanizeStrategy(double precision=0.75);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy(double).precision'></a>

`precision` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

precision of approximation, if not provided  0\.75 will be used as a default precision\.
### Methods

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo)'></a>

## PrecisionDateTimeOffsetHumanizeStrategy\.Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\) Method

Returns localized & humanized distance of time between two dates; given a specific precision\.

```csharp
public string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\)](Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy.md#Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo) 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeOffsetHumanizeStrategy\.Humanize\(System\.DateTimeOffset, System\.DateTimeOffset, System\.Globalization\.CultureInfo\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')