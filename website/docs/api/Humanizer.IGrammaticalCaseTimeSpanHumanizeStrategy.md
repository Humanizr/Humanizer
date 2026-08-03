---
title: 'Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy'
sidebar_label: 'Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy'
description: 'API reference for Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.'
---
## IGrammaticalCaseTimeSpanHumanizeStrategy Interface

Optionally extends a time\-span humanization strategy with grammatical\-case support\.
Existing [ITimeSpanHumanizeStrategy](Humanizer.ITimeSpanHumanizeStrategy.md 'Humanizer\.ITimeSpanHumanizeStrategy') implementations remain valid for existing
duration APIs but cannot service `HumanizeWithCase`\.

```csharp
public interface IGrammaticalCaseTimeSpanHumanizeStrategy : Humanizer.ITimeSpanHumanizeStrategy
```

Derived  
↳ [DefaultTimeSpanHumanizeStrategy](Humanizer.DefaultTimeSpanHumanizeStrategy.md 'Humanizer\.DefaultTimeSpanHumanizeStrategy')

Implements [ITimeSpanHumanizeStrategy](Humanizer.ITimeSpanHumanizeStrategy.md 'Humanizer\.ITimeSpanHumanizeStrategy')
- *Methods*
  - **[Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, GrammaticalCase\)](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md#Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase) 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)')**
### Methods

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase)'></a>

#### IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, GrammaticalCase\) Method

Converts a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') using locale\-authored unit\-case phrases\.

```csharp
string Humanize(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, Humanizer.TimeUnit minUnit, string? collectionSeparator, bool toSymbols, Humanizer.GrammaticalCase grammaticalCase);
```
##### Parameters

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md#Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).precision 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)\.precision')\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If null, the current culture is used\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether time units are rendered as symbols\.

<a name='Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case used to select each unit phrase\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A bare locale\-authored duration phrase whose count may be explicit or encoded by the unit form\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[grammaticalCase](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md#Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).grammaticalCase 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)\.grammaticalCase') is outside its defined enum range\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
The selected locale, formatter, or duration unit does not have verified support for
[grammaticalCase](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md#Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,Humanizer.GrammaticalCase).grammaticalCase 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, Humanizer\.GrammaticalCase\)\.grammaticalCase')\.
