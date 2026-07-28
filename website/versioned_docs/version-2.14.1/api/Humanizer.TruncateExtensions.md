## TruncateExtensions Class

Allow strings to be truncated

```csharp
public static class TruncateExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TruncateExtensions
### Methods

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int)'></a>

## TruncateExtensions\.Truncate\(this string, int\) Method

Truncate the string

```csharp
public static string Truncate(this string input, int length);
```
#### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The length to truncate to

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom)'></a>

## TruncateExtensions\.Truncate\(this string, int, ITruncator, TruncateFrom\) Method

Truncate the string

```csharp
public static string Truncate(this string input, int length, Humanizer.ITruncator truncator, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
#### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The length to truncate to

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator'></a>

`truncator` [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')

The truncate to use

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

The enum value used to determine from where to truncate the string

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom)'></a>

## TruncateExtensions\.Truncate\(this string, int, string, ITruncator, TruncateFrom\) Method

Truncate the string

```csharp
public static string Truncate(this string input, int length, string truncationString, Humanizer.ITruncator truncator, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
#### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The length to truncate to

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).truncationString'></a>

`truncationString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string used to truncate with

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator'></a>

`truncator` [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')

The truncator to use

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

The enum value used to determine from where to truncate the string

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom)'></a>

## TruncateExtensions\.Truncate\(this string, int, string, TruncateFrom\) Method

Truncate the string

```csharp
public static string Truncate(this string input, int length, string truncationString, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
#### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The length to truncate to

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).truncationString'></a>

`truncationString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string used to truncate with

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

The enum value used to determine from where to truncate the string

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string