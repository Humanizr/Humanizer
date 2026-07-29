## CardinalInflectionForms Class

Provides authored forms of one common noun for cardinal\-count inflection\.

```csharp
public sealed class CardinalInflectionForms
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → CardinalInflectionForms

### Remarks
These forms represent a bare noun governed by a cardinal count\. They do not model articles,
adjectives, classifiers, independently selectable grammatical case or gender, or complete noun
phrases\. An authored form can include the case governed by its cardinal quantity\.
### Constructors

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string)'></a>

## CardinalInflectionForms\(string, string, string, string, string, string, string\) Constructor

Creates a set of authored cardinal forms\.

```csharp
public CardinalInflectionForms(string lemma, string other, string? zero=null, string? one=null, string? two=null, string? few=null, string? many=null);
```
#### Parameters

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).lemma'></a>

`lemma` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The noun's citation form\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).other'></a>

`other` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `other` category\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).zero'></a>

`zero` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `zero` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).one'></a>

`one` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `one` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).two'></a>

`two` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `two` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).few'></a>

`few` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `few` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

<a name='Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).many'></a>

`many` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The form for the CLDR `many` category, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when unavailable\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
[lemma](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).lemma 'Humanizer\.CardinalInflectionForms\.CardinalInflectionForms\(string, string, string, string, string, string, string\)\.lemma') or [other](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).other 'Humanizer\.CardinalInflectionForms\.CardinalInflectionForms\(string, string, string, string, string, string, string\)\.other') is empty or whitespace, or an optional supplied form is empty or whitespace\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[lemma](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).lemma 'Humanizer\.CardinalInflectionForms\.CardinalInflectionForms\(string, string, string, string, string, string, string\)\.lemma') or [other](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.CardinalInflectionForms(string,string,string,string,string,string,string).other 'Humanizer\.CardinalInflectionForms\.CardinalInflectionForms\(string, string, string, string, string, string, string\)\.other') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.
### Properties

<a name='Humanizer.CardinalInflectionForms.Few'></a>

## CardinalInflectionForms\.Few Property

Gets the form for the CLDR `few` category\.

```csharp
public string? Few { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.Lemma'></a>

## CardinalInflectionForms\.Lemma Property

Gets the noun's citation form\.

```csharp
public string Lemma { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.Many'></a>

## CardinalInflectionForms\.Many Property

Gets the form for the CLDR `many` category\.

```csharp
public string? Many { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.One'></a>

## CardinalInflectionForms\.One Property

Gets the form for the CLDR `one` category\.

```csharp
public string? One { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.Other'></a>

## CardinalInflectionForms\.Other Property

Gets the form for the CLDR `other` category\.

```csharp
public string Other { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.Two'></a>

## CardinalInflectionForms\.Two Property

Gets the form for the CLDR `two` category\.

```csharp
public string? Two { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CardinalInflectionForms.Zero'></a>

## CardinalInflectionForms\.Zero Property

Gets the form for the CLDR `zero` category\.

```csharp
public string? Zero { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='Humanizer.CardinalInflectionForms.Invariant(string)'></a>

## CardinalInflectionForms\.Invariant\(string\) Method

Creates forms for a noun that remains unchanged in every cardinal category\.

```csharp
public static Humanizer.CardinalInflectionForms Invariant(string lemma);
```
#### Parameters

<a name='Humanizer.CardinalInflectionForms.Invariant(string).lemma'></a>

`lemma` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The invariant citation form\.

#### Returns
[CardinalInflectionForms](Humanizer.CardinalInflectionForms.md 'Humanizer\.CardinalInflectionForms')  
A form set containing [lemma](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.Invariant(string).lemma 'Humanizer\.CardinalInflectionForms\.Invariant\(string\)\.lemma') for every category\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
[lemma](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.Invariant(string).lemma 'Humanizer\.CardinalInflectionForms\.Invariant\(string\)\.lemma') is empty or whitespace\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[lemma](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.Invariant(string).lemma 'Humanizer\.CardinalInflectionForms\.Invariant\(string\)\.lemma') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.