---
title: 'Humanizer.IGrammaticalCaseTimeSpanFormatter'
sidebar_label: 'Humanizer.IGrammaticalCaseTimeSpanFormatter'
description: 'API reference for Humanizer.IGrammaticalCaseTimeSpanFormatter.'
---
## IGrammaticalCaseTimeSpanFormatter Interface

Optionally extends a locale formatter with grammatical\-case\-aware duration phrases\.

```csharp
public interface IGrammaticalCaseTimeSpanFormatter : Humanizer.IFormatter
```

Derived  
↳ [DefaultFormatter](Humanizer.DefaultFormatter.md 'Humanizer\.DefaultFormatter')

Implements [IFormatter](Humanizer.IFormatter.md 'Humanizer\.IFormatter')
- *Methods*
  - **[TimeSpanHumanize\(TimeUnit, int, GrammaticalCase\)](Humanizer.IGrammaticalCaseTimeSpanFormatter.md#Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase) 'Humanizer\.IGrammaticalCaseTimeSpanFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, Humanizer\.GrammaticalCase\)')**
### Methods

<a name='Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase)'></a>

#### IGrammaticalCaseTimeSpanFormatter\.TimeSpanHumanize\(TimeUnit, int, GrammaticalCase\) Method

Returns the locale\-authored unit\-case phrase for a duration count\.

```csharp
string TimeSpanHumanize(Humanizer.TimeUnit timeUnit, int unit, Humanizer.GrammaticalCase grammaticalCase);
```
##### Parameters

<a name='Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being described\.

<a name='Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of units being described\.

<a name='Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case used to select the unit phrase\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A locale\-authored phrase whose count may be explicit or encoded by the unit form\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[timeUnit](Humanizer.IGrammaticalCaseTimeSpanFormatter.md#Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).timeUnit 'Humanizer\.IGrammaticalCaseTimeSpanFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, Humanizer\.GrammaticalCase\)\.timeUnit') or [grammaticalCase](Humanizer.IGrammaticalCaseTimeSpanFormatter.md#Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).grammaticalCase 'Humanizer\.IGrammaticalCaseTimeSpanFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, Humanizer\.GrammaticalCase\)\.grammaticalCase') is outside its defined enum range\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
The locale or duration unit does not have verified support for [grammaticalCase](Humanizer.IGrammaticalCaseTimeSpanFormatter.md#Humanizer.IGrammaticalCaseTimeSpanFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,Humanizer.GrammaticalCase).grammaticalCase 'Humanizer\.IGrammaticalCaseTimeSpanFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, Humanizer\.GrammaticalCase\)\.grammaticalCase')\.
