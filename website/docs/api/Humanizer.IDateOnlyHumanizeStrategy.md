---
title: 'Humanizer.IDateOnlyHumanizeStrategy'
sidebar_label: 'Humanizer.IDateOnlyHumanizeStrategy'
description: 'API reference for Humanizer.IDateOnlyHumanizeStrategy.'
---
## IDateOnlyHumanizeStrategy Interface

Implement this interface to create a new strategy for DateOnly\.Humanize and hook it in the Configurator\.DateOnlyHumanizeStrategy

```csharp
public interface IDateOnlyHumanizeStrategy
```

Derived  
↳ [DefaultDateOnlyHumanizeStrategy](Humanizer.DefaultDateOnlyHumanizeStrategy.md 'Humanizer\.DefaultDateOnlyHumanizeStrategy')  
↳ [PrecisionDateOnlyHumanizeStrategy](Humanizer.PrecisionDateOnlyHumanizeStrategy.md 'Humanizer\.PrecisionDateOnlyHumanizeStrategy')
- *Methods*
  - **[Humanize\(DateOnly, DateOnly, CultureInfo\)](Humanizer.IDateOnlyHumanizeStrategy.md#Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo) 'Humanizer\.IDateOnlyHumanizeStrategy\.Humanize\(System\.DateOnly, System\.DateOnly, System\.Globalization\.CultureInfo\)')**
### Methods

<a name='Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo)'></a>

#### IDateOnlyHumanizeStrategy\.Humanize\(DateOnly, DateOnly, CultureInfo\) Method

Calculates the distance of time in words between two provided dates used for DateOnly\.Humanize

```csharp
string Humanize(System.DateOnly input, System.DateOnly comparisonBase, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).comparisonBase'></a>

`comparisonBase` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.IDateOnlyHumanizeStrategy.Humanize(System.DateOnly,System.DateOnly,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
