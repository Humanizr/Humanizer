## ITimeOnlyToClockNotationConverter Interface

Converts times into the localized text used by `ToClockNotation`\.

```csharp
public interface ITimeOnlyToClockNotationConverter
```
### Methods

<a name='Humanizer.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding)'></a>

## ITimeOnlyToClockNotationConverter\.Convert\(TimeOnly, ClockNotationRounding\) Method

Converts the given [time](Humanizer.ITimeOnlyToClockNotationConverter.md#Humanizer.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding).time 'Humanizer\.ITimeOnlyToClockNotationConverter\.Convert\(System\.TimeOnly, Humanizer\.ClockNotationRounding\)\.time') to clock notation\.

```csharp
string Convert(System.TimeOnly time, Humanizer.ClockNotationRounding roundToNearestFive);
```
#### Parameters

<a name='Humanizer.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding).time'></a>

`time` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

The time to format\.

<a name='Humanizer.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding).roundToNearestFive'></a>

`roundToNearestFive` [ClockNotationRounding](Humanizer.ClockNotationRounding.md 'Humanizer\.ClockNotationRounding')

The rounding mode to apply before formatting the time\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized clock\-notation string\.