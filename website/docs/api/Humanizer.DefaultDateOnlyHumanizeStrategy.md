---
title: 'Humanizer.DefaultDateOnlyHumanizeStrategy'
sidebar_label: 'Humanizer.DefaultDateOnlyHumanizeStrategy'
description: 'API reference for Humanizer.DefaultDateOnlyHumanizeStrategy.'
---
## DefaultDateOnlyHumanizeStrategy Class

The default 'distance of time' \-\> words calculator\.

```csharp
public class DefaultDateOnlyHumanizeStrategy : Humanizer.IDateOnlyHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultDateOnlyHumanizeStrategy

Implements [IDateOnlyHumanizeStrategy](Humanizer.IDateOnlyHumanizeStrategy.md 'Humanizer\.IDateOnlyHumanizeStrategy')
- *Constructors*
  - **[DefaultDateOnlyHumanizeStrategy\(\)](Humanizer.DefaultDateOnlyHumanizeStrategy.md#Humanizer.DefaultDateOnlyHumanizeStrategy.DefaultDateOnlyHumanizeStrategy())**
- *Methods*
  - **[Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.DefaultDateOnlyHumanizeStrategy.md#Humanizer.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.DefaultDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')**
### Constructors

<a name='Humanizer.DefaultDateOnlyHumanizeStrategy.DefaultDateOnlyHumanizeStrategy()'></a>

#### DefaultDateOnlyHumanizeStrategy\(\) Constructor

Initializes a new instance of the DefaultDateOnlyHumanizeStrategy class.

```csharp
public DefaultDateOnlyHumanizeStrategy();
```
### Methods

<a name='Humanizer.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo)'></a>

#### DefaultDateOnlyHumanizeStrategy\.Humanize\(DateOnly, DateOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided dates

```csharp
public string Humanize(System.DateOnly input, System.DateOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.DefaultDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Implements [Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.IDateOnlyHumanizeStrategy.md#Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.IDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
