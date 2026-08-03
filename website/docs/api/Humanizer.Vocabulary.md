---
title: 'Humanizer.Vocabulary'
sidebar_label: 'Humanizer.Vocabulary'
description: 'API reference for Humanizer.Vocabulary.'
---
## Vocabulary Class

A container for custom acronym casing and exceptions to simple pluralization/singularization rules\.
Vocabularies\.Default contains an extensive list of rules for US English and supports process\-wide acronym registration\.
At this time, multiple vocabularies and removing existing rules are not supported\.

```csharp
public class Vocabulary
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Vocabulary
- *Methods*
  - **[AddAcronym\(string\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddAcronym(string) 'Humanizer\.Vocabulary\.AddAcronym\(string\)')**
  - **[AddIrregular\(string, string, bool\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddIrregular(string,string,bool) 'Humanizer\.Vocabulary\.AddIrregular\(string, string, bool\)')**
  - **[AddPlural\(string, string\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddPlural(string,string) 'Humanizer\.Vocabulary\.AddPlural\(string, string\)')**
  - **[AddSingular\(string, string\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddSingular(string,string) 'Humanizer\.Vocabulary\.AddSingular\(string, string\)')**
  - **[AddUncountable\(string\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddUncountable(string) 'Humanizer\.Vocabulary\.AddUncountable\(string\)')**
  - **[Pluralize\(string, bool\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.Pluralize(string,bool) 'Humanizer\.Vocabulary\.Pluralize\(string, bool\)')**
  - **[Singularize\(string, bool, bool\)](Humanizer.Vocabulary.md#Humanizer.Vocabulary.Singularize(string,bool,bool) 'Humanizer\.Vocabulary\.Singularize\(string, bool, bool\)')**
### Methods

<a name='Humanizer.Vocabulary.AddAcronym(string)'></a>

#### Vocabulary\.AddAcronym\(string\) Method

Adds an acronym whose casing should be preserved when humanizing strings\.

```csharp
public void AddAcronym(string acronym);
```
##### Parameters

<a name='Humanizer.Vocabulary.AddAcronym(string).acronym'></a>

`acronym` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The letters in the acronym's canonical output casing, e\.g\. "HTML"\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[acronym](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddAcronym(string).acronym 'Humanizer\.Vocabulary\.AddAcronym\(string\)\.acronym') is null\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
[acronym](Humanizer.Vocabulary.md#Humanizer.Vocabulary.AddAcronym(string).acronym 'Humanizer\.Vocabulary\.AddAcronym\(string\)\.acronym') is empty or contains a non\-letter\.

<a name='Humanizer.Vocabulary.AddIrregular(string,string,bool)'></a>

#### Vocabulary\.AddIrregular\(string, string, bool\) Method

Adds a word to the vocabulary which cannot easily be pluralized/singularized by RegEx, e\.g\. "person" and "people"\.

```csharp
public void AddIrregular(string singular, string plural, bool matchEnding=true);
```
##### Parameters

<a name='Humanizer.Vocabulary.AddIrregular(string,string,bool).singular'></a>

`singular` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The singular form of the irregular word, e\.g\. "person"\.

<a name='Humanizer.Vocabulary.AddIrregular(string,string,bool).plural'></a>

`plural` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The plural form of the irregular word, e\.g\. "people"\.

<a name='Humanizer.Vocabulary.AddIrregular(string,string,bool).matchEnding'></a>

`matchEnding` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

True to match these words on their own as well as at the end of longer words\. False, otherwise\.

<a name='Humanizer.Vocabulary.AddPlural(string,string)'></a>

#### Vocabulary\.AddPlural\(string, string\) Method

Adds a rule to the vocabulary that does not follow trivial rules for pluralization, e\.g\. "bus" \-\> "buses"

```csharp
public void AddPlural(string rule, string replacement);
```
##### Parameters

<a name='Humanizer.Vocabulary.AddPlural(string,string).rule'></a>

`rule` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

RegEx to be matched, case insensitive, e\.g\. "\(bus\)es$"

<a name='Humanizer.Vocabulary.AddPlural(string,string).replacement'></a>

`replacement` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

RegEx replacement  e\.g\. "$1"

<a name='Humanizer.Vocabulary.AddSingular(string,string)'></a>

#### Vocabulary\.AddSingular\(string, string\) Method

Adds a rule to the vocabulary that does not follow trivial rules for singularization, e\.g\. "vertices/indices \-\> "vertex/index"

```csharp
public void AddSingular(string rule, string replacement);
```
##### Parameters

<a name='Humanizer.Vocabulary.AddSingular(string,string).rule'></a>

`rule` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

RegEx to be matched, case insensitive, e\.g\. ""\(vert\|ind\)ices$""

<a name='Humanizer.Vocabulary.AddSingular(string,string).replacement'></a>

`replacement` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

RegEx replacement  e\.g\. "$1ex"

<a name='Humanizer.Vocabulary.AddUncountable(string)'></a>

#### Vocabulary\.AddUncountable\(string\) Method

Adds an uncountable word to the vocabulary, e\.g\. "fish"\.  Will be ignored when plurality is changed\.

```csharp
public void AddUncountable(string word);
```
##### Parameters

<a name='Humanizer.Vocabulary.AddUncountable(string).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Word to be added to the list of uncountables\.

<a name='Humanizer.Vocabulary.Pluralize(string,bool)'></a>

#### Vocabulary\.Pluralize\(string, bool\) Method

Pluralizes the provided input considering irregular words

```csharp
public string? Pluralize(string? word, bool inputIsKnownToBeSingular=true);
```
##### Parameters

<a name='Humanizer.Vocabulary.Pluralize(string,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Word to be pluralized

<a name='Humanizer.Vocabulary.Pluralize(string,bool).inputIsKnownToBeSingular'></a>

`inputIsKnownToBeSingular` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Normally you call Pluralize on singular words; but if you're unsure call it with false

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Vocabulary.Singularize(string,bool,bool)'></a>

#### Vocabulary\.Singularize\(string, bool, bool\) Method

Singularizes the provided input considering irregular words

```csharp
public string? Singularize(string? word, bool inputIsKnownToBePlural=true, bool skipSimpleWords=false);
```
##### Parameters

<a name='Humanizer.Vocabulary.Singularize(string,bool,bool).word'></a>

`word` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Word to be singularized

<a name='Humanizer.Vocabulary.Singularize(string,bool,bool).inputIsKnownToBePlural'></a>

`inputIsKnownToBePlural` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Normally you call Singularize on plural words; but if you're unsure call it with false

<a name='Humanizer.Vocabulary.Singularize(string,bool,bool).skipSimpleWords'></a>

`skipSimpleWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Skip singularizing single words that have an 's' on the end

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
