---
title: 'Humanizer.IDateOnlyToOrdinalWordConverter'
sidebar_label: 'Humanizer.IDateOnlyToOrdinalWordConverter'
description: 'API reference for Humanizer.IDateOnlyToOrdinalWordConverter.'
---
## IDateOnlyToOrdinalWordConverter Interface

Converts dates into the localized text used by `ToOrdinalWords`\.

```csharp
public interface IDateOnlyToOrdinalWordConverter
```
- *Methods*
  - **[Convert\(DateOnly\)](Humanizer.IDateOnlyToOrdinalWordConverter.md#Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly) 'Humanizer\.IDateOnlyToOrdinalWordConverter\.Convert\(System\.DateOnly\)')**
  - **[Convert\(DateOnly, GrammaticalCase\)](Humanizer.IDateOnlyToOrdinalWordConverter.md#Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase) 'Humanizer\.IDateOnlyToOrdinalWordConverter\.Convert\(System\.DateOnly, Humanizer\.GrammaticalCase\)')**
### Methods

<a name='Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly)'></a>

#### IDateOnlyToOrdinalWordConverter\.Convert\(DateOnly\) Method

Converts the given [date](Humanizer.IDateOnlyToOrdinalWordConverter.md#Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly).date 'Humanizer\.IDateOnlyToOrdinalWordConverter\.Convert\(System\.DateOnly\)\.date') to ordinal words for the current culture\.

```csharp
string Convert(System.DateOnly date);
```
##### Parameters

<a name='Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly).date'></a>

`date` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

The date to format\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal\-date string\.

<a name='Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase)'></a>

#### IDateOnlyToOrdinalWordConverter\.Convert\(DateOnly, GrammaticalCase\) Method

Converts the given [date](Humanizer.IDateOnlyToOrdinalWordConverter.md#Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase).date 'Humanizer\.IDateOnlyToOrdinalWordConverter\.Convert\(System\.DateOnly, Humanizer\.GrammaticalCase\)\.date') to ordinal words using the specified grammatical case\.

```csharp
string Convert(System.DateOnly date, Humanizer.GrammaticalCase grammaticalCase);
```
##### Parameters

<a name='Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase).date'></a>

`date` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

The date to format\.

<a name='Humanizer.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case to apply when the locale supports case\-specific date forms\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal\-date string\.
