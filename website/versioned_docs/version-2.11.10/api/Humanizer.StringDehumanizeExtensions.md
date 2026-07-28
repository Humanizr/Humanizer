## StringDehumanizeExtensions Class

Contains extension methods for dehumanizing strings\.

```csharp
public static class StringDehumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → StringDehumanizeExtensions
### Methods

<a name='Humanizer.StringDehumanizeExtensions.Dehumanize(thisstring)'></a>

## StringDehumanizeExtensions\.Dehumanize\(this string\) Method

Dehumanizes a string; e\.g\. 'some string', 'Some String', 'Some string' \-\> 'SomeString'

```csharp
public static string Dehumanize(this string input);
```
#### Parameters

<a name='Humanizer.StringDehumanizeExtensions.Dehumanize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be dehumanized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')