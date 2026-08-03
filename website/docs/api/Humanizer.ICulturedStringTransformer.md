---
title: 'Humanizer.ICulturedStringTransformer'
sidebar_label: 'Humanizer.ICulturedStringTransformer'
description: 'API reference for Humanizer.ICulturedStringTransformer.'
---
## ICulturedStringTransformer Interface

Can transform a string with the given culture

```csharp
public interface ICulturedStringTransformer : Humanizer.IStringTransformer
```

Implements [IStringTransformer](Humanizer.IStringTransformer.md 'Humanizer\.IStringTransformer')
- *Methods*
  - **[Transform\(string, CultureInfo\)](Humanizer.ICulturedStringTransformer.md#Humanizer.ICulturedStringTransformer.Transform(string,System.Globalization.CultureInfo) 'Humanizer\.ICulturedStringTransformer\.Transform\(string, System\.Globalization\.CultureInfo\)')**
### Methods

<a name='Humanizer.ICulturedStringTransformer.Transform(string,System.Globalization.CultureInfo)'></a>

#### ICulturedStringTransformer\.Transform\(string, CultureInfo\) Method

Transform the input

```csharp
string Transform(string input, System.Globalization.CultureInfo culture);
```
##### Parameters

<a name='Humanizer.ICulturedStringTransformer.Transform(string,System.Globalization.CultureInfo).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

String to be transformed

<a name='Humanizer.ICulturedStringTransformer.Transform(string,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
