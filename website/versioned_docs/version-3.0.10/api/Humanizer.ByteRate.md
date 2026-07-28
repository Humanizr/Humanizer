## ByteRate Class

Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer

```csharp
public class ByteRate
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ByteRate

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

<a name='Humanizer.ByteRate.Humanize(Humanizer.TimeUnit)'></a>

## ByteRate\.Humanize\(TimeUnit\) Method

Calculate rate for the quantity of bytes and interval defined by this instance

```csharp
public string Humanize(Humanizer.TimeUnit timeUnit=Humanizer.TimeUnit.Second);
```
#### Parameters

<a name='Humanizer.ByteRate.Humanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

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

`timeUnit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

Unit of time to calculate rate for \(defaults is per second\)

<a name='Humanizer.ByteRate.Humanize(string,Humanizer.TimeUnit,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')