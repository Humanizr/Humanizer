## StringDehumanizeExtensions Class

Contains extension methods for dehumanizing strings\.

```csharp
public static class StringDehumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → StringDehumanizeExtensions
### Methods

<a name='Humanizer.StringDehumanizeExtensions.Dehumanize(thisstring)'></a>

## StringDehumanizeExtensions\.Dehumanize\(this string\) Method

Converts a humanized string back to PascalCase format by removing spaces and capitalizing each word\.

```csharp
public static string Dehumanize(this string input);
```
#### Parameters

<a name='Humanizer.StringDehumanizeExtensions.Dehumanize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be dehumanized\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A PascalCase string with all spaces removed and each word capitalized\.
If the input is already in PascalCase \(contains no spaces\), it is returned unchanged\.

### Example

```csharp
"some string".Dehumanize() => "SomeString"
"Some String".Dehumanize() => "SomeString"
"Some string".Dehumanize() => "SomeString"
"SomeStringAndAnotherString".Dehumanize() => "SomeStringAndAnotherString" // Already dehumanized, returned unchanged
```

### Remarks
This method reverses the humanization process by:
1\. Splitting the input on spaces
2\. Humanizing each word \(to handle any edge cases\)
3\. Pascalizing each word \(capitalizing first letter\)
4\. Removing all spaces
This is the inverse operation of [Humanize\(this string\)](Humanizer.StringHumanizeExtensions.md#Humanizer.StringHumanizeExtensions.Humanize(thisstring) 'Humanizer\.StringHumanizeExtensions\.Humanize\(this string\)')\.