## TimeOnlyToClockNotationExtensions Class

Humanizes TimeOnly into human readable sentence

```csharp
public static class TimeOnlyToClockNotationExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeOnlyToClockNotationExtensions
### Methods

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding)'></a>

## TimeOnlyToClockNotationExtensions\.ToClockNotation\(this TimeOnly, ClockNotationRounding\) Method

Converts a [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly') value to its clock notation string representation
\(e\.g\., "three o'clock", "half past four", "quarter to six"\)\.

```csharp
public static string ToClockNotation(this System.TimeOnly input, Humanizer.ClockNotationRounding roundToNearestFive=Humanizer.ClockNotationRounding.None);
```
#### Parameters

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding).input'></a>

`input` [System\.TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly 'System\.TimeOnly')

The time to be converted to clock notation\.

<a name='Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(thisSystem.TimeOnly,Humanizer.ClockNotationRounding).roundToNearestFive'></a>

`roundToNearestFive` [ClockNotationRounding](Humanizer.ClockNotationRounding.md 'Humanizer\.ClockNotationRounding')

Specifies whether and how to round the minutes\. Default is [None](Humanizer.ClockNotationRounding.md#Humanizer.ClockNotationRounding.None 'Humanizer\.ClockNotationRounding\.None')\.
\- [None](Humanizer.ClockNotationRounding.md#Humanizer.ClockNotationRounding.None 'Humanizer\.ClockNotationRounding\.None'): Use exact minutes
\- [NearestFiveMinutes](Humanizer.ClockNotationRounding.md#Humanizer.ClockNotationRounding.NearestFiveMinutes 'Humanizer\.ClockNotationRounding\.NearestFiveMinutes'): Round to nearest 5 minutes

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A culture\-specific string representation of the time in clock notation\.
For English: "three o'clock", "ten past four", "quarter to six", etc\.

### Example

```csharp
// English (en-US) examples:
new TimeOnly(15, 0).ToClockNotation() => "three o'clock"
new TimeOnly(15, 15).ToClockNotation() => "quarter past three"
new TimeOnly(15, 30).ToClockNotation() => "half past three"
new TimeOnly(15, 45).ToClockNotation() => "quarter to four"
new TimeOnly(15, 7).ToClockNotation(ClockNotationRounding.NearestFiveMinutes) => "five past three"
```

### Remarks
The output format varies by culture\. Some cultures express time differently than others\.
This method is only available on \.NET 6\.0 and later\.