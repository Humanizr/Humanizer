## IDateToOrdinalWordConverter Interface

The interface used to localise the ToOrdinalWords method\.

```csharp
public interface IDateToOrdinalWordConverter
```
### Methods

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime)'></a>

## IDateToOrdinalWordConverter\.Convert\(DateTime\) Method

Converts the date to Ordinal Words

```csharp
string Convert(System.DateTime date);
```
#### Parameters

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase)'></a>

## IDateToOrdinalWordConverter\.Convert\(DateTime, GrammaticalCase\) Method

Converts the date to Ordinal Words using the provided grammatical case

```csharp
string Convert(System.DateTime date, Humanizer.GrammaticalCase grammaticalCase);
```
#### Parameters

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.IDateToOrdinalWordConverter.Convert(System.DateTime,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')