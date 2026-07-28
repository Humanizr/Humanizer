## IOrdinalizer Interface

The interface used to localise the Ordinalize method

```csharp
public interface IOrdinalizer
```
### Methods

<a name='Humanizer.IOrdinalizer.Convert(int,string)'></a>

## IOrdinalizer\.Convert\(int, string\) Method

Ordinalizes the number

```csharp
string Convert(int number, string numberString);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IOrdinalizer.Convert(int,string).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender)'></a>

## IOrdinalizer\.Convert\(int, string, GrammaticalGender\) Method

Ordinalizes the number using the provided grammatical gender

```csharp
string Convert(int number, string numberString, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## IOrdinalizer\.Convert\(int, string, GrammaticalGender, WordForm\) Method

Ordinalizes the number to a locale's specific form using the provided grammatical gender\.

```csharp
string Convert(int number, string numberString, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm)'></a>

## IOrdinalizer\.Convert\(int, string, WordForm\) Method

Ordinalizes the number to a locale's specific form\.

```csharp
string Convert(int number, string numberString, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')