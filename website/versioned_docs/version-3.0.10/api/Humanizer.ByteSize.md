## ByteSize Struct

```csharp
public struct ByteSize : System.IComparable<Humanizer.ByteSize>, System.IEquatable<Humanizer.ByteSize>, System.IComparable, System.IFormattable
```

Implements [System\.IComparable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable 'System\.IComparable'), [System\.IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable 'System\.IFormattable')
### Methods

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider)'></a>

## ByteSize\.ToFullWords\(string, IFormatProvider\) Method

Converts the value of the current ByteSize object to a string with
full words\. The metric prefix symbol \(bit, byte, kilo, mega, giga,
tera\) used is the largest metric prefix such that the corresponding
value is greater than or equal to one\.

```csharp
public readonly string ToFullWords(string? format=null, System.IFormatProvider? provider=null);
```
#### Parameters

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString()'></a>

## ByteSize\.ToString\(\) Method

Converts the value of the current ByteSize object to a string\.
The metric prefix symbol \(bit, byte, kilo, mega, giga, tera\) used is
the largest metric prefix such that the corresponding value is greater
 than or equal to one\.

```csharp
public override readonly string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')