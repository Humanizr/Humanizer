## TimeOnlyToClockNotationExtensions Class

Humanizes TimeOnly into human readable sentence

```csharp
public static class TimeOnlyToClockNotationExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeOnlyToClockNotationExtensions
### Methods

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding)'></a>

## TimeOnlyToClockNotationExtensions\.ToClockNotation\(this TimeOnly, ClockNotationRounding\) Method

Turns the provided time into clock notation

```csharp
public static string ToClockNotation(this System.TimeOnly input, Humanizer.ClockNotationRounding roundToNearestFive=Humanizer.ClockNotationRounding.None);
```
#### Parameters

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

The time to be made into clock notation

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding).roundToNearestFive'></a>

`roundToNearestFive` [ClockNotationRounding](Humanizer.ClockNotationRounding.md 'Humanizer\.ClockNotationRounding')

Whether to round the minutes to the nearest five or not

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The time in clock notation