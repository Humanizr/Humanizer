---
title: 'Humanizer.IDateToOrdinalWordConverter'
sidebar_label: 'Humanizer.IDateToOrdinalWordConverter'
description: 'API reference for Humanizer.IDateToOrdinalWordConverter.'
---
## IDateToOrdinalWordConverter Interface

Converts dates into the localized text used by `ToOrdinalWords`\.

```csharp
public interface IDateToOrdinalWordConverter
```
- *Methods*
  - **[Convert\(DateTime\)](Humanizer.IDateToOrdinalWordConverter.md#Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime) 'Humanizer\.IDateToOrdinalWordConverter\.Convert\(System\.DateTime\)')**
  - **[Convert\(DateTime, GrammaticalCase\)](Humanizer.IDateToOrdinalWordConverter.md#Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase) 'Humanizer\.IDateToOrdinalWordConverter\.Convert\(System\.DateTime, Humanizer\.GrammaticalCase\)')**
### Methods

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime)'></a>

#### IDateToOrdinalWordConverter\.Convert\(DateTime\) Method

Converts the given [date](Humanizer.IDateToOrdinalWordConverter.md#Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime).date 'Humanizer\.IDateToOrdinalWordConverter\.Convert\(System\.DateTime\)\.date') to ordinal words for the current culture\.

```csharp
string Convert(System.DateTime date);
```
##### Parameters

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to format\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal\-date string\.

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase)'></a>

#### IDateToOrdinalWordConverter\.Convert\(DateTime, GrammaticalCase\) Method

Converts the given [date](Humanizer.IDateToOrdinalWordConverter.md#Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase).date 'Humanizer\.IDateToOrdinalWordConverter\.Convert\(System\.DateTime, Humanizer\.GrammaticalCase\)\.date') to ordinal words using the specified grammatical case\.

```csharp
string Convert(System.DateTime date, Humanizer.GrammaticalCase grammaticalCase);
```
##### Parameters

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to format\.

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case to apply when the locale supports case\-specific date forms\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal\-date string\.
