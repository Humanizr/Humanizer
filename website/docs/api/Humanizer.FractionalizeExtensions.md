---
title: 'Humanizer.FractionalizeExtensions'
sidebar_label: 'Humanizer.FractionalizeExtensions'
description: 'API reference for Humanizer.FractionalizeExtensions.'
---
## FractionalizeExtensions Class

Contains extension methods for converting decimals to common fractions\.

```csharp
public static class FractionalizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → FractionalizeExtensions
- *Methods*
  - **[Fractionalize\(this decimal, int, decimal, bool\)](Humanizer.FractionalizeExtensions.md#Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool) 'Humanizer\.FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\)')**
### Methods

<a name='Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool)'></a>

#### FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\) Method

Converts a decimal to the closest reduced fraction whose denominator does not exceed the specified maximum\.

```csharp
public static string Fractionalize(this decimal input, int maxDenominator, decimal tolerance, bool useUnicode=false);
```
##### Parameters

<a name='Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).input'></a>

`input` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The decimal to convert\.

<a name='Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).maxDenominator'></a>

`maxDenominator` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest denominator that may be used\.

<a name='Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).tolerance'></a>

`tolerance` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The maximum absolute difference allowed between the input and the fraction\.

<a name='Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).useUnicode'></a>

`useUnicode` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to use a Unicode vulgar\-fraction character when an exact character exists\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A whole number, proper fraction, or mixed fraction when the closest fraction is within
[tolerance](Humanizer.FractionalizeExtensions.md#Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).tolerance 'Humanizer\.FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\)\.tolerance'); otherwise, the culture\-formatted input\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [maxDenominator](Humanizer.FractionalizeExtensions.md#Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).maxDenominator 'Humanizer\.FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\)\.maxDenominator') is less than one or [tolerance](Humanizer.FractionalizeExtensions.md#Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).tolerance 'Humanizer\.FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\)\.tolerance') is negative\.

##### Example

```csharp
1.25m.Fractionalize(5, 0m) => "1 1/4"
0.34m.Fractionalize(5, 0.01m) => "1/3"
0.75m.Fractionalize(4, 0m, useUnicode: true) => "¾"
```

##### Remarks
The tolerance boundary is inclusive\. Equidistant fractions prefer the smaller denominator,
then the value farther from zero\. Fraction components use invariant digits; slash notation is used
unless [useUnicode](Humanizer.FractionalizeExtensions.md#Humanizer.FractionalizeExtensions.Fractionalize(thisdecimal,int,decimal,bool).useUnicode 'Humanizer\.FractionalizeExtensions\.Fractionalize\(this decimal, int, decimal, bool\)\.useUnicode') requests an available exact glyph\.
