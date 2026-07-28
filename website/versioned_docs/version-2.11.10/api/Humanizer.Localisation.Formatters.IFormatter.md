## IFormatter Interface

Implement this interface if your language has complex rules around dealing with numbers\. 
For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and 
in Arabic 2 days is يومين not 2 يوم

```csharp
public interface IFormatter
```

Derived  
↳ [DefaultFormatter](Humanizer.Localisation.Formatters.DefaultFormatter.md 'Humanizer\.Localisation\.Formatters\.DefaultFormatter')
### Methods

<a name='Humanizer.Localisation.Formatters.IFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool)'></a>

## IFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the string representation of the provided DataUnit, either as a symbol or full word

```csharp
string DataUnitHumanize(Humanizer.Localisation.DataUnit dataUnit, double count, bool toSymbol=true);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.IFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [DataUnit](Humanizer.Localisation.DataUnit.md 'Humanizer\.Localisation\.DataUnit')

Data unit

<a name='Humanizer.Localisation.Formatters.IFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Number of said units, to adjust for singular/plural forms

<a name='Humanizer.Localisation.Formatters.IFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the data unit should be expressed as symbol or full word

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided DataUnit

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int)'></a>

## IFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the string representation of the provided DateTime

```csharp
string DateHumanize(Humanizer.Localisation.TimeUnit timeUnit, Humanizer.Localisation.Tense timeUnitTense, int unit);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Localisation.Tense.md 'Humanizer\.Localisation\.Tense')

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize_Never()'></a>

## IFormatter\.DateHumanize\_Never\(\) Method

Never

```csharp
string DateHumanize_Never();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns Never

<a name='Humanizer.Localisation.Formatters.IFormatter.DateHumanize_Now()'></a>

## IFormatter\.DateHumanize\_Now\(\) Method

Now

```csharp
string DateHumanize_Now();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns Now

<a name='Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool)'></a>

## IFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the string representation of the provided TimeSpan

```csharp
string TimeSpanHumanize(Humanizer.Localisation.TimeUnit timeUnit, int unit, bool toWords=false);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

<a name='Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize_Zero()'></a>

## IFormatter\.TimeSpanHumanize\_Zero\(\) Method

0 seconds

```csharp
string TimeSpanHumanize_Zero();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns 0 seconds as the string representation of Zero TimeSpan