## ITimeOnlyToClockNotationConverter Interface

The interface used to localise the ToClockNotation method\.

```csharp
public interface ITimeOnlyToClockNotationConverter
```
### Methods

<a name='Humanizer.Localisation.TimeToClockNotation.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding)'></a>

## ITimeOnlyToClockNotationConverter\.Convert\(TimeOnly, ClockNotationRounding\) Method

Converts the time to Clock Notation

```csharp
string Convert(System.TimeOnly time, Humanizer.ClockNotationRounding roundToNearestFive);
```
#### Parameters

<a name='Humanizer.Localisation.TimeToClockNotation.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding).time'></a>

`time` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

<a name='Humanizer.Localisation.TimeToClockNotation.ITimeOnlyToClockNotationConverter.Convert(System.TimeOnly,Humanizer.ClockNotationRounding).roundToNearestFive'></a>

`roundToNearestFive` [ClockNotationRounding](Humanizer.ClockNotationRounding.md 'Humanizer\.ClockNotationRounding')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')