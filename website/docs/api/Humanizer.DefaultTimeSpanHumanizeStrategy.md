---
title: 'Humanizer.DefaultTimeSpanHumanizeStrategy'
sidebar_label: 'Humanizer.DefaultTimeSpanHumanizeStrategy'
description: 'API reference for Humanizer.DefaultTimeSpanHumanizeStrategy.'
---
## DefaultTimeSpanHumanizeStrategy Class

The default strategy for converting [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') values into human\-readable text\.

```csharp
public class DefaultTimeSpanHumanizeStrategy : Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy, Humanizer.ITimeSpanHumanizeStrategy
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultTimeSpanHumanizeStrategy

Implements [IGrammaticalCaseTimeSpanHumanizeStrategy](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy'), [ITimeSpanHumanizeStrategy](Humanizer.ITimeSpanHumanizeStrategy.md 'Humanizer\.ITimeSpanHumanizeStrategy')
- *Constructors*
  - **[DefaultTimeSpanHumanizeStrategy\(\)](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.DefaultTimeSpanHumanizeStrategy())**
- *Methods*
  - **[Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, bool\)](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool) 'Humanizer\.DefaultTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, bool\)')**
  - **[HumanizeWithFractionalSeconds\(TimeSpan, int, bool, CultureInfo, TimeUnit, string, int, MidpointRounding, bool\)](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool) 'Humanizer\.DefaultTimeSpanHumanizeStrategy\.HumanizeWithFractionalSeconds\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, string, int, System\.MidpointRounding, bool\)')**
- *Explicit Interface Implementations*
  - **[Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, GrammaticalCase\)](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase) 'Humanizer\.DefaultTimeSpanHumanizeStrategy\.Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)')**
### Constructors

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.DefaultTimeSpanHumanizeStrategy()'></a>

#### DefaultTimeSpanHumanizeStrategy\(\) Constructor

Initializes a new instance of the DefaultTimeSpanHumanizeStrategy class.

```csharp
public DefaultTimeSpanHumanizeStrategy();
```
### Methods

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool)'></a>

#### DefaultTimeSpanHumanizeStrategy\.Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, bool\) Method

Converts a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into human\-readable text\.

```csharp
public string Humanize(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, Humanizer.TimeUnit minUnit, string? collectionSeparator, bool toWords, bool toSymbols);
```
##### Parameters

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).precision 'Humanizer\.DefaultTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, bool\)\.precision')\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If null, the current culture is used\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether numbers are rendered as words\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether time units are rendered as symbols\.

Implements [Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, bool\)](Humanizer.ITimeSpanHumanizeStrategy.md#Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool) 'Humanizer\.ITimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, bool\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool)'></a>

#### DefaultTimeSpanHumanizeStrategy\.HumanizeWithFractionalSeconds\(TimeSpan, int, bool, CultureInfo, TimeUnit, string, int, MidpointRounding, bool\) Method

Converts a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into human\-readable text with seconds as the minimum unit\.

```csharp
public virtual string HumanizeWithFractionalSeconds(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator, int maxFractionalDigits, System.MidpointRounding roundingMode, bool toSymbols);
```
##### Parameters

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.DefaultTimeSpanHumanizeStrategy.md#Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).precision 'Humanizer\.DefaultTimeSpanHumanizeStrategy\.HumanizeWithFractionalSeconds\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, string, int, System\.MidpointRounding, bool\)\.precision')\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If null, the current culture is used\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\.

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether time units are rendered as symbols\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span\.
### Explicit Interface Implementations

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase)'></a>

#### Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, GrammaticalCase\) Method

```csharp
string Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, Humanizer.TimeUnit minUnit, string? collectionSeparator, bool toSymbols, Humanizer.GrammaticalCase grammaticalCase);
```
##### Parameters

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.DefaultTimeSpanHumanizeStrategy.Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

Implements [Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, GrammaticalCase\)](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md#Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase) 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)')
