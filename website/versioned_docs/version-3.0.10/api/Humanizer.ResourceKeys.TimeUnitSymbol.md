## ResourceKeys\.TimeUnitSymbol Class

Encapsulates the logic required to get the resource keys for TimeUnit\.ToSymbol

```csharp
public static class ResourceKeys.TimeUnitSymbol
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeUnitSymbol
### Methods

<a name='Humanizer.ResourceKeys.TimeUnitSymbol.GetResourceKey(Humanizer.TimeUnit)'></a>

## ResourceKeys\.TimeUnitSymbol\.GetResourceKey\(TimeUnit\) Method

Generates Resource Keys according to convention\.
Examples: TimeUnit\_Minute, TimeUnit\_Hour\.

```csharp
public static string GetResourceKey(Humanizer.TimeUnit unit);
```
#### Parameters

<a name='Humanizer.ResourceKeys.TimeUnitSymbol.GetResourceKey(Humanizer.TimeUnit).unit'></a>

`unit` [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')

Time unit, [Humanizer\.TimeUnit](https://learn.microsoft.com/en-us/dotnet/api/humanizer.timeunit 'Humanizer\.TimeUnit')\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Resource key, like TimeSpanHumanize\_SingleMinute