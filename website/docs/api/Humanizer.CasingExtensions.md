---
title: 'Humanizer.CasingExtensions'
sidebar_label: 'Humanizer.CasingExtensions'
description: 'API reference for Humanizer.CasingExtensions.'
---
## CasingExtensions Class

ApplyCase method to allow changing the case of a sentence easily

```csharp
public static class CasingExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → CasingExtensions
- *Methods*
  - **[ApplyCase\(this string, LetterCasing\)](Humanizer.CasingExtensions.md#Humanizer.CasingExtensions.ApplyCase(thisstring,Humanizer.LetterCasing) 'Humanizer\.CasingExtensions\.ApplyCase\(this string, Humanizer\.LetterCasing\)')**
### Methods

<a name='Humanizer.CasingExtensions.ApplyCase(thisstring,Humanizer.LetterCasing)'></a>

#### CasingExtensions\.ApplyCase\(this string, LetterCasing\) Method

Applies the specified letter casing transformation to the input string\.

```csharp
public static string ApplyCase(this string input, Humanizer.LetterCasing casing);
```
##### Parameters

<a name='Humanizer.CasingExtensions.ApplyCase(thisstring,Humanizer.LetterCasing).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to transform\. Must not be null\.

<a name='Humanizer.CasingExtensions.ApplyCase(thisstring,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing style to apply to the input string\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A new string with the specified casing applied\.
\- [Title](Humanizer.LetterCasing.md#Humanizer.LetterCasing.Title 'Humanizer\.LetterCasing\.Title'): Each word is capitalized \(e\.g\., "Some String"\)
\- [LowerCase](Humanizer.LetterCasing.md#Humanizer.LetterCasing.LowerCase 'Humanizer\.LetterCasing\.LowerCase'): All letters are lowercase \(e\.g\., "some string"\)
\- [AllCaps](Humanizer.LetterCasing.md#Humanizer.LetterCasing.AllCaps 'Humanizer\.LetterCasing\.AllCaps'): All letters are uppercase \(e\.g\., "SOME STRING"\)
\- [Sentence](Humanizer.LetterCasing.md#Humanizer.LetterCasing.Sentence 'Humanizer\.LetterCasing\.Sentence'): First character uppercased, remainder unchanged \(e\.g\., "Some string"\)

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when an invalid [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing') value is provided\.

##### Example

```csharp
"some string".ApplyCase(LetterCasing.Title) => "Some String"
"SOME STRING".ApplyCase(LetterCasing.LowerCase) => "some string"
"some string".ApplyCase(LetterCasing.AllCaps) => "SOME STRING"
"some string".ApplyCase(LetterCasing.Sentence) => "Some string"
```
