## TimeUnitToSymbolExtensions Class

Transform a time unit into a symbol; e\.g\. [Year](Humanizer.TimeUnit.md#Humanizer.TimeUnit.Year 'Humanizer\.TimeUnit\.Year') =\> "a"

```csharp
public static class TimeUnitToSymbolExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeUnitToSymbolExtensions
### Methods

<a name='Humanizer.TimeUnitToSymbolExtensions.ToSymbol(thisHumanizer.TimeUnit,System.Globalization.CultureInfo)'></a>

## TimeUnitToSymbolExtensions\.ToSymbol\(this TimeUnit, CultureInfo\) Method

TimeUnit\.Day\.ToSymbol\(\) \-\> "d"

```csharp
public static string ToSymbol(this Humanizer.TimeUnit unit, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.TimeUnitToSymbolExtensions.ToSymbol(thisHumanizer.TimeUnit,System.Globalization.CultureInfo).unit'></a>

`unit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Unit of time to be turned to a symbol

<a name='Humanizer.TimeUnitToSymbolExtensions.ToSymbol(thisHumanizer.TimeUnit,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')