## DefaultFormatter Class

Default implementation of IFormatter interface\.

```csharp
public class DefaultFormatter : Humanizer.Localisation.Formatters.IFormatter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultFormatter

Implements [IFormatter](Humanizer.Localisation.Formatters.IFormatter.md 'Humanizer\.Localisation\.Formatters\.IFormatter')
### Constructors

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DefaultFormatter(string)'></a>

## DefaultFormatter\(string\) Constructor

Constructor\.

```csharp
public DefaultFormatter(string localeCode);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DefaultFormatter(string).localeCode'></a>

`localeCode` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Name of the culture to use\.
### Methods

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool)'></a>

## DefaultFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the string representation of the provided DataUnit, either as a symbol or full word

```csharp
public virtual string DataUnitHumanize(Humanizer.Localisation.DataUnit dataUnit, double count, bool toSymbol=true);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [DataUnit](Humanizer.Localisation.DataUnit.md 'Humanizer\.Localisation\.DataUnit')

Data unit

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Number of said units, to adjust for singular/plural forms

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the data unit should be expressed as symbol or full word

Implements [DataUnitHumanize\(DataUnit, double, bool\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.DataUnitHumanize(Humanizer.Localisation.DataUnit,double,bool) 'Humanizer\.Localisation\.Formatters\.IFormatter\.DataUnitHumanize\(Humanizer\.Localisation\.DataUnit, double, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided DataUnit

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int)'></a>

## DefaultFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the string representation of the provided DateTime

```csharp
public virtual string DateHumanize(Humanizer.Localisation.TimeUnit timeUnit, Humanizer.Localisation.Tense timeUnitTense, int unit);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Localisation.Tense.md 'Humanizer\.Localisation\.Tense')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Implements [DateHumanize\(TimeUnit, Tense, int\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.DateHumanize(Humanizer.Localisation.TimeUnit,Humanizer.Localisation.Tense,int) 'Humanizer\.Localisation\.Formatters\.IFormatter\.DateHumanize\(Humanizer\.Localisation\.TimeUnit, Humanizer\.Localisation\.Tense, int\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize_Never()'></a>

## DefaultFormatter\.DateHumanize\_Never\(\) Method

Never

```csharp
public virtual string DateHumanize_Never();
```

Implements [DateHumanize\_Never\(\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.DateHumanize_Never() 'Humanizer\.Localisation\.Formatters\.IFormatter\.DateHumanize\_Never\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns Never

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.DateHumanize_Now()'></a>

## DefaultFormatter\.DateHumanize\_Now\(\) Method

Now

```csharp
public virtual string DateHumanize_Now();
```

Implements [DateHumanize\_Now\(\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.DateHumanize_Now() 'Humanizer\.Localisation\.Formatters\.IFormatter\.DateHumanize\_Now\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns Now

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string)'></a>

## DefaultFormatter\.Format\(string\) Method

Formats the specified resource key\.

```csharp
protected virtual string Format(string resourceKey);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If the resource not exists on the specified culture\.

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string,int,bool)'></a>

## DefaultFormatter\.Format\(string, int, bool\) Method

Formats the specified resource key\.

```csharp
protected virtual string Format(string resourceKey, int number, bool toWords=false);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string,int,bool).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key\.

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string,int,bool).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number\.

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.Format(string,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If the resource not exists on the specified culture\.

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.GetResourceKey(string)'></a>

## DefaultFormatter\.GetResourceKey\(string\) Method

```csharp
protected virtual string GetResourceKey(string resourceKey);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.GetResourceKey(string).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.GetResourceKey(string,int)'></a>

## DefaultFormatter\.GetResourceKey\(string, int\) Method

Override this method if your locale has complex rules around multiple units; e\.g\. Arabic, Russian

```csharp
protected virtual string GetResourceKey(string resourceKey, int number);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.GetResourceKey(string,int).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key that's being in formatting

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.GetResourceKey(string,int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of the units being used in formatting

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool)'></a>

## DefaultFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the string representation of the provided TimeSpan

```csharp
public virtual string TimeSpanHumanize(Humanizer.Localisation.TimeUnit timeUnit, int unit, bool toWords=false);
```
#### Parameters

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

A time unit to represent\.

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Implements [TimeSpanHumanize\(TimeUnit, int, bool\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize(Humanizer.Localisation.TimeUnit,int,bool) 'Humanizer\.Localisation\.Formatters\.IFormatter\.TimeSpanHumanize\(Humanizer\.Localisation\.TimeUnit, int, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Is thrown when timeUnit is larger than TimeUnit\.Week

<a name='Humanizer.Localisation.Formatters.DefaultFormatter.TimeSpanHumanize_Zero()'></a>

## DefaultFormatter\.TimeSpanHumanize\_Zero\(\) Method

0 seconds

```csharp
public virtual string TimeSpanHumanize_Zero();
```

Implements [TimeSpanHumanize\_Zero\(\)](Humanizer.Localisation.Formatters.IFormatter.md#Humanizer.Localisation.Formatters.IFormatter.TimeSpanHumanize_Zero() 'Humanizer\.Localisation\.Formatters\.IFormatter\.TimeSpanHumanize\_Zero\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns 0 seconds as the string representation of Zero TimeSpan