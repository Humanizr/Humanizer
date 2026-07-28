## IFormatter Interface

Implement this interface if your language has complex rules around dealing with numbers\.
For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and
in Arabic 2 days is يومين not 2 يوم

```csharp
public interface IFormatter
```

Derived  
↳ [DefaultFormatter](Humanizer.DefaultFormatter.md 'Humanizer\.DefaultFormatter')
### Methods

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool)'></a>

## IFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the string representation of the provided DataUnit, either as a symbol or full word

```csharp
string DataUnitHumanize(Humanizer.DataUnit dataUnit, double count, bool toSymbol=true);
```
#### Parameters

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [Humanizer\.DataUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.dataunit 'Humanizer\.DataUnit')

Data unit

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Number of said units, to adjust for singular/plural forms

<a name='Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the data unit should be expressed as symbol or full word

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided DataUnit

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int)'></a>

## IFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the string representation of the provided DateTime

```csharp
string DateHumanize(Humanizer.TimeUnit timeUnit, Humanizer.Tense timeUnitTense, int unit);
```
#### Parameters

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit'></a>

`timeUnit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Tense.md 'Humanizer\.Tense')

<a name='Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool)'></a>

## IFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the string representation of the provided TimeSpan

```csharp
string TimeSpanHumanize(Humanizer.TimeUnit timeUnit, int unit, bool toWords=false);
```
#### Parameters

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.IFormatter.TimeSpanHumanize_Age()'></a>

## IFormatter\.TimeSpanHumanize\_Age\(\) Method

Returns the age format that converts a humanized TimeSpan string to an age expression\.
For instance, in English that format adds the " old" suffix, so that "40 years" becomes "40 years old"\.

```csharp
string TimeSpanHumanize_Age();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Age format

<a name='Humanizer.IFormatter.TimeSpanHumanize_Zero()'></a>

## IFormatter\.TimeSpanHumanize\_Zero\(\) Method

0 seconds

```csharp
string TimeSpanHumanize_Zero();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns 0 seconds as the string representation of Zero TimeSpan

<a name='Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit)'></a>

## IFormatter\.TimeUnitHumanize\(TimeUnit\) Method

Returns the symbol for the given TimeUnit

```csharp
string TimeUnitHumanize(Humanizer.TimeUnit timeUnit);
```
#### Parameters

<a name='Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

Time unit

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided TimeUnit