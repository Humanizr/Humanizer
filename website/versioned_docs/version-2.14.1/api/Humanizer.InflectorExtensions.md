## InflectorExtensions Class

Inflector extensions

```csharp
public static class InflectorExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → InflectorExtensions
### Methods

<a name='Humanizer.InflectorExtensions.Camelize(thisstring)'></a>

## InflectorExtensions\.Camelize\(this string\) Method

Same as Pascalize except that the first character is lower case

```csharp
public static string Camelize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Camelize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Dasherize(thisstring)'></a>

## InflectorExtensions\.Dasherize\(this string\) Method

Replaces underscores with dashes in the string

```csharp
public static string Dasherize(this string underscoredWord);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Dasherize(thisstring).underscoredWord'></a>

`underscoredWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Hyphenate(thisstring)'></a>

## InflectorExtensions\.Hyphenate\(this string\) Method

Replaces underscores with hyphens in the string

```csharp
public static string Hyphenate(this string underscoredWord);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Hyphenate(thisstring).underscoredWord'></a>

`underscoredWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Kebaberize(thisstring)'></a>

## InflectorExtensions\.Kebaberize\(this string\) Method

Separates the input words with hyphens and all the words are converted to lowercase

```csharp
public static string Kebaberize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Kebaberize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring)'></a>

## InflectorExtensions\.Pascalize\(this string\) Method

By default, pascalize converts strings to UpperCamelCase also removing underscores

```csharp
public static string Pascalize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool)'></a>

## InflectorExtensions\.Pluralize\(this string, bool\) Method

Pluralizes the provided input considering irregular words

```csharp
public static string Pluralize(this string word, bool inputIsKnownToBeSingular=true);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Word to be pluralized

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool).inputIsKnownToBeSingular'></a>

`inputIsKnownToBeSingular` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Normally you call Pluralize on singular words; but if you're unsure call it with false

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool)'></a>

## InflectorExtensions\.Singularize\(this string, bool, bool\) Method

Singularizes the provided input considering irregular words

```csharp
public static string Singularize(this string word, bool inputIsKnownToBePlural=true, bool skipSimpleWords=false);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Word to be singularized

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).inputIsKnownToBePlural'></a>

`inputIsKnownToBePlural` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Normally you call Singularize on plural words; but if you're unsure call it with false

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).skipSimpleWords'></a>

`skipSimpleWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Skip singularizing single words that have an 's' on the end

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Titleize(thisstring)'></a>

## InflectorExtensions\.Titleize\(this string\) Method

Humanizes the input with Title casing

```csharp
public static string Titleize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Titleize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be titleized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.InflectorExtensions.Underscore(thisstring)'></a>

## InflectorExtensions\.Underscore\(this string\) Method

Separates the input words with underscore

```csharp
public static string Underscore(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Underscore(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be underscored

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')