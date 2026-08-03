---
title: 'Humanizer.WordsToNumberExtension'
sidebar_label: 'Humanizer.WordsToNumberExtension'
description: 'API reference for Humanizer.WordsToNumberExtension.'
---
## WordsToNumberExtension Class

Converts localized number words back into numeric values\.
Parsing is culture\-aware, honors locale inheritance, and supports the same natural high\-range
forms that the locale authoring data exposes through `number.words` and
`number.parse`\.

```csharp
public static class WordsToNumberExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → WordsToNumberExtension
- *Methods*
  - **[ToNumber\(this string, CultureInfo\)](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo) 'Humanizer\.WordsToNumberExtension\.ToNumber\(this string, System\.Globalization\.CultureInfo\)')**
  - **[TryToNumber\(this string, long, CultureInfo\)](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo) 'Humanizer\.WordsToNumberExtension\.TryToNumber\(this string, long, System\.Globalization\.CultureInfo\)')**
  - **[TryToNumber\(this string, long, CultureInfo, string\)](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string) 'Humanizer\.WordsToNumberExtension\.TryToNumber\(this string, long, System\.Globalization\.CultureInfo, string\)')**
### Methods

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo)'></a>

#### WordsToNumberExtension\.ToNumber\(this string, CultureInfo\) Method

Converts a spelled\-out number string to its numeric representation\.

```csharp
public static long ToNumber(this string words, System.Globalization.CultureInfo culture);
```
##### Parameters

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number\. Must not be `null`\.

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

##### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The numeric value represented by [words](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).words 'Humanizer\.WordsToNumberExtension\.ToNumber\(this string, System\.Globalization\.CultureInfo\)\.words')\.

##### Exceptions

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
If the input contains unrecognized words or cannot be parsed as a number\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [words](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).words 'Humanizer\.WordsToNumberExtension\.ToNumber\(this string, System\.Globalization\.CultureInfo\)\.words') is `null`\.

##### Remarks
This method now returns [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64') to support locale\-authored high\-range number
parsing beyond [System\.Int32\.MaxValue](https://learn.microsoft.com/en-us/dotnet/api/system.int32.maxvalue 'System\.Int32\.MaxValue')\. Existing code that depended on an [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')
result should either switch the receiving type to [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64') or use an explicit
checked conversion\.
Use [TryToNumber\(this string, long, CultureInfo\)](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo) 'Humanizer\.WordsToNumberExtension\.TryToNumber\(this string, long, System\.Globalization\.CultureInfo\)') when you want a non\-throwing
parse path and the first unrecognized token\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo)'></a>

#### WordsToNumberExtension\.TryToNumber\(this string, long, CultureInfo\) Method

Attempts to convert a spelled\-out number string to its numeric representation without throwing\.

```csharp
public static bool TryToNumber(this string words, out long parsedNumber, System.Globalization.CultureInfo culture);
```
##### Parameters

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number\. Must not be `null`\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo).parsedNumber'></a>

`parsedNumber` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

When this method returns, contains the parsed numeric value if successful; otherwise, 0\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the conversion was successful; otherwise, `false`\.

##### Remarks
This is the recommended method when the input may be invalid\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string)'></a>

#### WordsToNumberExtension\.TryToNumber\(this string, long, CultureInfo, string\) Method

Attempts to convert a spelled\-out number string to its numeric representation and reports the
first unrecognized word if the conversion fails\.

```csharp
public static bool TryToNumber(this string words, out long parsedNumber, System.Globalization.CultureInfo culture, out string? unrecognizedWord);
```
##### Parameters

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number\. Must not be `null`\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string).parsedNumber'></a>

`parsedNumber` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

When this method returns, contains the parsed numeric value if successful; otherwise, 0\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,long,System.Globalization.CultureInfo,string).unrecognizedWord'></a>

`unrecognizedWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

When this method returns `false`, contains the first unrecognized word found in the input\.
When this method returns `true`, this parameter is set to `null`\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the conversion was successful; otherwise, `false`\.

##### Remarks
This overload is useful for debugging or user\-facing validation because it identifies the
first token that could not be recognized\.
