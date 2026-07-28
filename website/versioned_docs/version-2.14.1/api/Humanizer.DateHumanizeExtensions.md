## DateHumanizeExtensions Class

Humanizes DateTime into human readable sentence

```csharp
public static class DateHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DateHumanizeExtensions
### Methods

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateOnly,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this DateOnly, Nullable\<DateOnly\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence

```csharp
public static string Humanize(this System.DateOnly input, System.Nullable<System.DateOnly> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateOnly,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateOnly,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateOnly,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTime,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this DateTime, Nullable\<bool\>, Nullable\<DateTime\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence

```csharp
public static string Humanize(this System.DateTime input, System.Nullable<bool> utcDate=null, System.Nullable<System.DateTime> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTime,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTime,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).utcDate'></a>

`utcDate` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Nullable boolean value indicating whether the date is in UTC or local\. If null, current date is used with the same DateTimeKind of input

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTime,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTime,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTimeOffset,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this DateTimeOffset, Nullable\<DateTimeOffset\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence

```csharp
public static string Humanize(this System.DateTimeOffset input, System.Nullable<System.DateTimeOffset> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTimeOffset,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).input'></a>

`input` [System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTimeOffset,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.DateTimeOffset,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateOnly_,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this Nullable\<DateOnly\>, Nullable\<DateOnly\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence, overload for the nullable DateTime, returning 'never' in case null

```csharp
public static string Humanize(this System.Nullable<System.DateOnly> input, System.Nullable<System.DateOnly> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateOnly_,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).input'></a>

`input` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateOnly_,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateOnly_,System.Nullable_System.DateOnly_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTime_,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this Nullable\<DateTime\>, Nullable\<bool\>, Nullable\<DateTime\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence, overload for the nullable DateTime, returning 'never' in case null

```csharp
public static string Humanize(this System.Nullable<System.DateTime> input, System.Nullable<bool> utcDate=null, System.Nullable<System.DateTime> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTime_,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).input'></a>

`input` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTime_,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).utcDate'></a>

`utcDate` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Nullable boolean value indicating whether the date is in UTC or local\. If null, current date is used with the same DateTimeKind of input

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTime_,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTime_,System.Nullable_bool_,System.Nullable_System.DateTime_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTimeOffset_,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this Nullable\<DateTimeOffset\>, Nullable\<DateTimeOffset\>, CultureInfo\) Method

Turns the current or provided date into a human readable sentence, overload for the nullable DateTimeOffset, returning 'never' in case null

```csharp
public static string Humanize(this System.Nullable<System.DateTimeOffset> input, System.Nullable<System.DateTimeOffset> dateToCompareAgainst=null, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTimeOffset_,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).input'></a>

`input` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTimeOffset_,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).dateToCompareAgainst'></a>

`dateToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset 'System\.DateTimeOffset')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.DateTimeOffset_,System.Nullable_System.DateTimeOffset_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this Nullable\<TimeOnly\>, Nullable\<TimeOnly\>, bool, CultureInfo\) Method

Turns the current or provided time into a human readable sentence, overload for the nullable TimeOnly, returning 'never' in case null

```csharp
public static string Humanize(this System.Nullable<System.TimeOnly> input, System.Nullable<System.TimeOnly> timeToCompareAgainst=null, bool useUtc=true, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).input'></a>

`input` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).timeToCompareAgainst'></a>

`timeToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Time to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).useUtc'></a>

`useUtc` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If [timeToCompareAgainst](Humanizer.DateHumanizeExtensions.md#Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).timeToCompareAgainst 'Humanizer\.DateHumanizeExtensions\.Humanize\(this System\.Nullable\<System\.TimeOnly\>, System\.Nullable\<System\.TimeOnly\>, bool, System\.Globalization\.CultureInfo\)\.timeToCompareAgainst') is null, used to determine if the current time is UTC or local\. Defaults to UTC\.

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.Nullable_System.TimeOnly_,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo)'></a>

## DateHumanizeExtensions\.Humanize\(this TimeOnly, Nullable\<TimeOnly\>, bool, CultureInfo\) Method

Turns the current or provided time into a human readable sentence

```csharp
public static string Humanize(this System.TimeOnly input, System.Nullable<System.TimeOnly> timeToCompareAgainst=null, bool useUtc=true, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

The date to be humanized

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).timeToCompareAgainst'></a>

`timeToCompareAgainst` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Date to compare the input against\. If null, current date is used as base

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).useUtc'></a>

`useUtc` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

If [timeToCompareAgainst](Humanizer.DateHumanizeExtensions.md#Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).timeToCompareAgainst 'Humanizer\.DateHumanizeExtensions\.Humanize\(this System\.TimeOnly, System\.Nullable\<System\.TimeOnly\>, bool, System\.Globalization\.CultureInfo\)\.timeToCompareAgainst') is null, used to determine if the current time is UTC or local\. Defaults to UTC\.

<a name='Humanizer.DateHumanizeExtensions.Humanize(thisSystem.TimeOnly,System.Nullable_System.TimeOnly_,bool,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
distance of time in words