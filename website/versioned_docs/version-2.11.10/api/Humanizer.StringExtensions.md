## StringExtensions Class

Extension methods for String type\.

```csharp
public static class StringExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → StringExtensions
### Methods

<a name='Humanizer.StringExtensions.FormatWith(thisstring,object[])'></a>

## StringExtensions\.FormatWith\(this string, object\[\]\) Method

Extension method to format string with passed arguments\. Current thread's current culture is used

```csharp
public static string FormatWith(this string format, params object[] args);
```
#### Parameters

<a name='Humanizer.StringExtensions.FormatWith(thisstring,object[]).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

string format

<a name='Humanizer.StringExtensions.FormatWith(thisstring,object[]).args'></a>

`args` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

arguments

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.StringExtensions.FormatWith(thisstring,System.IFormatProvider,object[])'></a>

## StringExtensions\.FormatWith\(this string, IFormatProvider, object\[\]\) Method

Extension method to format string with passed arguments using specified format provider \(i\.e\. CultureInfo\)

```csharp
public static string FormatWith(this string format, System.IFormatProvider provider, params object[] args);
```
#### Parameters

<a name='Humanizer.StringExtensions.FormatWith(thisstring,System.IFormatProvider,object[]).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

string format

<a name='Humanizer.StringExtensions.FormatWith(thisstring,System.IFormatProvider,object[]).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

An object that supplies culture\-specific formatting information

<a name='Humanizer.StringExtensions.FormatWith(thisstring,System.IFormatProvider,object[]).args'></a>

`args` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

arguments

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')