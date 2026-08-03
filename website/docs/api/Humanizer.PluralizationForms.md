---
title: 'Humanizer.PluralizationForms'
sidebar_label: 'Humanizer.PluralizationForms'
description: 'API reference for Humanizer.PluralizationForms.'
---
## PluralizationForms Class

Provides authored singular and plural forms of one noun\.

```csharp
public sealed class PluralizationForms
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PluralizationForms

### Remarks
Humanizer applies the supported culture's cardinal plural rule to select an authored form\.
Missing selected forms are not inferred\.
- *Constructors*
  - **[PluralizationForms\(string, string, string, string, string, string, string\)](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string) 'Humanizer\.PluralizationForms\.PluralizationForms\(string, string, string, string, string, string, string\)')**
- *Properties*
  - **[Few](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Few 'Humanizer\.PluralizationForms\.Few')**
  - **[Many](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Many 'Humanizer\.PluralizationForms\.Many')**
  - **[One](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.One 'Humanizer\.PluralizationForms\.One')**
  - **[Other](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Other 'Humanizer\.PluralizationForms\.Other')**
  - **[Singular](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Singular 'Humanizer\.PluralizationForms\.Singular')**
  - **[Two](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Two 'Humanizer\.PluralizationForms\.Two')**
  - **[Zero](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Zero 'Humanizer\.PluralizationForms\.Zero')**
- *Methods*
  - **[Invariant\(string\)](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Invariant(string) 'Humanizer\.PluralizationForms\.Invariant\(string\)')**
  - **[TryPluralize\(decimal, CultureInfo, string\)](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string) 'Humanizer\.PluralizationForms\.TryPluralize\(decimal, System\.Globalization\.CultureInfo, string\)')**
  - **[TrySingularize\(string, string\)](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TrySingularize(string,string) 'Humanizer\.PluralizationForms\.TrySingularize\(string, string\)')**
### Constructors

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string)'></a>

#### PluralizationForms\(string, string, string, string, string, string, string\) Constructor

Creates a set of authored singular and plural forms\.

```csharp
public PluralizationForms(string singular, string other, string? zero=null, string? one=null, string? two=null, string? few=null, string? many=null);
```
##### Parameters

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).singular'></a>

`singular` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The noun's singular form\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).other'></a>

`other` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `other` category\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).zero'></a>

`zero` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `zero` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).one'></a>

`one` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `one` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).two'></a>

`two` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `two` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).few'></a>

`few` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `few` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).many'></a>

`many` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `many` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

##### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
[singular](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).singular 'Humanizer\.PluralizationForms\.PluralizationForms\(string, string, string, string, string, string, string\)\.singular') or [other](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).other 'Humanizer\.PluralizationForms\.PluralizationForms\(string, string, string, string, string, string, string\)\.other') is empty or whitespace, or an optional supplied form is empty or whitespace\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[singular](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).singular 'Humanizer\.PluralizationForms\.PluralizationForms\(string, string, string, string, string, string, string\)\.singular') or [other](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.PluralizationForms(string,string,string,string,string,string,string).other 'Humanizer\.PluralizationForms\.PluralizationForms\(string, string, string, string, string, string, string\)\.other') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.
### Properties

<a name='Humanizer.PluralizationForms.Few'></a>

#### PluralizationForms\.Few Property

Gets the form for the CLDR `few` category\.

```csharp
public string? Few { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.Many'></a>

#### PluralizationForms\.Many Property

Gets the form for the CLDR `many` category\.

```csharp
public string? Many { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.One'></a>

#### PluralizationForms\.One Property

Gets the form for the CLDR `one` category\.

```csharp
public string? One { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.Other'></a>

#### PluralizationForms\.Other Property

Gets the form for the CLDR `other` category\.

```csharp
public string Other { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.Singular'></a>

#### PluralizationForms\.Singular Property

Gets the noun's singular form\.

```csharp
public string Singular { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.Two'></a>

#### PluralizationForms\.Two Property

Gets the form for the CLDR `two` category\.

```csharp
public string? Two { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.PluralizationForms.Zero'></a>

#### PluralizationForms\.Zero Property

Gets the form for the CLDR `zero` category\.

```csharp
public string? Zero { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='Humanizer.PluralizationForms.Invariant(string)'></a>

#### PluralizationForms\.Invariant\(string\) Method

Creates forms for a noun that remains unchanged in every cardinal category\.

```csharp
public static Humanizer.PluralizationForms Invariant(string word);
```
##### Parameters

<a name='Humanizer.PluralizationForms.Invariant(string).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The invariant word\.

##### Returns
[PluralizationForms](Humanizer.PluralizationForms.md 'Humanizer\.PluralizationForms')  
A form set containing [word](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Invariant(string).word 'Humanizer\.PluralizationForms\.Invariant\(string\)\.word') for every category\.

##### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
[word](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Invariant(string).word 'Humanizer\.PluralizationForms\.Invariant\(string\)\.word') is empty or whitespace\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[word](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.Invariant(string).word 'Humanizer\.PluralizationForms\.Invariant\(string\)\.word') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string)'></a>

#### PluralizationForms\.TryPluralize\(decimal, CultureInfo, string\) Method

Attempts to pluralize this noun for a quantity using the supported culture's cardinal plural rule\.

```csharp
public bool TryPluralize(decimal quantity, System.Globalization.CultureInfo culture, out string? result);
```
##### Parameters

<a name='Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string).quantity'></a>

`quantity` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The quantity\. Its encoded decimal scale supplies CLDR visible\-fraction operands\.

<a name='Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The supported culture whose cardinal plural rule is applied\.

<a name='Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string).result'></a>

`result` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The selected authored form when available; otherwise, [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the selected form is available; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[culture](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TryPluralize(decimal,System.Globalization.CultureInfo,string).culture 'Humanizer\.PluralizationForms\.TryPluralize\(decimal, System\.Globalization\.CultureInfo, string\)\.culture') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='Humanizer.PluralizationForms.TrySingularize(string,string)'></a>

#### PluralizationForms\.TrySingularize\(string, string\) Method

Attempts to resolve one of this noun's exact authored forms to its singular form\.

```csharp
public bool TrySingularize(string form, out string? result);
```
##### Parameters

<a name='Humanizer.PluralizationForms.TrySingularize(string,string).form'></a>

`form` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An exact authored form\.

<a name='Humanizer.PluralizationForms.TrySingularize(string,string).result'></a>

`result` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The singular form when [form](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TrySingularize(string,string).form 'Humanizer\.PluralizationForms\.TrySingularize\(string, string\)\.form') matches; otherwise, [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when [form](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TrySingularize(string,string).form 'Humanizer\.PluralizationForms\.TrySingularize\(string, string\)\.form') matches an authored form; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[form](Humanizer.PluralizationForms.md#Humanizer.PluralizationForms.TrySingularize(string,string).form 'Humanizer\.PluralizationForms\.TrySingularize\(string, string\)\.form') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

##### Remarks
Matching is Unicode NFC\-normalized, ordinal, and case\-sensitive\.
