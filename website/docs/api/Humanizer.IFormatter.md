---
title: 'Humanizer.IFormatter'
sidebar_label: 'Humanizer.IFormatter'
description: 'API reference for Humanizer.IFormatter.'
---
## IFormatter Interface

Localizes Humanizer's number, date, duration, and unit formatting\.

```csharp
public interface IFormatter
```

Derived  
↳ [DefaultFormatter](Humanizer.DefaultFormatter.md 'Humanizer\.DefaultFormatter')  
↳ [IGrammaticalCaseTimeSpanFormatter](Humanizer.IGrammaticalCaseTimeSpanFormatter.md 'Humanizer\.IGrammaticalCaseTimeSpanFormatter')
- *Methods*
  - **[DataUnitHumanize\(DataUnit, double, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool) 'Humanizer\.IFormatter\.DataUnitHumanize\(Humanizer\.DataUnit, double, bool\)')**
  - **[DateHumanize\(TimeUnit, Tense, int\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int) 'Humanizer\.IFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)')**
  - **[DateHumanize\_Never\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Never() 'Humanizer\.IFormatter\.DateHumanize\_Never\(\)')**
  - **[DateHumanize\_Now\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Now() 'Humanizer\.IFormatter\.DateHumanize\_Now\(\)')**
  - **[TimeSpanHumanize\(TimeUnit, int, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool) 'Humanizer\.IFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)')**
  - **[TimeSpanHumanize\_Age\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Age() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Age\(\)')**
  - **[TimeSpanHumanize\_Zero\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Zero() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Zero\(\)')**
  - **[TimeUnitHumanize\(TimeUnit\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit) 'Humanizer\.IFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)')**
### Methods

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool)'></a>

#### IFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the localized representation of a data unit, either as a symbol or a full word\.

```csharp
string DataUnitHumanize(Humanizer.DataUnit dataUnit, double count, bool toSymbol=true);
```
##### Parameters

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [DataUnit](Humanizer.DataUnit.md 'Humanizer\.DataUnit')

The data unit to format\.

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of units being described\.

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the unit should be rendered as a symbol\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized data\-unit representation\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [dataUnit](Humanizer.IFormatter.md#Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit 'Humanizer\.IFormatter\.DataUnitHumanize\(Humanizer\.DataUnit, double, bool\)\.dataUnit') is unsupported\.

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int)'></a>

#### IFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the localized representation of a relative date phrase\.

```csharp
string DateHumanize(Humanizer.TimeUnit timeUnit, Humanizer.Tense timeUnitTense, int unit);
```
##### Parameters

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being described\.

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Tense.md 'Humanizer\.Tense')

Whether the reference is in the past or the future\.

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of units being described\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized relative date phrase\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit 'Humanizer\.IFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)\.timeUnit') is unsupported or [unit](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit 'Humanizer\.IFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)\.unit') is negative\.

<a name='Humanizer.IFormatter.DateHumanize_Never()'></a>

#### IFormatter\.DateHumanize\_Never\(\) Method

Returns the localized text used when a date never occurs\.

```csharp
string DateHumanize_Never();
```

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IFormatter.DateHumanize_Now()'></a>

#### IFormatter\.DateHumanize\_Now\(\) Method

Returns the localized text for the current moment\.

```csharp
string DateHumanize_Now();
```

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool)'></a>

#### IFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the localized representation of a duration\.

```csharp
string TimeSpanHumanize(Humanizer.TimeUnit timeUnit, int unit, bool toWords=false);
```
##### Parameters

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being described\.

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of units being described\.

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the number should be rendered as words\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized duration phrase\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit 'Humanizer\.IFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)\.timeUnit') is unsupported or [unit](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit 'Humanizer\.IFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)\.unit') is negative\.

<a name='Humanizer.IFormatter.TimeSpanHumanize_Age()'></a>

#### IFormatter\.TimeSpanHumanize\_Age\(\) Method

Returns the localized age suffix format for a humanized duration\.

```csharp
string TimeSpanHumanize_Age();
```

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized age suffix format\.

<a name='Humanizer.IFormatter.TimeSpanHumanize_Zero()'></a>

#### IFormatter\.TimeSpanHumanize\_Zero\(\) Method

Returns the localized representation of a zero\-length duration\.

```csharp
string TimeSpanHumanize_Zero();
```

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized zero\-duration phrase\.

<a name='Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit)'></a>

#### IFormatter\.TimeUnitHumanize\(TimeUnit\) Method

Returns the localized symbol for the given [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')\.

```csharp
string TimeUnitHumanize(Humanizer.TimeUnit timeUnit);
```
##### Parameters

<a name='Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The time unit to format\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized symbol for [timeUnit](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit 'Humanizer\.IFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)\.timeUnit')\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit 'Humanizer\.IFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)\.timeUnit') is unsupported\.
