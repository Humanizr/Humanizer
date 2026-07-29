## InflectorExtensions Class

```csharp
public static class InflectorExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → InflectorExtensions
### Methods

<a name='Humanizer.InflectorExtensions.Camelize(thisstring)'></a>

## InflectorExtensions\.Camelize\(this string\) Method

Converts a string to camelCase \(lowerCamelCase\) by preserving leading underscores, capitalizing
the first letter of each word except the first word, and removing other spaces, underscores, dashes, and dots\.

```csharp
public static string Camelize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Camelize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be camelized\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A camelCase version of the input where leading underscores are preserved, the first word starts
with a lowercase letter, subsequent words start with uppercase letters, and other separators are removed\.

### Example

```csharp
"some_property_name".Camelize() => "somePropertyName"
"some property name".Camelize() => "somePropertyName"
"some.property.name".Camelize() => "somePropertyName"
"SomePropertyName".Camelize() => "somePropertyName"
"_some_property_name".Camelize() => "_somePropertyName"
```

### Remarks
camelCase is the same as PascalCase except any leading underscores are preserved and the first
character after them is lowercase\.
It's commonly used for variable and method parameter names in \.NET\.
Casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.Dasherize(thisstring)'></a>

## InflectorExtensions\.Dasherize\(this string\) Method

Replaces all underscores in the string with dashes \(hyphens\)\.

```csharp
public static string Dasherize(this string underscoredWord);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Dasherize(thisstring).underscoredWord'></a>

`underscoredWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string containing underscores to be replaced with dashes\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string with all underscores replaced by dashes\.

### Example

```csharp
"some_property_name".Dasherize() => "some-property-name"
"some_longer_property_name".Dasherize() => "some-longer-property-name"
```

### Remarks
This is typically used after calling [Underscore\(this string\)](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.Underscore(thisstring) 'Humanizer\.InflectorExtensions\.Underscore\(this string\)') to convert from underscore\_case to dash\-case \(kebab\-case\)\.

<a name='Humanizer.InflectorExtensions.Hyphenate(thisstring)'></a>

## InflectorExtensions\.Hyphenate\(this string\) Method

Replaces all underscores in the string with hyphens\. This is an alias for [Dasherize\(this string\)](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.Dasherize(thisstring) 'Humanizer\.InflectorExtensions\.Dasherize\(this string\)')\.

```csharp
public static string Hyphenate(this string underscoredWord);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Hyphenate(thisstring).underscoredWord'></a>

`underscoredWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string containing underscores to be replaced with hyphens\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string with all underscores replaced by hyphens\.

### Example

```csharp
"some_property_name".Hyphenate() => "some-property-name"
```

### Remarks
This method is functionally identical to [Dasherize\(this string\)](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.Dasherize(thisstring) 'Humanizer\.InflectorExtensions\.Dasherize\(this string\)') and is provided for API clarity\.

<a name='Humanizer.InflectorExtensions.Kebaberize(thisstring)'></a>

## InflectorExtensions\.Kebaberize\(this string\) Method

Converts a string to kebab\-case \(lowercase words separated by hyphens\), transforming
PascalCase, camelCase, spaces, and underscores into hyphenated lowercase text\.

```csharp
public static string Kebaberize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Kebaberize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be converted to kebab\-case\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A lowercase string with words separated by hyphens\.

### Example

```csharp
"SomePropertyName".Kebaberize() => "some-property-name"
"somePropertyName".Kebaberize() => "some-property-name"
"some property name".Kebaberize() => "some-property-name"
"some_property_name".Kebaberize() => "some-property-name"
```

### Remarks
Kebab\-case is commonly used for CSS class names, HTML attributes, and URL slugs\.
This is equivalent to calling [Underscore\(this string\)](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.Underscore(thisstring) 'Humanizer\.InflectorExtensions\.Underscore\(this string\)') followed by [Dasherize\(this string\)](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.Dasherize(thisstring) 'Humanizer\.InflectorExtensions\.Dasherize\(this string\)')\.
Casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring)'></a>

## InflectorExtensions\.Pascalize\(this string\) Method

Converts a string to PascalCase \(UpperCamelCase\) by capitalizing the first letter of each word
and removing spaces, underscores, dashes, and dots\.

```csharp
public static string Pascalize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be pascalized\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A PascalCase version of the input where each word starts with an uppercase letter and 
spaces, underscores, dashes, and dots are removed\.

### Example

```csharp
"some_property_name".Pascalize() => "SomePropertyName"
"some property name".Pascalize() => "SomePropertyName"
"some-property-name".Pascalize() => "SomePropertyName"
"some.property.name".Pascalize() => "SomePropertyName"
```

### Remarks
PascalCase \(also known as UpperCamelCase\) is commonly used for class names and type names in \.NET\.
Casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring,bool)'></a>

## InflectorExtensions\.Pascalize\(this string, bool\) Method

Converts a string to PascalCase \(UpperCamelCase\), optionally normalizing uppercase sequences as words\.

```csharp
public static string Pascalize(this string input, bool preserveUppercase);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring,bool).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be pascalized\. Must not be null\.

<a name='Humanizer.InflectorExtensions.Pascalize(thisstring,bool).preserveUppercase'></a>

`preserveUppercase` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to preserve uppercase sequences; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to normalize them as words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A PascalCase version of the input\.

### Example

```csharp
"SMS parameter provider".Pascalize(preserveUppercase: true) => "SMSParameterProvider"
"HTTP IO module".Pascalize(preserveUppercase: false) => "HttpIoModule"
```

### Remarks
Uppercase sequences are split using identifier word boundaries, and casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool)'></a>

## InflectorExtensions\.Pluralize\(this string, bool\) Method

Converts a singular word to its plural form, handling both regular and irregular pluralizations\.

```csharp
public static string? Pluralize(this string? word, bool inputIsKnownToBeSingular=true);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be pluralized\. Can be null\.

<a name='Humanizer.InflectorExtensions.Pluralize(thisstring,bool).inputIsKnownToBeSingular'></a>

`inputIsKnownToBeSingular` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the input is known to be in singular form\. 
Set to true \(default\) if you're certain the word is singular\.
Set to false if the word might already be plural, in which case the method will check and avoid double\-pluralization\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The plural form of the word, or null if the input was null\.
Handles irregular plurals \(e\.g\., "person" → "people", "child" → "children"\) and regular plurals \(e\.g\., "cat" → "cats"\)\.

### Example

```csharp
"person".Pluralize() => "people"
"cat".Pluralize() => "cats"
"box".Pluralize() => "boxes"
"man".Pluralize() => "men"
"meter per second".Pluralize() => "meters per second"
"PERSON".Pluralize() => "PEOPLE"
"cats".Pluralize(inputIsKnownToBeSingular: false) => "cats" (avoids double pluralization)
```

### Remarks
Uses the default vocabulary which includes English pluralization rules and common irregular forms\.
In compound rates separated by the word "per", the numerator is pluralized and the denominator is preserved\.

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool)'></a>

## InflectorExtensions\.Singularize\(this string, bool, bool\) Method

Converts a plural word to its singular form, handling both regular and irregular singularizations\.

```csharp
public static string Singularize(this string word, bool inputIsKnownToBePlural=true, bool skipSimpleWords=false);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be singularized\. Must not be null\.

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).inputIsKnownToBePlural'></a>

`inputIsKnownToBePlural` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the input is known to be in plural form\.
Set to true \(default\) if you're certain the word is plural\.
Set to false if the word might already be singular, in which case the method will check and avoid incorrect singularization\.

<a name='Humanizer.InflectorExtensions.Singularize(thisstring,bool,bool).skipSimpleWords'></a>

`skipSimpleWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When true, skips singularization of simple words that just end in 's'\.
This helps avoid incorrectly singularizing words like "ross" to "ros"\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The singular form of the word\.
Handles irregular singulars \(e\.g\., "people" → "person", "children" → "child"\) and regular singulars \(e\.g\., "cats" → "cat"\)\.

### Example

```csharp
"people".Singularize() => "person"
"cats".Singularize() => "cat"
"boxes".Singularize() => "box"
"men".Singularize() => "man"
"meters per second".Singularize() => "meter per second"
"PEOPLE".Singularize() => "PERSON"
"person".Singularize(inputIsKnownToBePlural: false) => "person" (avoids incorrect singularization)
```

### Remarks
Uses the default vocabulary which includes English singularization rules and common irregular forms\.
In compound rates separated by the word "per", the numerator is singularized and the denominator is preserved\.

<a name='Humanizer.InflectorExtensions.Titleize(thisstring)'></a>

## InflectorExtensions\.Titleize\(this string\) Method

Converts a string to title case by humanizing it first and then applying title casing\.
Each word in the result will start with an uppercase letter\.

```csharp
public static string Titleize(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Titleize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be converted to title case\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A humanized string with each word capitalized \(title case\)\.
If humanization produces an empty string, returns the original input unchanged\.

### Example

```csharp
"some_title".Titleize() => "Some Title"
"someTitle".Titleize() => "Some Title"
"some-package_name".Titleize() => "Some Package Name"
```

### Remarks
This method first humanizes the input \(breaking up PascalCase, underscores, etc\.\) and then applies title casing\.

<a name='Humanizer.InflectorExtensions.ToCamelCase(thisstring)'></a>

## InflectorExtensions\.ToCamelCase\(this string\) Method

Converts a string to camelCase while preserving leading underscores and normalizing uppercase sequences as words\.

```csharp
public static string ToCamelCase(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.ToCamelCase(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be converted\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A camelCase version of the input with leading underscores preserved and uppercase sequences normalized\.

### Example

```csharp
"IOModule".ToCamelCase() => "ioModule"
"__XMLHttpRequest".ToCamelCase() => "__xmlHttpRequest"
```

### Remarks
Uppercase sequences are split using identifier word boundaries, and casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool)'></a>

## InflectorExtensions\.ToPossessive\(this string, bool, bool\) Method

Converts an English noun or noun phrase to its possessive form\.

```csharp
public static string? ToPossessive(this string? word, bool inputIsPlural=false, bool useApostropheOnlyForSingularWordsEndingInS=false);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The noun or noun phrase to convert\.

<a name='Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool).inputIsPlural'></a>

`inputIsPlural` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether [word](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool).word 'Humanizer\.InflectorExtensions\.ToPossessive\(this string, bool, bool\)\.word') is plural\.

<a name='Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool).useApostropheOnlyForSingularWordsEndingInS'></a>

`useApostropheOnlyForSingularWordsEndingInS` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether singular words ending in `s` should use only an apostrophe instead of `'s`\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The possessive form of [word](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.ToPossessive(thisstring,bool,bool).word 'Humanizer\.InflectorExtensions\.ToPossessive\(this string, bool, bool\)\.word')\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string)'></a>

## InflectorExtensions\.TryInflect\(this CardinalInflectionForms, decimal, CultureInfo, string\) Method

Attempts to inflect an authored set of cardinal noun forms for a quantity and culture\.

```csharp
public static bool TryInflect(this Humanizer.CardinalInflectionForms forms, decimal quantity, System.Globalization.CultureInfo culture, out string? result);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).forms'></a>

`forms` [CardinalInflectionForms](Humanizer.CardinalInflectionForms.md 'Humanizer\.CardinalInflectionForms')

The authored cardinal forms\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).quantity'></a>

`quantity` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The cardinal quantity\. Its encoded decimal scale supplies CLDR visible\-fraction operands\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture whose cardinal plural rules are applied\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).result'></a>

`result` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The selected authored form when available; otherwise, [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when Humanizer supports [culture](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).culture 'Humanizer\.InflectorExtensions\.TryInflect\(this Humanizer\.CardinalInflectionForms, decimal, System\.Globalization\.CultureInfo, string\)\.culture') and the selected
            category has an explicitly authored form; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[forms](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).forms 'Humanizer\.InflectorExtensions\.TryInflect\(this Humanizer\.CardinalInflectionForms, decimal, System\.Globalization\.CultureInfo, string\)\.forms') or [culture](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryInflect(thisHumanizer.CardinalInflectionForms,decimal,System.Globalization.CultureInfo,string).culture 'Humanizer\.InflectorExtensions\.TryInflect\(this Humanizer\.CardinalInflectionForms, decimal, System\.Globalization\.CultureInfo, string\)\.culture') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks
Missing category forms are not inferred\. Use [Invariant\(string\)](Humanizer.CardinalInflectionForms.md#Humanizer.CardinalInflectionForms.Invariant(string) 'Humanizer\.CardinalInflectionForms\.Invariant\(string\)')
for an invariant noun, or explicitly supply equal forms when categories share spelling\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string)'></a>

## InflectorExtensions\.TryInflect\(this string, decimal, CultureInfo, string\) Method

Attempts to inflect a localized citation\-form noun from Humanizer's built\-in exact lexicon\.

```csharp
public static bool TryInflect(this string lemma, decimal quantity, System.Globalization.CultureInfo culture, out string? result);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).lemma'></a>

`lemma` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

A single common noun in its localized citation form\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).quantity'></a>

`quantity` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The cardinal quantity\. Its encoded decimal scale supplies CLDR visible\-fraction operands\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture whose exact lexicon and cardinal rules are applied\.

<a name='Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).result'></a>

`result` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The selected authored form when available; otherwise, [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') for a known exact lexeme and form; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[lemma](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).lemma 'Humanizer\.InflectorExtensions\.TryInflect\(this string, decimal, System\.Globalization\.CultureInfo, string\)\.lemma') or [culture](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryInflect(thisstring,decimal,System.Globalization.CultureInfo,string).culture 'Humanizer\.InflectorExtensions\.TryInflect\(this string, decimal, System\.Globalization\.CultureInfo, string\)\.culture') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks
Matching is Unicode NFC\-normalized, ordinal, and case\-sensitive\. Output preserves authored
casing\. This method never falls back to English and never guesses unknown noun morphology\.

<a name='Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string)'></a>

## InflectorExtensions\.TryLemmatize\(this string, CultureInfo, string\) Method

Attempts to resolve a localized cardinal noun form to its citation form\.

```csharp
public static bool TryLemmatize(this string form, System.Globalization.CultureInfo culture, out string? lemma);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string).form'></a>

`form` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An exact form from Humanizer's built\-in localized lexicon\.

<a name='Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture whose exact lexicon is searched\.

<a name='Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string).lemma'></a>

`lemma` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique citation form in the selected lexicon; otherwise, [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the selected culture's authored reverse index identifies exactly
            one citation form; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[form](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string).form 'Humanizer\.InflectorExtensions\.TryLemmatize\(this string, System\.Globalization\.CultureInfo, string\)\.form') or [culture](Humanizer.InflectorExtensions.md#Humanizer.InflectorExtensions.TryLemmatize(thisstring,System.Globalization.CultureInfo,string).culture 'Humanizer\.InflectorExtensions\.TryLemmatize\(this string, System\.Globalization\.CultureInfo, string\)\.culture') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks
Matching is Unicode NFC\-normalized, ordinal, and case\-sensitive\. The returned lemma preserves
authored casing\. This method is catalog\-bounded: it never reverses spelling rules or claims
uniqueness across the complete natural language\.

<a name='Humanizer.InflectorExtensions.Underscore(thisstring)'></a>

## InflectorExtensions\.Underscore\(this string\) Method

Converts a string to lowercase and separates words with underscores, transforming 
PascalCase, camelCase, and spaces into underscore\_case\.

```csharp
public static string Underscore(this string input);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Underscore(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be underscored\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A lowercase string with words separated by underscores instead of spaces, case changes, or dashes\.

### Example

```csharp
"SomePropertyName".Underscore() => "some_property_name"
"somePropertyName".Underscore() => "some_property_name"
"some-property-name".Underscore() => "some_property_name"
"some property name".Underscore() => "some_property_name"
```

### Remarks
This transformation is commonly used for database column names, file names, and URL slugs in some conventions\.
Casing is culture\-invariant\.

<a name='Humanizer.InflectorExtensions.Underscore(thisstring,bool)'></a>

## InflectorExtensions\.Underscore\(this string, bool\) Method

Separates words with underscores, optionally preserving the input casing\.

```csharp
public static string Underscore(this string input, bool preserveCase);
```
#### Parameters

<a name='Humanizer.InflectorExtensions.Underscore(thisstring,bool).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be underscored\. Must not be null\.

<a name='Humanizer.InflectorExtensions.Underscore(thisstring,bool).preserveCase'></a>

`preserveCase` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to preserve the input casing; [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') to convert the result to lowercase\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string with words separated by underscores\.

### Example

```csharp
"SomePropertyName".Underscore(preserveCase: true) => "Some_Property_Name"
"HTMLParser".Underscore(preserveCase: true) => "HTML_Parser"
```

### Remarks
Acronyms are split using identifier word boundaries, and lowercasing is culture\-invariant\.