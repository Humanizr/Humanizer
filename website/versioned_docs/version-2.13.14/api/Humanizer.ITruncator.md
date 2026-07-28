## ITruncator Interface

Can truncate a string\.

```csharp
public interface ITruncator
```
### Methods

<a name='Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom)'></a>

## ITruncator\.Truncate\(string, int, string, TruncateFrom\) Method

Truncate a string

```csharp
string Truncate(string value, int length, string truncationString, Humanizer.TruncateFrom truncateFrom=Humanizer.TruncateFrom.Right);
```
#### Parameters

<a name='Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to truncate

<a name='Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The length to truncate to

<a name='Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom).truncationString'></a>

`truncationString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string used to truncate with

<a name='Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom).truncateFrom'></a>

`truncateFrom` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

The enum value used to determine from where to truncate the string

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string