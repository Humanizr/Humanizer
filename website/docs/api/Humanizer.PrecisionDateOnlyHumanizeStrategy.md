---
title: 'Humanizer.PrecisionDateOnlyHumanizeStrategy'
sidebar_label: 'Humanizer.PrecisionDateOnlyHumanizeStrategy'
description: 'API reference for Humanizer.PrecisionDateOnlyHumanizeStrategy.'
---
## PrecisionDateOnlyHumanizeStrategy Class

Precision\-based calculator for distance between two times

```csharp
public class PrecisionDateOnlyHumanizeStrategy : Humanizer.IDateOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrecisionDateOnlyHumanizeStrategy

Implements [IDateOnlyHumanizeStrategy](Humanizer.IDateOnlyHumanizeStrategy.md 'Humanizer\.IDateOnlyHumanizeStrategy')

### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
- *Constructors*
  - **[PrecisionDateOnlyHumanizeStrategy\(double\)](Humanizer.PrecisionDateOnlyHumanizeStrategy.md#Humanizer.PrecisionDateOnlyHumanizeStrategy.PrecisionDateOnlyHumanizeStrategy(double) 'Humanizer\.PrecisionDateOnlyHumanizeStrategy\.PrecisionDateOnlyHumanizeStrategy\(double\)')**
- *Methods*
  - **[Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.PrecisionDateOnlyHumanizeStrategy.md#Humanizer.PrecisionDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.PrecisionDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')**
### Constructors

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.PrecisionDateOnlyHumanizeStrategy(double)'></a>

#### PrecisionDateOnlyHumanizeStrategy\(double\) Constructor

Precision\-based calculator for distance between two times

```csharp
public PrecisionDateOnlyHumanizeStrategy(double precision=0.75);
```
##### Parameters

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.PrecisionDateOnlyHumanizeStrategy(double).precision'></a>

`precision` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

precision of approximation, if not provided  0\.75 will be used as a default precision\.

##### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
### Methods

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo)'></a>

#### PrecisionDateOnlyHumanizeStrategy\.Humanize\(DateOnly, DateOnly, CultureInfo\) Method

Returns localized & humanized distance of time between two dates; given a specific precision\.

```csharp
public string Humanize(System.DateOnly input, System.DateOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.PrecisionDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.IDateOnlyHumanizeStrategy.md#Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.IDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
