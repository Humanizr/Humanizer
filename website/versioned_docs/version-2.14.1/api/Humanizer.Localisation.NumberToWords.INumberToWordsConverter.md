## INumberToWordsConverter Interface

An interface you should implement to localise ToWords and ToOrdinalWords methods

```csharp
public interface INumberToWordsConverter
```
### Methods

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long)'></a>

## INumberToWordsConverter\.Convert\(long\) Method

Converts the number to string using the locale's default grammatical gender

```csharp
string Convert(long number);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool)'></a>

## INumberToWordsConverter\.Convert\(long, bool\) Method

Converts the number to string using the locale's default grammatical gender with or without adding 'And'

```csharp
string Convert(long number, bool addAnd);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Specify with our without adding "And"

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.Convert\(long, bool, WordForm\) Method

Converts the number to a specific string form using the locale's default grammatical gender with or without adding 'And'

```csharp
string Convert(long number, bool addAnd, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool)'></a>

## INumberToWordsConverter\.Convert\(long, GrammaticalGender, bool\) Method

Converts the number to string using the provided grammatical gender

```csharp
string Convert(long number, Humanizer.GrammaticalGender gender, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.Convert\(long, WordForm\) Method

Converts the number to a specific string form using the locale's default grammatical gender\.

```csharp
string Convert(long number, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool)'></a>

## INumberToWordsConverter\.Convert\(long, WordForm, GrammaticalGender, bool\) Method

Converts the number to a specific string form using the provided grammatical gender\.

```csharp
string Convert(long number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int\) Method

Converts the number to ordinal string using the locale's default grammatical gender

```csharp
string ConvertToOrdinal(int number);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, GrammaticalGender\) Method

Converts the number to ordinal string using the provided grammatical gender

```csharp
string ConvertToOrdinal(int number, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, GrammaticalGender, WordForm\) Method

Converts the number to a specific ordinal string form using the provided grammatical gender\.

```csharp
string ConvertToOrdinal(int number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, WordForm\) Method

Converts the number to a specific ordinal string form using the locale's default grammatical gender\.

```csharp
string ConvertToOrdinal(int number, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToTuple(int)'></a>

## INumberToWordsConverter\.ConvertToTuple\(int\) Method

Converts integer to named tuple \(e\.g\. 'single', 'double' etc\.\)\.

```csharp
string ConvertToTuple(int number);
```
#### Parameters

<a name='Humanizer.Localisation.NumberToWords.INumberToWordsConverter.ConvertToTuple(int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')