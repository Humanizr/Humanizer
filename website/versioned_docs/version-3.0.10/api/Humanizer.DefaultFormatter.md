## DefaultFormatter Class

Default implementation of IFormatter interface\.

```csharp
public class DefaultFormatter : Humanizer.IFormatter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultFormatter

Implements [IFormatter](Humanizer.IFormatter.md 'Humanizer\.IFormatter')
### Constructors

<a name='Humanizer.DefaultFormatter.DefaultFormatter(string)'></a>

## DefaultFormatter\(string\) Constructor

```csharp
public DefaultFormatter(string localeCode);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DefaultFormatter(string).localeCode'></a>

`localeCode` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.DefaultFormatter(System.Globalization.CultureInfo)'></a>

## DefaultFormatter\(CultureInfo\) Constructor

Default implementation of IFormatter interface\.

```csharp
public DefaultFormatter(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DefaultFormatter(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')
### Properties

<a name='Humanizer.DefaultFormatter.Culture'></a>

## DefaultFormatter\.Culture Property

```csharp
protected System.Globalization.CultureInfo Culture { protected get; }
```

#### Property Value
[System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')
### Methods

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool)'></a>

## DefaultFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the string representation of the provided DataUnit, either as a symbol or full word

```csharp
public virtual string DataUnitHumanize(Humanizer.DataUnit dataUnit, double count, bool toSymbol=true);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [DataUnit](Humanizer.DataUnit.md 'Humanizer\.DataUnit')

Data unit

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Number of said units, to adjust for singular/plural forms

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether the data unit should be expressed as symbol or full word

Implements [DataUnitHumanize\(DataUnit, double, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool) 'Humanizer\.IFormatter\.DataUnitHumanize\(Humanizer\.DataUnit, double, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided DataUnit

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int)'></a>

## DefaultFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the string representation of the provided DateTime

```csharp
public virtual string DateHumanize(Humanizer.TimeUnit timeUnit, Humanizer.Tense timeUnitTense, int unit);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Tense.md 'Humanizer\.Tense')

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Implements [DateHumanize\(TimeUnit, Tense, int\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int) 'Humanizer\.IFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.DateHumanize_Never()'></a>

## DefaultFormatter\.DateHumanize\_Never\(\) Method

```csharp
public virtual string DateHumanize_Never();
```

Implements [DateHumanize\_Never\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Never() 'Humanizer\.IFormatter\.DateHumanize\_Never\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.DateHumanize_Now()'></a>

## DefaultFormatter\.DateHumanize\_Now\(\) Method

```csharp
public virtual string DateHumanize_Now();
```

Implements [DateHumanize\_Now\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Now() 'Humanizer\.IFormatter\.DateHumanize\_Now\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.Format(Humanizer.TimeUnit,string,int,bool)'></a>

## DefaultFormatter\.Format\(TimeUnit, string, int, bool\) Method

Formats the specified resource key\.

```csharp
protected virtual string Format(Humanizer.TimeUnit unit, string resourceKey, int number, bool toWords=false);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.Format(Humanizer.TimeUnit,string,int,bool).unit'></a>

`unit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

<a name='Humanizer.DefaultFormatter.Format(Humanizer.TimeUnit,string,int,bool).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key\.

<a name='Humanizer.DefaultFormatter.Format(Humanizer.TimeUnit,string,int,bool).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number\.

<a name='Humanizer.DefaultFormatter.Format(Humanizer.TimeUnit,string,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If the resource not exists on the specified culture\.

<a name='Humanizer.DefaultFormatter.Format(string)'></a>

## DefaultFormatter\.Format\(string\) Method

Formats the specified resource key\.

```csharp
protected virtual string Format(string resourceKey);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.Format(string).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If the resource not exists on the specified culture\.

<a name='Humanizer.DefaultFormatter.GetResourceKey(string)'></a>

## DefaultFormatter\.GetResourceKey\(string\) Method

```csharp
protected virtual string GetResourceKey(string resourceKey);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.GetResourceKey(string).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.GetResourceKey(string,int)'></a>

## DefaultFormatter\.GetResourceKey\(string, int\) Method

Override this method if your locale has complex rules around multiple units; e\.g\. Arabic, Russian

```csharp
protected virtual string GetResourceKey(string resourceKey, int number);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.GetResourceKey(string,int).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The resource key that's being in formatting

<a name='Humanizer.DefaultFormatter.GetResourceKey(string,int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of the units being used in formatting

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo)'></a>

## DefaultFormatter\.NumberToWords\(TimeUnit, int, CultureInfo\) Method

```csharp
protected virtual string NumberToWords(Humanizer.TimeUnit unit, int number, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).unit'></a>

`unit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool)'></a>

## DefaultFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the string representation of the provided TimeSpan

```csharp
public virtual string TimeSpanHumanize(Humanizer.TimeUnit timeUnit, int unit, bool toWords=false);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

A time unit to represent\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Implements [TimeSpanHumanize\(TimeUnit, int, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool) 'Humanizer\.IFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Is thrown when timeUnit is larger than TimeUnit\.Week

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize_Age()'></a>

## DefaultFormatter\.TimeSpanHumanize\_Age\(\) Method

Returns the age format that converts a humanized TimeSpan string to an age expression\.
For instance, in English that format adds the " old" suffix, so that "40 years" becomes "40 years old"\.

```csharp
public virtual string TimeSpanHumanize_Age();
```

Implements [TimeSpanHumanize\_Age\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Age() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Age\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Age format

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize_Zero()'></a>

## DefaultFormatter\.TimeSpanHumanize\_Zero\(\) Method

0 seconds

```csharp
public virtual string TimeSpanHumanize_Zero();
```

Implements [TimeSpanHumanize\_Zero\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Zero() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Zero\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Returns 0 seconds as the string representation of Zero TimeSpan

<a name='Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit)'></a>

## DefaultFormatter\.TimeUnitHumanize\(TimeUnit\) Method

Returns the symbol for the given TimeUnit

```csharp
public virtual string TimeUnitHumanize(Humanizer.TimeUnit timeUnit);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Time unit

Implements [TimeUnitHumanize\(TimeUnit\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit) 'Humanizer\.IFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
String representation of the provided TimeUnit