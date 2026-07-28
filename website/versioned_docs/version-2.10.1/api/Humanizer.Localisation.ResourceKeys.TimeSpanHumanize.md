## ResourceKeys\.TimeSpanHumanize Class

Encapsulates the logic required to get the resource keys for TimeSpan\.Humanize

```csharp
public static class ResourceKeys.TimeSpanHumanize
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeSpanHumanize
### Methods

<a name='Humanizer.Localisation.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.Localisation.TimeUnit,int,bool)'></a>

## ResourceKeys\.TimeSpanHumanize\.GetResourceKey\(TimeUnit, int, bool\) Method

Generates Resource Keys according to convention\.

```csharp
public static string GetResourceKey(Humanizer.Localisation.TimeUnit unit, int count=1, bool toWords=false);
```
#### Parameters

<a name='Humanizer.Localisation.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.Localisation.TimeUnit,int,bool).unit'></a>

`unit` [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')

Time unit, [TimeUnit](Humanizer.Localisation.TimeUnit.md 'Humanizer\.Localisation\.TimeUnit')\.

<a name='Humanizer.Localisation.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.Localisation.TimeUnit,int,bool).count'></a>

`count` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number of units, default is One\.

<a name='Humanizer.Localisation.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.Localisation.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Result to words, default is false\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Resource key, like TimeSpanHumanize\_SingleMinute