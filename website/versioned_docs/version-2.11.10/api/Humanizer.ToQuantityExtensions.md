## ToQuantityExtensions Class

Provides extensions for formatting a [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String') word as a quantity\.

```csharp
public static class ToQuantityExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ToQuantityExtensions
### Methods

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, double\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, double quantity);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double).quantity'></a>

`quantity` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The quantity of the word

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\.2\) =\> "0\.2 requests"

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double,string,System.IFormatProvider)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, double, string, IFormatProvider\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, double quantity, string format=null, System.IFormatProvider formatProvider=null);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double,string,System.IFormatProvider).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double,string,System.IFormatProvider).quantity'></a>

`quantity` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The quantity of the word

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

A standard or custom numeric format string\.

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,double,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

An object that supplies culture\-specific formatting information\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\.2\) =\> "0\.2 requests"
"request"\.ToQuantity\(10\.6, format: "N0"\) =\> "10\.6 requests"
"request"\.ToQuantity\(1\.0, format: "N0"\) =\> "1 request"

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,Humanizer.ShowQuantityAs)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, int, ShowQuantityAs\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, int quantity, Humanizer.ShowQuantityAs showQuantityAs=Humanizer.ShowQuantityAs.Numeric);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,Humanizer.ShowQuantityAs).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,Humanizer.ShowQuantityAs).quantity'></a>

`quantity` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The quantity of the word

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,Humanizer.ShowQuantityAs).showQuantityAs'></a>

`showQuantityAs` [ShowQuantityAs](Humanizer.ShowQuantityAs.md 'Humanizer\.ShowQuantityAs')

How to show the quantity\. Numeric by default

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\) =\> "0 requests"
"request"\.ToQuantity\(1\) =\> "1 request"
"request"\.ToQuantity\(2\) =\> "2 requests"
"men"\.ToQuantity\(2\) =\> "2 men"
"process"\.ToQuantity\(1200, ShowQuantityAs\.Words\) =\> "one thousand two hundred processes"

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,string,System.IFormatProvider)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, int, string, IFormatProvider\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, int quantity, string format, System.IFormatProvider formatProvider=null);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,string,System.IFormatProvider).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,string,System.IFormatProvider).quantity'></a>

`quantity` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The quantity of the word

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

A standard or custom numeric format string\.

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,int,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

An object that supplies culture\-specific formatting information\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\) =\> "0 requests"
"request"\.ToQuantity\(10000, format: "N0"\) =\> "10,000 requests"
"request"\.ToQuantity\(1, format: "N0"\) =\> "1 request"

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,Humanizer.ShowQuantityAs)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, long, ShowQuantityAs\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, long quantity, Humanizer.ShowQuantityAs showQuantityAs=Humanizer.ShowQuantityAs.Numeric);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,Humanizer.ShowQuantityAs).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,Humanizer.ShowQuantityAs).quantity'></a>

`quantity` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The quantity of the word

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,Humanizer.ShowQuantityAs).showQuantityAs'></a>

`showQuantityAs` [ShowQuantityAs](Humanizer.ShowQuantityAs.md 'Humanizer\.ShowQuantityAs')

How to show the quantity\. Numeric by default

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\) =\> "0 requests"
"request"\.ToQuantity\(1\) =\> "1 request"
"request"\.ToQuantity\(2\) =\> "2 requests"
"men"\.ToQuantity\(2\) =\> "2 men"
"process"\.ToQuantity\(1200, ShowQuantityAs\.Words\) =\> "one thousand two hundred processes"

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,string,System.IFormatProvider)'></a>

## ToQuantityExtensions\.ToQuantity\(this string, long, string, IFormatProvider\) Method

Prefixes the provided word with the number and accordingly pluralizes or singularizes the word

```csharp
public static string ToQuantity(this string input, long quantity, string format, System.IFormatProvider formatProvider=null);
```
#### Parameters

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,string,System.IFormatProvider).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The word to be prefixed

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,string,System.IFormatProvider).quantity'></a>

`quantity` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The quantity of the word

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

A standard or custom numeric format string\.

<a name='Humanizer.ToQuantityExtensions.ToQuantity(thisstring,long,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

An object that supplies culture\-specific formatting information\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"request"\.ToQuantity\(0\) =\> "0 requests"
"request"\.ToQuantity\(10000, format: "N0"\) =\> "10,000 requests"
"request"\.ToQuantity\(1, format: "N0"\) =\> "1 request"