## TimeSpanHumanizeExtensions Class

Humanizes TimeSpan into human readable form

```csharp
public static class TimeSpanHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeSpanHumanizeExtensions
### Methods

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool)'></a>

## TimeSpanHumanizeExtensions\.Humanize\(this TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool\) Method

Turns a TimeSpan into a human readable form\. E\.g\. 1 day\.

```csharp
public static string Humanize(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture=null, Humanizer.TimeUnit maxUnit=Humanizer.TimeUnit.Week, Humanizer.TimeUnit minUnit=Humanizer.TimeUnit.Millisecond, string? collectionSeparator=", ", bool toWords=false);
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Controls whether empty time units should be counted towards maximum number of time units\. Leading empty time units never count\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Week 'Humanizer\.TimeUnit\.Week')\. The time units [Month](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Month 'Humanizer\.TimeUnit\.Month') and [Year](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Year 'Humanizer\.TimeUnit\.Year') will give approximations for time spans bigger than 30 days by calculating with 365\.2425 days a year and 30\.4369 days a month\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses words instead of numbers if true\. E\.g\. one day\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool)'></a>

## TimeSpanHumanizeExtensions\.Humanize\(this TimeSpan, int, CultureInfo, TimeUnit, TimeUnit, string, bool\) Method

Turns a TimeSpan into a human readable form\. E\.g\. 1 day\.

```csharp
public static string Humanize(this System.TimeSpan timeSpan, int precision=1, System.Globalization.CultureInfo? culture=null, Humanizer.TimeUnit maxUnit=Humanizer.TimeUnit.Week, Humanizer.TimeUnit minUnit=Humanizer.TimeUnit.Millisecond, string? collectionSeparator=", ", bool toWords=false);
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\. Defaulted is 1 which means the largest unit is returned

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Week 'Humanizer\.TimeUnit\.Week')\. The time units [Month](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Month 'Humanizer\.TimeUnit\.Month') and [Year](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Year 'Humanizer\.TimeUnit\.Year') will give approximations for time spans bigger 30 days by calculating with 365\.2425 days a year and 30\.4369 days a month\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses words instead of numbers if true\. E\.g\. one day\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeToSymbols\(this TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human readable form using localized unit symbols\.

```csharp
public static string HumanizeToSymbols(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture=null, Humanizer.TimeUnit maxUnit=Humanizer.TimeUnit.Week, Humanizer.TimeUnit minUnit=Humanizer.TimeUnit.Millisecond, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Controls whether empty time units should be counted towards the maximum number of time units\. Leading empty time units never count\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Week 'Humanizer\.TimeUnit\.Week')\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeToSymbols\(this TimeSpan, int, CultureInfo, TimeUnit, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human readable form using localized unit symbols\.

```csharp
public static string HumanizeToSymbols(this System.TimeSpan timeSpan, int precision=1, System.Globalization.CultureInfo? culture=null, Humanizer.TimeUnit maxUnit=Humanizer.TimeUnit.Week, Humanizer.TimeUnit minUnit=Humanizer.TimeUnit.Millisecond, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Week 'Humanizer\.TimeUnit\.Week')\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbols(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeToSymbolsWithFractionalSeconds\(this TimeSpan, int, bool, int, MidpointRounding, CultureInfo, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human\-readable form using localized unit symbols and fractional seconds\.

```csharp
public static string HumanizeToSymbolsWithFractionalSeconds(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, int maxFractionalDigits, System.MidpointRounding roundingMode, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.TimeSpanHumanizeExtensions.md#Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision 'Humanizer\.TimeSpanHumanizeExtensions\.HumanizeToSymbolsWithFractionalSeconds\(this System\.TimeSpan, int, bool, int, System\.MidpointRounding, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, string\)\.precision')\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\. Only [System\.MidpointRounding\.ToEven](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.toeven 'System\.MidpointRounding\.ToEven') and [System\.MidpointRounding\.AwayFromZero](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.awayfromzero 'System\.MidpointRounding\.AwayFromZero') are supported\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span, using seconds as its minimum unit\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeToSymbolsWithFractionalSeconds\(this TimeSpan, int, int, MidpointRounding, CultureInfo, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human\-readable form using localized unit symbols and fractional seconds\.

```csharp
public static string HumanizeToSymbolsWithFractionalSeconds(this System.TimeSpan timeSpan, int precision, int maxFractionalDigits, System.MidpointRounding roundingMode, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\. Only [System\.MidpointRounding\.ToEven](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.toeven 'System\.MidpointRounding\.ToEven') and [System\.MidpointRounding\.AwayFromZero](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.awayfromzero 'System\.MidpointRounding\.AwayFromZero') are supported\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeToSymbolsWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span, using seconds as its minimum unit\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeWithFractionalSeconds\(this TimeSpan, int, bool, int, MidpointRounding, CultureInfo, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human\-readable form with fractional seconds\.

```csharp
public static string HumanizeWithFractionalSeconds(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, int maxFractionalDigits, System.MidpointRounding roundingMode, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.TimeSpanHumanizeExtensions.md#Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision 'Humanizer\.TimeSpanHumanizeExtensions\.HumanizeWithFractionalSeconds\(this System\.TimeSpan, int, bool, int, System\.MidpointRounding, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, string\)\.precision')\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\. Only [System\.MidpointRounding\.ToEven](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.toeven 'System\.MidpointRounding\.ToEven') and [System\.MidpointRounding\.AwayFromZero](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.awayfromzero 'System\.MidpointRounding\.AwayFromZero') are supported\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,bool,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span, using seconds as its minimum unit\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string)'></a>

## TimeSpanHumanizeExtensions\.HumanizeWithFractionalSeconds\(this TimeSpan, int, int, MidpointRounding, CultureInfo, TimeUnit, string\) Method

Turns a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into a human\-readable form with fractional seconds\.

```csharp
public static string HumanizeWithFractionalSeconds(this System.TimeSpan timeSpan, int precision, int maxFractionalDigits, System.MidpointRounding roundingMode, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, string? collectionSeparator=", ");
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxFractionalDigits'></a>

`maxFractionalDigits` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of fractional\-second digits, from 0 through 7\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).roundingMode'></a>

`roundingMode` [System\.MidpointRounding](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding 'System\.MidpointRounding')

The midpoint rounding mode\. Only [System\.MidpointRounding\.ToEven](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.toeven 'System\.MidpointRounding\.ToEven') and [System\.MidpointRounding\.AwayFromZero](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding.awayfromzero 'System\.MidpointRounding\.AwayFromZero') are supported\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.HumanizeWithFractionalSeconds(thisSystem.TimeSpan,int,int,System.MidpointRounding,System.Globalization.CultureInfo,Humanizer.TimeUnit,string).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span, using seconds as its minimum unit\.

<a name='Humanizer.TimeSpanHumanizeExtensions.ToAge(thisSystem.TimeSpan,System.Globalization.CultureInfo,Humanizer.TimeUnit,bool)'></a>

## TimeSpanHumanizeExtensions\.ToAge\(this TimeSpan, CultureInfo, TimeUnit, bool\) Method

Turns a TimeSpan into an age expression, e\.g\. "40 years old"

```csharp
public static string ToAge(this System.TimeSpan timeSpan, System.Globalization.CultureInfo? culture=null, Humanizer.TimeUnit maxUnit=Humanizer.TimeUnit.Year, bool toWords=false);
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.ToAge(thisSystem.TimeSpan,System.Globalization.CultureInfo,Humanizer.TimeUnit,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

Elapsed time

<a name='Humanizer.TimeSpanHumanizeExtensions.ToAge(thisSystem.TimeSpan,System.Globalization.CultureInfo,Humanizer.TimeUnit,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.ToAge(thisSystem.TimeSpan,System.Globalization.CultureInfo,Humanizer.TimeUnit,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\. The default value is [Year](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Year 'Humanizer\.TimeUnit\.Year')\.

<a name='Humanizer.TimeSpanHumanizeExtensions.ToAge(thisSystem.TimeSpan,System.Globalization.CultureInfo,Humanizer.TimeUnit,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses words instead of numbers if true\. E\.g\. "forty years old"\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Age expression in the given culture/language