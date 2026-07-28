## ByteRate Class

Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer

```csharp
public class ByteRate
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ByteRate
### Constructors

<a name='Humanizer.Bytes.ByteRate.ByteRate(Humanizer.Bytes.ByteSize,System.TimeSpan)'></a>

## ByteRate\(ByteSize, TimeSpan\) Constructor

Create a ByteRate with given quantity of bytes across an interval

```csharp
public ByteRate(Humanizer.Bytes.ByteSize size, System.TimeSpan interval);
```
#### Parameters

<a name='Humanizer.Bytes.ByteRate.ByteRate(Humanizer.Bytes.ByteSize,System.TimeSpan).size'></a>

`size` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteRate.ByteRate(Humanizer.Bytes.ByteSize,System.TimeSpan).interval'></a>

`interval` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')
### Properties

<a name='Humanizer.Bytes.ByteRate.Interval'></a>

## ByteRate\.Interval Property

Interval that bytes were transferred in

```csharp
public System.TimeSpan Interval { get; }
```

#### Property Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.Bytes.ByteRate.Size'></a>

## ByteRate\.Size Property

Quantity of bytes

```csharp
public Humanizer.Bytes.ByteSize Size { get; }
```

#### Property Value
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')
### Methods

<a name='Humanizer.Bytes.ByteRate.Humanize(Humanizer.Localisation.TimeUnit)'></a>

## ByteRate\.Humanize\(TimeUnit\) Method

Calculate rate for the quantity of bytes and interval defined by this instance

```csharp
public string Humanize(Humanizer.Localisation.TimeUnit timeUnit=Humanizer.Localisation.TimeUnit.Second);
```
#### Parameters

<a name='Humanizer.Bytes.ByteRate.Humanize(Humanizer.Localisation.TimeUnit).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

Unit of time to calculate rate for \(defaults is per second\)

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteRate.Humanize(string,Humanizer.Localisation.TimeUnit,System.Globalization.CultureInfo)'></a>

## ByteRate\.Humanize\(string, TimeUnit, CultureInfo\) Method

Calculate rate for the quantity of bytes and interval defined by this instance

```csharp
public string Humanize(string format, Humanizer.Localisation.TimeUnit timeUnit=Humanizer.Localisation.TimeUnit.Second, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.Bytes.ByteRate.Humanize(string,Humanizer.Localisation.TimeUnit,System.Globalization.CultureInfo).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string format to use for the number of bytes

<a name='Humanizer.Bytes.ByteRate.Humanize(string,Humanizer.Localisation.TimeUnit,System.Globalization.CultureInfo).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

Unit of time to calculate rate for \(defaults is per second\)

<a name='Humanizer.Bytes.ByteRate.Humanize(string,Humanizer.Localisation.TimeUnit,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')