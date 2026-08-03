---
title: 'Humanizer.IWordsToNumberConverter'
sidebar_label: 'Humanizer.IWordsToNumberConverter'
description: 'API reference for Humanizer.IWordsToNumberConverter.'
---
## IWordsToNumberConverter Interface

Converts localized number words into numeric values\.

```csharp
public interface IWordsToNumberConverter
```

### Remarks
Implementations expect a meaningful localized number phrase\. They may throw
[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException') for `null`, empty, or whitespace input, and fallback
implementations for unsupported locales may throw [System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException') instead
of returning a parse failure\.
- *Methods*
  - **[Convert\(string\)](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.Convert(string) 'Humanizer\.IWordsToNumberConverter\.Convert\(string\)')**
  - **[TryConvert\(string, long\)](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long) 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long\)')**
  - **[TryConvert\(string, long, string\)](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long,string) 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long, string\)')**
### Methods

<a name='Humanizer.IWordsToNumberConverter.Convert(string)'></a>

#### IWordsToNumberConverter\.Convert\(string\) Method

Converts [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.Convert(string).words 'Humanizer\.IWordsToNumberConverter\.Convert\(string\)\.words') into a numeric value\.

```csharp
long Convert(string words);
```
##### Parameters

<a name='Humanizer.IWordsToNumberConverter.Convert(string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized number phrase to convert\.

##### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The parsed numeric value\.

##### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.Convert(string).words 'Humanizer\.IWordsToNumberConverter\.Convert\(string\)\.words') is `null`, empty, whitespace, or cannot be parsed by the current implementation\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
If the current implementation does not support words\-to\-number conversion for its locale\.

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long)'></a>

#### IWordsToNumberConverter\.TryConvert\(string, long\) Method

Attempts to convert [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long).words 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long\)\.words') into a numeric value\.

```csharp
bool TryConvert(string words, out long parsedValue);
```
##### Parameters

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized number phrase to convert\.

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long).parsedValue'></a>

`parsedValue` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

When this method returns, contains the parsed numeric value\. The value is meaningful only when the method returns `true`\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the phrase was parsed successfully; otherwise, `false`\.

##### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long).words 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long\)\.words') is `null`, empty, or whitespace\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
If the current implementation does not support words\-to\-number conversion for its locale\.

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long,string)'></a>

#### IWordsToNumberConverter\.TryConvert\(string, long, string\) Method

Attempts to convert [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long,string).words 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long, string\)\.words') into a numeric value and reports the first
token\-like fragment that could not be interpreted when parsing fails\.

```csharp
bool TryConvert(string words, out long parsedValue, out string? unrecognizedNumber);
```
##### Parameters

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized number phrase to convert\.

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long,string).parsedValue'></a>

`parsedValue` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

When this method returns, contains the parsed numeric value\. The value is meaningful only when the method returns `true`\.

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,long,string).unrecognizedNumber'></a>

`unrecognizedNumber` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

When parsing fails, the best\-effort token or fragment that remained unrecognized\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the value was parsed successfully; otherwise, `false`\.

##### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If [words](Humanizer.IWordsToNumberConverter.md#Humanizer.IWordsToNumberConverter.TryConvert(string,long,string).words 'Humanizer\.IWordsToNumberConverter\.TryConvert\(string, long, string\)\.words') is `null`, empty, or whitespace\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
If the current implementation does not support words\-to\-number conversion for its locale\.
