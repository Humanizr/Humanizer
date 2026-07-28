## TimeSpanHumanizeExtensions Class

Humanizes TimeSpan into human readable form

```csharp
public static class TimeSpanHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeSpanHumanizeExtensions
### Methods

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool)'></a>

## TimeSpanHumanizeExtensions\.Humanize\(this TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool\) Method

Turns a TimeSpan into a human readable form\. E\.g\. 1 day\.

```csharp
public static string Humanize(this System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo culture=null, Humanizer.Localisation.TimeUnit maxUnit=Humanizer.Localisation.TimeUnit.Week, Humanizer.Localisation.TimeUnit minUnit=Humanizer.Localisation.TimeUnit.Millisecond, string collectionSeparator=", ", bool toWords=false);
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Controls whether empty time units should be counted towards maximum number of time units\. Leading empty time units never count\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Week 'Humanizer\.Localisation\.TimeUnit\.Week')\. The time units [Month](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Month 'Humanizer\.Localisation\.TimeUnit\.Month') and [Year](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Year 'Humanizer\.Localisation\.TimeUnit\.Year') will give approximations for time spans bigger than 30 days by calculating with 365\.2425 days a year and 30\.4369 days a month\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses words instead of numbers if true\. E\.g\. one day\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool)'></a>

## TimeSpanHumanizeExtensions\.Humanize\(this TimeSpan, int, CultureInfo, TimeUnit, TimeUnit, string, bool\) Method

Turns a TimeSpan into a human readable form\. E\.g\. 1 day\.

```csharp
public static string Humanize(this System.TimeSpan timeSpan, int precision=1, System.Globalization.CultureInfo culture=null, Humanizer.Localisation.TimeUnit maxUnit=Humanizer.Localisation.TimeUnit.Week, Humanizer.Localisation.TimeUnit minUnit=Humanizer.Localisation.TimeUnit.Millisecond, string collectionSeparator=", ", bool toWords=false);
```
#### Parameters

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\. Defaulted is 1 which means the largest unit is returned

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

The maximum unit of time to output\. The default value is [Week](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Week 'Humanizer\.Localisation\.TimeUnit\.Week')\. The time units [Month](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Month 'Humanizer\.Localisation\.TimeUnit\.Month') and [Year](Humanizer.Localisation.TimeUnit.md#Humanizer.Localisation.TimeUnit.Year 'Humanizer\.Localisation\.TimeUnit\.Year') will give approximations for time spans bigger 30 days by calculating with 365\.2425 days a year and 30\.4369 days a month\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use when combining humanized time parts\. If null, the default collection formatter for the current culture is used\.

<a name='Humanizer.TimeSpanHumanizeExtensions.Humanize(thisSystem.TimeSpan,int,System.Globalization.CultureInfo,Humanizer.Localisation.TimeUnit,Humanizer.Localisation.TimeUnit,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses words instead of numbers if true\. E\.g\. one day\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')