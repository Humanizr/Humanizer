---
title: 'Humanizer.PrecisionDateTimeOffsetHumanizeStrategy'
sidebar_label: 'Humanizer.PrecisionDateTimeOffsetHumanizeStrategy'
description: 'API reference for Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.'
---
## PrecisionDateTimeOffsetHumanizeStrategy Class

Precision\-based calculator for distance between two times

```csharp
public class PrecisionDateTimeOffsetHumanizeStrategy : Humanizer.IDateTimeOffsetHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrecisionDateTimeOffsetHumanizeStrategy

Implements [IDateTimeOffsetHumanizeStrategy](Humanizer.IDateTimeOffsetHumanizeStrategy.md 'Humanizer\.IDateTimeOffsetHumanizeStrategy')

### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
- *Constructors*
  - **[PrecisionDateTimeOffsetHumanizeStrategy\(double\)](Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.md#Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy(double) 'Humanizer\.PrecisionDateTimeOffsetHumanizeStrategy\.PrecisionDateTimeOffsetHumanizeStrategy\(double\)')**
- *Methods*
  - **[Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\)](Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.md#Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo) 'Humanizer\.PrecisionDateTimeOffsetHumanizeStrategy\.Humanize\(System\.DateTimeOffset, System\.DateTimeOffset, System\.Globalization\.CultureInfo\)')**
### Constructors

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy(double)'></a>

#### PrecisionDateTimeOffsetHumanizeStrategy\(double\) Constructor

Precision\-based calculator for distance between two times

```csharp
public PrecisionDateTimeOffsetHumanizeStrategy(double precision=0.75);
```
##### Parameters

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy(double).precision'></a>

`precision` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

precision of approximation, if not provided  0\.75 will be used as a default precision\.

##### Remarks
Constructs a precision\-based calculator for distance of time with default precision 0\.75\.
### Methods

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo)'></a>

#### PrecisionDateTimeOffsetHumanizeStrategy\.Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\) Method

Returns localized & humanized distance of time between two dates; given a specific precision\.

```csharp
public string Humanize(System.DateTimeOffset input, System.DateTimeOffset comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

<a name='Humanizer.PrecisionDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateTimeOffset, DateTimeOffset, CultureInfo\)](Humanizer.IDateTimeOffsetHumanizeStrategy.md#Humanizer.IDateTimeOffsetHumanizeStrategy.Humanize(System.DateTimeOffset,System.DateTimeOffset,System.Globalization.CultureInfo) 'Humanizer\.IDateTimeOffsetHumanizeStrategy\.Humanize\(System\.DateTimeOffset, System\.DateTimeOffset, System\.Globalization\.CultureInfo\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
