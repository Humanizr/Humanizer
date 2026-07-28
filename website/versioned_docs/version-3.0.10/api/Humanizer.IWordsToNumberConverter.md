## IWordsToNumberConverter Interface

```csharp
public interface IWordsToNumberConverter
```
### Methods

<a name='Humanizer.IWordsToNumberConverter.Convert(string)'></a>

## IWordsToNumberConverter\.Convert\(string\) Method

```csharp
int Convert(string words);
```
#### Parameters

<a name='Humanizer.IWordsToNumberConverter.Convert(string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int)'></a>

## IWordsToNumberConverter\.TryConvert\(string, int\) Method

```csharp
bool TryConvert(string words, out int parsedValue);
```
#### Parameters

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int).parsedValue'></a>

`parsedValue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int,string)'></a>

## IWordsToNumberConverter\.TryConvert\(string, int, string\) Method

```csharp
bool TryConvert(string words, out int parsedValue, out string? unrecognizedNumber);
```
#### Parameters

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int,string).parsedValue'></a>

`parsedValue` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IWordsToNumberConverter.TryConvert(string,int,string).unrecognizedNumber'></a>

`unrecognizedNumber` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')