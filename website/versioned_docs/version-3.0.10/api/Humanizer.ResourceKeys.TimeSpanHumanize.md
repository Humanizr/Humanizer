## ResourceKeys\.TimeSpanHumanize Class

Encapsulates the logic required to get the resource keys for TimeSpan\.Humanize
Examples: TimeSpanHumanize\_SingleMinute, TimeSpanHumanize\_MultipleHours\.

```csharp
public static class ResourceKeys.TimeSpanHumanize
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeSpanHumanize
### Methods

<a name='Humanizer.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.TimeUnit,int,bool)'></a>

## ResourceKeys\.TimeSpanHumanize\.GetResourceKey\(TimeUnit, int, bool\) Method

Generates Resource Keys according to convention\.

```csharp
public static string GetResourceKey(Humanizer.TimeUnit unit, int count=1, bool toWords=false);
```
#### Parameters

<a name='Humanizer.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.TimeUnit,int,bool).unit'></a>

`unit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Time unit, [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')\.

<a name='Humanizer.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.TimeUnit,int,bool).count'></a>

`count` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number of units, default is One\.

<a name='Humanizer.ResourceKeys.TimeSpanHumanize.GetResourceKey(Humanizer.TimeUnit,int,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Result to words, default is false\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Resource key, like TimeSpanHumanize\_SingleMinute