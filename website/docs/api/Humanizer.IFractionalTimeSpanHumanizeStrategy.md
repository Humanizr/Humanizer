## IFractionalTimeSpanHumanizeStrategy Interface

Extends a time\-span humanization strategy with fractional\-second support\.

```csharp
public interface IFractionalTimeSpanHumanizeStrategy
```
### Methods

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool)'></a>

## IFractionalTimeSpanHumanizeStrategy\.HumanizeWithFractionalSeconds\(TimeSpan, int, bool, CultureInfo, TimeUnit, string, int, MidpointRounding, bool\) Method

Converts a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into human\-readable text with seconds as the minimum unit\.

```csharp
string HumanizeWithFractionalSeconds(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator, int maxFractionalDigits, System.MidpointRounding roundingMode, bool toSymbols);
```
#### Parameters

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.IFractionalTimeSpanHumanizeStrategy.md#Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).precision 'Humanizer\.IFractionalTimeSpanHumanizeStrategy\.HumanizeWithFractionalSeconds\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, string, int, System\.MidpointRounding, bool\)\.precision')\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If null, the current culture is used\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\.

<a name='Humanizer.IFractionalTimeSpanHumanizeStrategy.HumanizeWithFractionalSeconds(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,string,int,System.MidpointRounding,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether time units are rendered as symbols\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span\.