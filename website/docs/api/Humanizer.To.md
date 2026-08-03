---
title: 'Humanizer.To'
sidebar_label: 'Humanizer.To'
description: 'API reference for Humanizer.To.'
---
## To Class

A portal to string transformation using IStringTransformer

```csharp
public static class To
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → To
- *Properties*
  - **[LowerCase](Humanizer.To.md#Humanizer.To.LowerCase 'Humanizer\.To\.LowerCase')**
  - **[SentenceCase](Humanizer.To.md#Humanizer.To.SentenceCase 'Humanizer\.To\.SentenceCase')**
  - **[TitleCase](Humanizer.To.md#Humanizer.To.TitleCase 'Humanizer\.To\.TitleCase')**
  - **[UpperCase](Humanizer.To.md#Humanizer.To.UpperCase 'Humanizer\.To\.UpperCase')**
- *Methods*
  - **[Transform\(this string, IStringTransformer\[\]\)](Humanizer.To.md#Humanizer.To.Transform(thisstring,Humanizer.IStringTransformer[]) 'Humanizer\.To\.Transform\(this string, Humanizer\.IStringTransformer\[\]\)')**
  - **[Transform\(this string, CultureInfo, ICulturedStringTransformer\[\]\)](Humanizer.To.md#Humanizer.To.Transform(thisstring,System.Globalization.CultureInfo,Humanizer.ICulturedStringTransformer[]) 'Humanizer\.To\.Transform\(this string, System\.Globalization\.CultureInfo, Humanizer\.ICulturedStringTransformer\[\]\)')**
### Properties

<a name='Humanizer.To.LowerCase'></a>

#### To\.LowerCase Property

Changes the string to lower case

```csharp
public static Humanizer.ICulturedStringTransformer LowerCase { get; }
```

##### Property Value
[ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')

##### Example
"Sentence casing" \-\> "sentence casing"

<a name='Humanizer.To.SentenceCase'></a>

#### To\.SentenceCase Property

Uppercases the first character of the string, leaving all remaining characters unchanged

```csharp
public static Humanizer.ICulturedStringTransformer SentenceCase { get; }
```

##### Property Value
[ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')

##### Example
"lower case statement" \-\> "Lower case statement"

<a name='Humanizer.To.TitleCase'></a>

#### To\.TitleCase Property

Changes string to title case

```csharp
public static Humanizer.ICulturedStringTransformer TitleCase { get; }
```

##### Property Value
[ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')

##### Example
"INvalid caSEs arE corrected" \-\> "Invalid Cases Are Corrected"

<a name='Humanizer.To.UpperCase'></a>

#### To\.UpperCase Property

Changes the string to upper case

```csharp
public static Humanizer.ICulturedStringTransformer UpperCase { get; }
```

##### Property Value
[ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')

##### Example
"lower case statement" \-\> "LOWER CASE STATEMENT"
### Methods

<a name='Humanizer.To.Transform(thisstring,Humanizer.IStringTransformer[])'></a>

#### To\.Transform\(this string, IStringTransformer\[\]\) Method

Transforms a string using the provided transformers\. Transformations are applied in the provided order\.

```csharp
public static string Transform(this string input, params Humanizer.IStringTransformer[] transformers);
```
##### Parameters

<a name='Humanizer.To.Transform(thisstring,Humanizer.IStringTransformer[]).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.To.Transform(thisstring,Humanizer.IStringTransformer[]).transformers'></a>

`transformers` [IStringTransformer](Humanizer.IStringTransformer.md 'Humanizer\.IStringTransformer')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.To.Transform(thisstring,System.Globalization.CultureInfo,Humanizer.ICulturedStringTransformer[])'></a>

#### To\.Transform\(this string, CultureInfo, ICulturedStringTransformer\[\]\) Method

Transforms a string using the provided transformers\. Transformations are applied in the provided order\.

```csharp
public static string Transform(this string input, System.Globalization.CultureInfo culture, params Humanizer.ICulturedStringTransformer[] transformers);
```
##### Parameters

<a name='Humanizer.To.Transform(thisstring,System.Globalization.CultureInfo,Humanizer.ICulturedStringTransformer[]).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.To.Transform(thisstring,System.Globalization.CultureInfo,Humanizer.ICulturedStringTransformer[]).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

<a name='Humanizer.To.Transform(thisstring,System.Globalization.CultureInfo,Humanizer.ICulturedStringTransformer[]).transformers'></a>

`transformers` [ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
