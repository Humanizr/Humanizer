## ITimeSpanHumanizeStrategy Interface

Defines a strategy for converting [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') values into human\-readable text\.

```csharp
public interface ITimeSpanHumanizeStrategy
```

Derived  
↳ [DefaultTimeSpanHumanizeStrategy](Humanizer.DefaultTimeSpanHumanizeStrategy.md 'Humanizer\.DefaultTimeSpanHumanizeStrategy')  
↳ [IGrammaticalCaseTimeSpanHumanizeStrategy](Humanizer.IGrammaticalCaseTimeSpanHumanizeStrategy.md 'Humanizer\.IGrammaticalCaseTimeSpanHumanizeStrategy')
### Methods

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool)'></a>

## ITimeSpanHumanizeStrategy\.Humanize\(TimeSpan, int, bool, CultureInfo, TimeUnit, TimeUnit, string, bool, bool\) Method

Converts a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') into human\-readable text\.

```csharp
string Humanize(System.TimeSpan timeSpan, int precision, bool countEmptyUnits, System.Globalization.CultureInfo? culture, Humanizer.TimeUnit maxUnit, Humanizer.TimeUnit minUnit, string? collectionSeparator, bool toWords, bool toSymbols);
```
#### Parameters

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).timeSpan'></a>

`timeSpan` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The time span to humanize\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of time units to return\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).countEmptyUnits'></a>

`countEmptyUnits` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether empty time units count toward [precision](Humanizer.ITimeSpanHumanizeStrategy.md#Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).precision 'Humanizer\.ITimeSpanHumanizeStrategy\.Humanize\(System\.TimeSpan, int, bool, System\.Globalization\.CultureInfo, Humanizer\.TimeUnit, Humanizer\.TimeUnit, string, bool, bool\)\.precision')\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If null, the current culture is used\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).maxUnit'></a>

`maxUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The maximum unit of time to output\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).minUnit'></a>

`minUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

The minimum unit of time to output\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).collectionSeparator'></a>

`collectionSeparator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator used to combine time parts\. If null, the culture's default collection formatter is used\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether numbers are rendered as words\.

<a name='Humanizer.ITimeSpanHumanizeStrategy.Humanize(System.TimeSpan,int,bool,System.Globalization.CultureInfo,Humanizer.TimeUnit,Humanizer.TimeUnit,string,bool,bool).toSymbols'></a>

`toSymbols` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether time units are rendered as symbols\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The human\-readable time span\.