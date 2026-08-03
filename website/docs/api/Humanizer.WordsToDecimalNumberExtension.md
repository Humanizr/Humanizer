---
title: 'Humanizer.WordsToDecimalNumberExtension'
sidebar_label: 'Humanizer.WordsToDecimalNumberExtension'
description: 'API reference for Humanizer.WordsToDecimalNumberExtension.'
---
## WordsToDecimalNumberExtension Class

Converts localized decimal number words into [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal') values\.

```csharp
public static class WordsToDecimalNumberExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → WordsToDecimalNumberExtension
- *Methods*
  - **[ToDecimalNumber\(this string, CultureInfo\)](Humanizer.WordsToDecimalNumberExtension.md#Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo) 'Humanizer\.WordsToDecimalNumberExtension\.ToDecimalNumber\(this string, System\.Globalization\.CultureInfo\)')**
  - **[TryToDecimalNumber\(this string, decimal, CultureInfo\)](Humanizer.WordsToDecimalNumberExtension.md#Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo) 'Humanizer\.WordsToDecimalNumberExtension\.TryToDecimalNumber\(this string, decimal, System\.Globalization\.CultureInfo\)')**
  - **[TryToDecimalNumber\(this string, decimal, CultureInfo, string\)](Humanizer.WordsToDecimalNumberExtension.md#Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string) 'Humanizer\.WordsToDecimalNumberExtension\.TryToDecimalNumber\(this string, decimal, System\.Globalization\.CultureInfo, string\)')**
### Methods

<a name='Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo)'></a>

#### WordsToDecimalNumberExtension\.ToDecimalNumber\(this string, CultureInfo\) Method

Converts localized decimal number words containing one locale\-specific decimal marker to a decimal value\.

```csharp
public static decimal ToDecimalNumber(this string words, System.Globalization.CultureInfo culture);
```
##### Parameters

<a name='Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The decimal number words to convert\.

<a name='Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

##### Returns
[System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')  
The parsed decimal value, including its authored fractional scale\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [words](Humanizer.WordsToDecimalNumberExtension.md#Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo).words 'Humanizer\.WordsToDecimalNumberExtension\.ToDecimalNumber\(this string, System\.Globalization\.CultureInfo\)\.words') is `null`\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
If the phrase is malformed or outside the supported [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal') range\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
If [culture](Humanizer.WordsToDecimalNumberExtension.md#Humanizer.WordsToDecimalNumberExtension.ToDecimalNumber(thisstring,System.Globalization.CultureInfo).culture 'Humanizer\.WordsToDecimalNumberExtension\.ToDecimalNumber\(this string, System\.Globalization\.CultureInfo\)\.culture') is not supported\.

##### Remarks
The integer part uses the selected culture's words\-to\-number grammar\. The fractional part
requires one to 28 localized digit words\. A supported negative affix applies to the complete
value\. The integer part may be omitted\. A decimal marker and at least one fractional digit
word are required\. Markers and digit words follow the selected grammar's token boundaries\.
English scale phrases may exceed the [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64') range when the result fits in a
[System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal'); other cultures use the range supported by their words\-to\-number grammar\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo)'></a>

#### WordsToDecimalNumberExtension\.TryToDecimalNumber\(this string, decimal, CultureInfo\) Method

Attempts to convert localized decimal number words without throwing for malformed input,
unsupported cultures, or values outside the supported [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal') range\.

```csharp
public static bool TryToDecimalNumber(this string words, out decimal parsedNumber, System.Globalization.CultureInfo culture);
```
##### Parameters

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The decimal number words to convert\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo).parsedNumber'></a>

`parsedNumber` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

When this method returns, contains the parsed value if successful; otherwise, zero\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if conversion succeeded; otherwise, `false`\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string)'></a>

#### WordsToDecimalNumberExtension\.TryToDecimalNumber\(this string, decimal, CultureInfo, string\) Method

Attempts to convert localized decimal number words and reports the first unrecognized token\.

```csharp
public static bool TryToDecimalNumber(this string words, out decimal parsedNumber, System.Globalization.CultureInfo culture, out string? unrecognizedWord);
```
##### Parameters

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The decimal number words to convert\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string).parsedNumber'></a>

`parsedNumber` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

When this method returns, contains the parsed value if successful; otherwise, zero\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

<a name='Humanizer.WordsToDecimalNumberExtension.TryToDecimalNumber(thisstring,decimal,System.Globalization.CultureInfo,string).unrecognizedWord'></a>

`unrecognizedWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

When conversion fails, the best\-effort unrecognized token or phrase\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if conversion succeeded; otherwise, `false`\.
