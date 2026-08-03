---
title: 'Humanizer.PrecisionTimeOnlyHumanizeStrategy'
sidebar_label: 'Humanizer.PrecisionTimeOnlyHumanizeStrategy'
description: 'API reference for Humanizer.PrecisionTimeOnlyHumanizeStrategy.'
---
## PrecisionTimeOnlyHumanizeStrategy Class

Precision\-based calculator for distance between two times

```csharp
public class PrecisionTimeOnlyHumanizeStrategy : Humanizer.ITimeOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrecisionTimeOnlyHumanizeStrategy

Implements [ITimeOnlyHumanizeStrategy](Humanizer.ITimeOnlyHumanizeStrategy.md 'Humanizer\.ITimeOnlyHumanizeStrategy')

### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
- *Constructors*
  - **[PrecisionTimeOnlyHumanizeStrategy\(double\)](Humanizer.PrecisionTimeOnlyHumanizeStrategy.md#Humanizer.PrecisionTimeOnlyHumanizeStrategy.PrecisionTimeOnlyHumanizeStrategy(double) 'Humanizer\.PrecisionTimeOnlyHumanizeStrategy\.PrecisionTimeOnlyHumanizeStrategy\(double\)')**
- *Methods*
  - **[Humanize\(TimeOnly, TimeOnly, CultureInfo\)](Humanizer.PrecisionTimeOnlyHumanizeStrategy.md#Humanizer.PrecisionTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo) 'Humanizer\.PrecisionTimeOnlyHumanizeStrategy\.Humanize\(System\.TimeOnly, System\.TimeOnly, System\.Globalization\.CultureInfo\)')**
### Constructors

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.PrecisionTimeOnlyHumanizeStrategy(double)'></a>

#### PrecisionTimeOnlyHumanizeStrategy\(double\) Constructor

Precision\-based calculator for distance between two times

```csharp
public PrecisionTimeOnlyHumanizeStrategy(double precision=0.75);
```
##### Parameters

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.PrecisionTimeOnlyHumanizeStrategy(double).precision'></a>

`precision` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

precision of approximation, if not provided  0\.75 will be used as a default precision\.

##### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
### Methods

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo)'></a>

#### PrecisionTimeOnlyHumanizeStrategy\.Humanize\(TimeOnly, TimeOnly, CultureInfo\) Method

Returns localized & humanized distance of time between two dates; given a specific precision\.

```csharp
public string Humanize(System.TimeOnly input, System.TimeOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.PrecisionTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(TimeOnly, TimeOnly, CultureInfo\)](Humanizer.ITimeOnlyHumanizeStrategy.md#Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo) 'Humanizer\.ITimeOnlyHumanizeStrategy\.Humanize\(System\.TimeOnly, System\.TimeOnly, System\.Globalization\.CultureInfo\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
