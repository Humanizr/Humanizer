## PrepositionsExtensions Class

[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime') extensions related to spatial or temporal relations

```csharp
public static class PrepositionsExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → PrepositionsExtensions
### Methods

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int)'></a>

## PrepositionsExtensions\.At\(this DateTime, int, int, int, int\) Method

Returns a new [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime') with the specified hour and, optionally
provided minutes, seconds, and milliseconds\.

```csharp
public static System.DateTime At(this System.DateTime date, int hour, int min=0, int second=0, int millisecond=0);
```
#### Parameters

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int).hour'></a>

`hour` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int).min'></a>

`min` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int).second'></a>

`second` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.PrepositionsExtensions.At(thisSystem.DateTime,int,int,int,int).millisecond'></a>

`millisecond` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.PrepositionsExtensions.AtMidnight(thisSystem.DateTime)'></a>

## PrepositionsExtensions\.AtMidnight\(this DateTime\) Method

Returns a new instance of DateTime based on the provided date where the time is set to midnight

```csharp
public static System.DateTime AtMidnight(this System.DateTime date);
```
#### Parameters

<a name='Humanizer.PrepositionsExtensions.AtMidnight(thisSystem.DateTime).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

#### Returns
[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.PrepositionsExtensions.AtNoon(thisSystem.DateTime)'></a>

## PrepositionsExtensions\.AtNoon\(this DateTime\) Method

Returns a new instance of DateTime based on the provided date where the time is set to noon

```csharp
public static System.DateTime AtNoon(this System.DateTime date);
```
#### Parameters

<a name='Humanizer.PrepositionsExtensions.AtNoon(thisSystem.DateTime).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

#### Returns
[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.PrepositionsExtensions.In(thisSystem.DateTime,int)'></a>

## PrepositionsExtensions\.In\(this DateTime, int\) Method

Returns a new instance of DateTime based on the provided date where the year is set to the provided year

```csharp
public static System.DateTime In(this System.DateTime date, int year);
```
#### Parameters

<a name='Humanizer.PrepositionsExtensions.In(thisSystem.DateTime,int).date'></a>

`date` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

<a name='Humanizer.PrepositionsExtensions.In(thisSystem.DateTime,int).year'></a>

`year` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

#### Returns
[System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')