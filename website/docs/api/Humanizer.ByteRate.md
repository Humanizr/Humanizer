## ByteRate Class

Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer

```csharp
public class ByteRate : System.IComparable<Humanizer.ByteRate>, System.IEquatable<Humanizer.ByteRate>, System.IComparable
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ByteRate

Implements [System\.IComparable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1')[ByteRate](Humanizer.ByteRate.md 'Humanizer\.ByteRate')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ByteRate](Humanizer.ByteRate.md 'Humanizer\.ByteRate')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable 'System\.IComparable')

### Remarks
Create a ByteRate with given quantity of bytes across an interval
### Constructors

<a name='Humanizer.ByteRate.ByteRate(Humanizer.ByteSize,System.TimeSpan)'></a>

## ByteRate\(ByteSize, TimeSpan\) Constructor

Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer

```csharp
public ByteRate(Humanizer.ByteSize size, System.TimeSpan interval);
```
#### Parameters

<a name='Humanizer.ByteRate.ByteRate(Humanizer.ByteSize,System.TimeSpan).size'></a>

`size` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteRate.ByteRate(Humanizer.ByteSize,System.TimeSpan).interval'></a>

`interval` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

### Remarks
Create a ByteRate with given quantity of bytes across an interval
### Properties

<a name='Humanizer.ByteRate.Interval'></a>

## ByteRate\.Interval Property

Interval that bytes were transferred in

```csharp
public System.TimeSpan Interval { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.ByteRate.Size'></a>

## ByteRate\.Size Property

Quantity of bytes

```csharp
public Humanizer.ByteSize Size { get; }
```

#### Property Value
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')
### Methods

<a name='Humanizer.ByteRate.CompareTo(Humanizer.ByteRate)'></a>

## ByteRate\.CompareTo\(ByteRate\) Method

Compares this rate with another rate after normalizing both to bytes per second\.

```csharp
public int CompareTo(Humanizer.ByteRate? other);
```
#### Parameters

<a name='Humanizer.ByteRate.CompareTo(Humanizer.ByteRate).other'></a>

`other` [ByteRate](Humanizer.ByteRate.md 'Humanizer\.ByteRate')

The rate to compare with\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteRate.CompareTo(object)'></a>

## ByteRate\.CompareTo\(object\) Method

```csharp
public int CompareTo(object? obj);
```
#### Parameters

<a name='Humanizer.ByteRate.CompareTo(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

Implements [CompareTo\(object\)](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable.compareto#system-icomparable-compareto(system-object) 'System\.IComparable\.CompareTo\(System\.Object\)')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteRate.Equals(Humanizer.ByteRate)'></a>

## ByteRate\.Equals\(ByteRate\) Method

```csharp
public bool Equals(Humanizer.ByteRate? other);
```
#### Parameters

<a name='Humanizer.ByteRate.Equals(Humanizer.ByteRate).other'></a>

`other` [ByteRate](Humanizer.ByteRate.md 'Humanizer\.ByteRate')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteRate.Equals(object)'></a>

## ByteRate\.Equals\(object\) Method

```csharp
public override bool Equals(object? obj);
```
#### Parameters

<a name='Humanizer.ByteRate.Equals(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteRate.GetHashCode()'></a>

## ByteRate\.GetHashCode\(\) Method

```csharp
public override int GetHashCode();
```

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteRate.Humanize(Humanizer.TimeUnit)'></a>

## ByteRate\.Humanize\(TimeUnit\) Method

Calculate rate for the quantity of bytes and interval defined by this instance

```csharp
public string Humanize(Humanizer.TimeUnit timeUnit=Humanizer.TimeUnit.Second);
```
#### Parameters

<a name='Humanizer.ByteRate.Humanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Unit of time to calculate rate for \(defaults is per second\)

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteRate.Humanize(string,Humanizer.TimeUnit,System.Globalization.CultureInfo)'></a>

## ByteRate\.Humanize\(string, TimeUnit, CultureInfo\) Method

Calculate rate for the quantity of bytes and interval defined by this instance

```csharp
public string Humanize(string? format, Humanizer.TimeUnit timeUnit=Humanizer.TimeUnit.Second, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.ByteRate.Humanize(string,Humanizer.TimeUnit,System.Globalization.CultureInfo).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string format to use for the number of bytes

<a name='Humanizer.ByteRate.Humanize(string,Humanizer.TimeUnit,System.Globalization.CultureInfo).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Unit of time to calculate rate for \(defaults is per second\)

<a name='Humanizer.ByteRate.Humanize(string,Humanizer.TimeUnit,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteRate.ToString()'></a>

## ByteRate\.ToString\(\) Method

Returns the humanized rate using the default format and time unit\.

```csharp
public override string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')