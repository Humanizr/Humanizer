## IFractionalTimeSpanFormatter Interface

Extends a formatter with support for fractional\-second duration values\.

```csharp
public interface IFractionalTimeSpanFormatter
```

Derived  
↳ [DefaultFormatter](Humanizer.DefaultFormatter.md 'Humanizer\.DefaultFormatter')
### Methods

<a name='Humanizer.IFractionalTimeSpanFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool)'></a>

## IFractionalTimeSpanFormatter\.TimeSpanHumanizeWithFractionalSeconds\(decimal, bool\) Method

Returns the localized representation of a seconds value\.

```csharp
string TimeSpanHumanizeWithFractionalSeconds(decimal seconds, bool toSymbols);
```
#### Parameters

<a name='Humanizer.IFractionalTimeSpanFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool).seconds'></a>

`seconds` [System\.Decimal](https://learn.microsoft.com/en-us/dotnet/api/system.decimal 'System\.Decimal')

The non\-negative seconds value to format\.

<a name='Humanizer.IFractionalTimeSpanFormatter.TimeSpanHumanizeWithFractionalSeconds(decimal,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether the seconds unit is rendered as a symbol\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized seconds value\.