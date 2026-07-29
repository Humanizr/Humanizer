## DefaultFormatter Class

Provides the standard formatter implementation for Humanizer locales\.

```csharp
public class DefaultFormatter : Humanizer.IFormatter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DefaultFormatter

Implements [IFormatter](Humanizer.IFormatter.md 'Humanizer\.IFormatter')
### Constructors

<a name='Humanizer.DefaultFormatter.DefaultFormatter(string)'></a>

## DefaultFormatter\(string\) Constructor

Initializes a new formatter for the specified locale code\.

```csharp
public DefaultFormatter(string localeCode);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DefaultFormatter(string).localeCode'></a>

`localeCode` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The locale code used to construct the formatter culture\.

<a name='Humanizer.DefaultFormatter.DefaultFormatter(System.Globalization.CultureInfo)'></a>

## DefaultFormatter\(CultureInfo\) Constructor

Initializes a new formatter for the specified culture\.

```csharp
public DefaultFormatter(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DefaultFormatter(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture used to resolve resources and localized number words\.
### Properties

<a name='Humanizer.DefaultFormatter.Culture'></a>

## DefaultFormatter\.Culture Property

Gets the culture used to resolve resources and localized number words\.

```csharp
protected System.Globalization.CultureInfo Culture { protected get; }
```

#### Property Value
[System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')
### Methods

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool)'></a>

## DefaultFormatter\.DataUnitHumanize\(DataUnit, double, bool\) Method

Returns the localized representation of a data unit, either as a symbol or a full word\.

```csharp
public virtual string DataUnitHumanize(Humanizer.DataUnit dataUnit, double count, bool toSymbol=true);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit'></a>

`dataUnit` [DataUnit](Humanizer.DataUnit.md 'Humanizer\.DataUnit')

The data unit to format\.

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).count'></a>

`count` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of units being described\.

<a name='Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).toSymbol'></a>

`toSymbol` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the unit should be rendered as a symbol\.

Implements [DataUnitHumanize\(DataUnit, double, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool) 'Humanizer\.IFormatter\.DataUnitHumanize\(Humanizer\.DataUnit, double, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized data\-unit representation\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [dataUnit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.DataUnitHumanize(Humanizer.DataUnit,double,bool).dataUnit 'Humanizer\.DefaultFormatter\.DataUnitHumanize\(Humanizer\.DataUnit, double, bool\)\.dataUnit') is unsupported\.

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int)'></a>

## DefaultFormatter\.DateHumanize\(TimeUnit, Tense, int\) Method

Returns the localized representation of a relative date phrase\.

```csharp
public virtual string DateHumanize(Humanizer.TimeUnit timeUnit, Humanizer.Tense timeUnitTense, int unit);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being described\.

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Tense.md 'Humanizer\.Tense')

Whether the reference is in the past or the future\.

<a name='Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of units being described\.

Implements [DateHumanize\(TimeUnit, Tense, int\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int) 'Humanizer\.IFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized relative date phrase\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit 'Humanizer\.DefaultFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)\.timeUnit') is unsupported or [unit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.DateHumanize(Humanizer.TimeUnit,Humanizer.Tense,int).unit 'Humanizer\.DefaultFormatter\.DateHumanize\(Humanizer\.TimeUnit, Humanizer\.Tense, int\)\.unit') is negative\.

<a name='Humanizer.DefaultFormatter.DateHumanize_Never()'></a>

## DefaultFormatter\.DateHumanize\_Never\(\) Method

Returns the localized text used when a date never occurs\.

```csharp
public virtual string DateHumanize_Never();
```

Implements [DateHumanize\_Never\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Never() 'Humanizer\.IFormatter\.DateHumanize\_Never\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.DateHumanize_Now()'></a>

## DefaultFormatter\.DateHumanize\_Now\(\) Method

Returns the localized text for the current moment\.

```csharp
public virtual string DateHumanize_Now();
```

Implements [DateHumanize\_Now\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.DateHumanize_Now() 'Humanizer\.IFormatter\.DateHumanize\_Now\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo)'></a>

## DefaultFormatter\.NumberToWords\(TimeUnit, int, CultureInfo\) Method

Converts a number to words for the current culture\.

```csharp
protected virtual string NumberToWords(Humanizer.TimeUnit unit, int number, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).unit'></a>

`unit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being formatted\.

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The numeric value to convert\.

<a name='Humanizer.DefaultFormatter.NumberToWords(Humanizer.TimeUnit,int,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use when generating the words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number rendered as words for the configured culture\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool)'></a>

## DefaultFormatter\.TimeSpanHumanize\(TimeUnit, int, bool\) Method

Returns the localized representation of a duration\.

```csharp
public virtual string TimeSpanHumanize(Humanizer.TimeUnit timeUnit, int unit, bool toWords=false);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The unit being described\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit'></a>

`unit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of units being described\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the number should be rendered as words\.

Implements [TimeSpanHumanize\(TimeUnit, int, bool\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool) 'Humanizer\.IFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized duration phrase\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).timeUnit 'Humanizer\.DefaultFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)\.timeUnit') is unsupported or [unit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.TimeSpanHumanize(Humanizer.TimeUnit,int,bool).unit 'Humanizer\.DefaultFormatter\.TimeSpanHumanize\(Humanizer\.TimeUnit, int, bool\)\.unit') is negative\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool)'></a>

## DefaultFormatter\.TimeSpanHumanizeWithFractionalSeconds\(decimal, bool\) Method

Returns the localized representation of a non\-negative seconds value\.

```csharp
public virtual string TimeSpanHumanizeWithFractionalSeconds(decimal seconds, bool toSymbols);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool).seconds'></a>

`seconds` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The non\-negative seconds value to format\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the seconds unit is rendered as a symbol\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized seconds value\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize_Age()'></a>

## DefaultFormatter\.TimeSpanHumanize\_Age\(\) Method

Returns the localized age suffix format for a humanized duration\.

```csharp
public virtual string TimeSpanHumanize_Age();
```

Implements [TimeSpanHumanize\_Age\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Age() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Age\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized age suffix format\.

<a name='Humanizer.DefaultFormatter.TimeSpanHumanize_Zero()'></a>

## DefaultFormatter\.TimeSpanHumanize\_Zero\(\) Method

Returns the localized representation of a zero\-length duration\.

```csharp
public virtual string TimeSpanHumanize_Zero();
```

Implements [TimeSpanHumanize\_Zero\(\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeSpanHumanize_Zero() 'Humanizer\.IFormatter\.TimeSpanHumanize\_Zero\(\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized zero\-duration phrase\.

<a name='Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit)'></a>

## DefaultFormatter\.TimeUnitHumanize\(TimeUnit\) Method

Returns the localized symbol for the given [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')\.

```csharp
public virtual string TimeUnitHumanize(Humanizer.TimeUnit timeUnit);
```
#### Parameters

<a name='Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The time unit to format\.

Implements [TimeUnitHumanize\(TimeUnit\)](Humanizer.IFormatter.md#Humanizer.IFormatter.TimeUnitHumanize(Humanizer.TimeUnit) 'Humanizer\.IFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized symbol for [timeUnit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit 'Humanizer\.DefaultFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)\.timeUnit')\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
If [timeUnit](Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.TimeUnitHumanize(Humanizer.TimeUnit).timeUnit 'Humanizer\.DefaultFormatter\.TimeUnitHumanize\(Humanizer\.TimeUnit\)\.timeUnit') is unsupported\.