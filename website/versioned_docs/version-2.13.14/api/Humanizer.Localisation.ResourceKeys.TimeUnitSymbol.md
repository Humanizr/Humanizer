## ResourceKeys\.TimeUnitSymbol Class

Encapsulates the logic required to get the resource keys for TimeUnit\.ToSymbol

```csharp
public static class ResourceKeys.TimeUnitSymbol
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeUnitSymbol
### Methods

<a name='Humanizer.Localisation.ResourceKeys.TimeUnitSymbol.GetResourceKey(Humanizer.Localisation.TimeUnit)'></a>

## ResourceKeys\.TimeUnitSymbol\.GetResourceKey\(TimeUnit\) Method

Generates Resource Keys according to convention\.

```csharp
public static string GetResourceKey(Humanizer.Localisation.TimeUnit unit);
```
#### Parameters

<a name='Humanizer.Localisation.ResourceKeys.TimeUnitSymbol.GetResourceKey(Humanizer.Localisation.TimeUnit).unit'></a>

`unit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

Time unit, [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Resource key, like TimeSpanHumanize\_SingleMinute