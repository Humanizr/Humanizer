---
title: 'Humanizer.DefaultTimeOnlyHumanizeStrategy'
sidebar_label: 'Humanizer.DefaultTimeOnlyHumanizeStrategy'
description: 'API reference for Humanizer.DefaultTimeOnlyHumanizeStrategy.'
---
## DefaultTimeOnlyHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultTimeOnlyHumanizeStrategy : Humanizer.ITimeOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultTimeOnlyHumanizeStrategy

Implements [ITimeOnlyHumanizeStrategy](Humanizer.ITimeOnlyHumanizeStrategy.md 'Humanizer\.ITimeOnlyHumanizeStrategy')
- *Constructors*
  - **[DefaultTimeOnlyHumanizeStrategy\(\)](Humanizer.DefaultTimeOnlyHumanizeStrategy.md#Humanizer.DefaultTimeOnlyHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy())**
- *Methods*
  - **[Humanize\(TimeOnly, TimeOnly, CultureInfo\)](Humanizer.DefaultTimeOnlyHumanizeStrategy.md#Humanizer.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo) 'Humanizer\.DefaultTimeOnlyHumanizeStrategy\.Humanize\(System\.TimeOnly, System\.TimeOnly, System\.Globalization\.CultureInfo\)')**
### Constructors

<a name='Humanizer.DefaultTimeOnlyHumanizeStrategy.DefaultTimeOnlyHumanizeStrategy()'></a>

#### DefaultTimeOnlyHumanizeStrategy\(\) Constructor

Initializes a new instance of the DefaultTimeOnlyHumanizeStrategy class.

```csharp
public DefaultTimeOnlyHumanizeStrategy();
```
### Methods

<a name='Humanizer.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo)'></a>

#### DefaultTimeOnlyHumanizeStrategy\.Humanize\(TimeOnly, TimeOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided times

```csharp
public string Humanize(System.TimeOnly input, System.TimeOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.DefaultTimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(TimeOnly, TimeOnly, CultureInfo\)](Humanizer.ITimeOnlyHumanizeStrategy.md#Humanizer.ITimeOnlyHumanizeStrategy.Humanize(System.TimeOnly,System.TimeOnly,System.Globalization.CultureInfo) 'Humanizer\.ITimeOnlyHumanizeStrategy\.Humanize\(System\.TimeOnly, System\.TimeOnly, System\.Globalization\.CultureInfo\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
