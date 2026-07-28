## IDateOnlyToOrdinalWordConverter Interface

The interface used to localise the ToOrdinalWords method\.

```csharp
public interface IDateOnlyToOrdinalWordConverter
```
### Methods

<a name='Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly)'></a>

## IDateOnlyToOrdinalWordConverter\.Convert\(DateOnly\) Method

Converts the date to Ordinal Words

```csharp
string Convert(System.DateOnly date);
```
#### Parameters

<a name='Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly).date'></a>

`date` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase)'></a>

## IDateOnlyToOrdinalWordConverter\.Convert\(DateOnly, GrammaticalCase\) Method

Converts the date to Ordinal Words using the provided grammatical case

```csharp
string Convert(System.DateOnly date, Humanizer.GrammaticalCase grammaticalCase);
```
#### Parameters

<a name='Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase).date'></a>

`date` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

<a name='Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.Convert(System.DateOnly,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')