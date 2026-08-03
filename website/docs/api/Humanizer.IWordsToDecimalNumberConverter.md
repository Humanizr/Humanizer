---
title: 'Humanizer.IWordsToDecimalNumberConverter'
sidebar_label: 'Humanizer.IWordsToDecimalNumberConverter'
description: 'API reference for Humanizer.IWordsToDecimalNumberConverter.'
---
## IWordsToDecimalNumberConverter Interface

Converts localized decimal number words into [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal') values\.

```csharp
public interface IWordsToDecimalNumberConverter
```
- *Methods*
  - **[Convert\(string\)](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.Convert(string) 'Humanizer\.IWordsToDecimalNumberConverter\.Convert\(string\)')**
  - **[TryConvert\(string, decimal\)](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal) 'Humanizer\.IWordsToDecimalNumberConverter\.TryConvert\(string, decimal\)')**
  - **[TryConvert\(string, decimal, string\)](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string) 'Humanizer\.IWordsToDecimalNumberConverter\.TryConvert\(string, decimal, string\)')**
### Methods

<a name='Humanizer.IWordsToDecimalNumberConverter.Convert(string)'></a>

#### IWordsToDecimalNumberConverter\.Convert\(string\) Method

Converts [words](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.Convert(string).words 'Humanizer\.IWordsToDecimalNumberConverter\.Convert\(string\)\.words') into a decimal value\.

```csharp
decimal Convert(string words);
```
##### Parameters

<a name='Humanizer.IWordsToDecimalNumberConverter.Convert(string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized decimal number phrase to convert\.

##### Returns
[System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')  
The parsed decimal value\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [words](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.Convert(string).words 'Humanizer\.IWordsToDecimalNumberConverter\.Convert\(string\)\.words') is `null`\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
If [words](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.Convert(string).words 'Humanizer\.IWordsToDecimalNumberConverter\.Convert\(string\)\.words') is malformed or outside the supported [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal') range\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
If decimal word parsing is not supported for the converter's locale\.

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal)'></a>

#### IWordsToDecimalNumberConverter\.TryConvert\(string, decimal\) Method

Attempts to convert [words](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal).words 'Humanizer\.IWordsToDecimalNumberConverter\.TryConvert\(string, decimal\)\.words') into a decimal value\.

```csharp
bool TryConvert(string words, out decimal parsedValue);
```
##### Parameters

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized decimal number phrase to convert\.

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal).parsedValue'></a>

`parsedValue` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

When this method returns, contains the parsed value if successful; otherwise, zero\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the phrase was parsed successfully; otherwise, `false`\.

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string)'></a>

#### IWordsToDecimalNumberConverter\.TryConvert\(string, decimal, string\) Method

Attempts to convert [words](Humanizer.IWordsToDecimalNumberConverter.md#Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string).words 'Humanizer\.IWordsToDecimalNumberConverter\.TryConvert\(string, decimal, string\)\.words') into a decimal value and reports the first
unrecognized token when parsing fails\.

```csharp
bool TryConvert(string words, out decimal parsedValue, out string? unrecognizedNumber);
```
##### Parameters

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized decimal number phrase to convert\.

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string).parsedValue'></a>

`parsedValue` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

When this method returns, contains the parsed value if successful; otherwise, zero\.

<a name='Humanizer.IWordsToDecimalNumberConverter.TryConvert(string,decimal,string).unrecognizedNumber'></a>

`unrecognizedNumber` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

When parsing fails, the best\-effort unrecognized token or phrase\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the phrase was parsed successfully; otherwise, `false`\.
