---
title: 'Humanizer.ILongOrdinalizer'
sidebar_label: 'Humanizer.ILongOrdinalizer'
description: 'API reference for Humanizer.ILongOrdinalizer.'
---
## ILongOrdinalizer Interface

Localizes the ordinal form of a 64\-bit integer\.

```csharp
public interface ILongOrdinalizer : Humanizer.IOrdinalizer
```

Implements [IOrdinalizer](Humanizer.IOrdinalizer.md 'Humanizer\.IOrdinalizer')

### Remarks
Implement this interface when registering a custom ordinalizer that supports values outside the
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32') range\. Existing [IOrdinalizer](Humanizer.IOrdinalizer.md 'Humanizer\.IOrdinalizer') implementations remain supported for
32\-bit values\.
- *Methods*
  - **[Convert\(long, string\)](Humanizer.ILongOrdinalizer.md#Humanizer.ILongOrdinalizer.Convert(long,string) 'Humanizer\.ILongOrdinalizer\.Convert\(long, string\)')**
  - **[Convert\(long, string, GrammaticalGender\)](Humanizer.ILongOrdinalizer.md#Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender) 'Humanizer\.ILongOrdinalizer\.Convert\(long, string, Humanizer\.GrammaticalGender\)')**
  - **[Convert\(long, string, GrammaticalGender, WordForm\)](Humanizer.ILongOrdinalizer.md#Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm) 'Humanizer\.ILongOrdinalizer\.Convert\(long, string, Humanizer\.GrammaticalGender, Humanizer\.WordForm\)')**
  - **[Convert\(long, string, WordForm\)](Humanizer.ILongOrdinalizer.md#Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.WordForm) 'Humanizer\.ILongOrdinalizer\.Convert\(long, string, Humanizer\.WordForm\)')**
### Methods

<a name='Humanizer.ILongOrdinalizer.Convert(long,string)'></a>

#### ILongOrdinalizer\.Convert\(long, string\) Method

Ordinalizes the number using the default grammatical form\.

```csharp
string Convert(long number, string numberString);
```
##### Parameters

<a name='Humanizer.ILongOrdinalizer.Convert(long,string).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The numeric value being ordinalized\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender)'></a>

#### ILongOrdinalizer\.Convert\(long, string, GrammaticalGender\) Method

Ordinalizes the number using the provided grammatical gender\.

```csharp
string Convert(long number, string numberString, Humanizer.GrammaticalGender gender);
```
##### Parameters

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The numeric value being ordinalized\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale requires one\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

#### ILongOrdinalizer\.Convert\(long, string, GrammaticalGender, WordForm\) Method

Ordinalizes the number using the provided grammatical gender and word form\.

```csharp
string Convert(long number, string numberString, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The numeric value being ordinalized\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale requires one\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The word form to use when the locale distinguishes abbreviations from full words\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.WordForm)'></a>

#### ILongOrdinalizer\.Convert\(long, string, WordForm\) Method

Ordinalizes the number using a locale\-specific word form\.

```csharp
string Convert(long number, string numberString, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The numeric value being ordinalized\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.ILongOrdinalizer.Convert(long,string,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The word form to use when the locale distinguishes abbreviations from full words\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.
