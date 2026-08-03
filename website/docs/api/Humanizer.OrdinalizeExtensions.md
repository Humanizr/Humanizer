---
title: 'Humanizer.OrdinalizeExtensions'
sidebar_label: 'Humanizer.OrdinalizeExtensions'
description: 'API reference for Humanizer.OrdinalizeExtensions.'
---
## OrdinalizeExtensions Class

Ordinalize extensions

```csharp
public static class OrdinalizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → OrdinalizeExtensions

### Remarks
Ordinalization accepts integral values only\. Callers with fractional values must choose and apply
an explicit rounding and conversion policy before ordinalizing\.
- *Methods*
  - **[Ordinalize\(this int\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int\)')**
  - **[Ordinalize\(this int, GrammaticalGender\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, Humanizer\.GrammaticalGender\)')**
  - **[Ordinalize\(this int, GrammaticalGender, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, Humanizer\.GrammaticalGender, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this int, GrammaticalGender, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this int, GrammaticalGender, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this int, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this int, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this int, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this int, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this long\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long\)')**
  - **[Ordinalize\(this long, GrammaticalGender\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, Humanizer\.GrammaticalGender\)')**
  - **[Ordinalize\(this long, GrammaticalGender, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, Humanizer\.GrammaticalGender, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this long, GrammaticalGender, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this long, GrammaticalGender, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this long, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this long, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this long, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this long, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this string\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string\)')**
  - **[Ordinalize\(this string, GrammaticalGender\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, Humanizer\.GrammaticalGender\)')**
  - **[Ordinalize\(this string, GrammaticalGender, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, Humanizer\.GrammaticalGender, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this string, GrammaticalGender, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this string, GrammaticalGender, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this string, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, Humanizer\.WordForm\)')**
  - **[Ordinalize\(this string, CultureInfo\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, System\.Globalization\.CultureInfo\)')**
  - **[Ordinalize\(this string, CultureInfo, WordForm\)](Humanizer.OrdinalizeExtensions.md#Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm) 'Humanizer\.OrdinalizeExtensions\.Ordinalize\(this string, System\.Globalization\.CultureInfo, Humanizer\.WordForm\)')**
### Methods

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this int number);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
1\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
1\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
1.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
1.Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
1\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
1\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
1.Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this int number, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
1.Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this int number, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this int, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this int number, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
1.Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this long number);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, GrammaticalGender\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence using the provided grammatical gender\.

```csharp
public static string Ordinalize(this long number, Humanizer.GrammaticalGender gender);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, GrammaticalGender, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided\.

```csharp
public static string Ordinalize(this long number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, GrammaticalGender, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence using the provided grammatical gender\.

```csharp
public static string Ordinalize(this long number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, GrammaticalGender, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided\.

```csharp
public static string Ordinalize(this long number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this long number, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this long number, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this long, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this long number, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thislong,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this string numberString);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
"1"\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
"1"\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
"1".Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
"1".Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, CultureInfo\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
"1"\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
"1"\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
"1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
"1".Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this string numberString, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
"1".Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, CultureInfo\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this string numberString, System.Globalization.CultureInfo? culture);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

#### OrdinalizeExtensions\.Ordinalize\(this string, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this string numberString, System.Globalization.CultureInfo? culture, Humanizer.WordForm wordForm);
```
##### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

##### Example
In Spanish:

```csharp
"1".Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```
