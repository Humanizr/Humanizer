## ResourceKeys\.DateHumanize Class

Encapsulates the logic required to get the resource keys for DateTime\.Humanize

```csharp
public static class ResourceKeys.DateHumanize
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DateHumanize
### Fields

<a name='Humanizer.ResourceKeys.DateHumanize.Never'></a>

## ResourceKeys\.DateHumanize\.Never Field

Resource key for Never\.

```csharp
public const string Never = "DateHumanize_Never";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ResourceKeys.DateHumanize.Now'></a>

## ResourceKeys\.DateHumanize\.Now Field

Resource key for Now\.

```csharp
public const string Now = "DateHumanize_Now";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='Humanizer.ResourceKeys.DateHumanize.GetResourceKey(Humanizer.TimeUnit,Humanizer.Tense,int)'></a>

## ResourceKeys\.DateHumanize\.GetResourceKey\(TimeUnit, Tense, int\) Method

Generates Resource Keys according to convention\.

```csharp
public static string GetResourceKey(Humanizer.TimeUnit timeUnit, Humanizer.Tense timeUnitTense, int count=1);
```
#### Parameters

<a name='Humanizer.ResourceKeys.DateHumanize.GetResourceKey(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnit'></a>

`timeUnit` [TimeUnit](Humanizer.TimeUnit.md 'Humanizer\.TimeUnit')

Time unit

<a name='Humanizer.ResourceKeys.DateHumanize.GetResourceKey(Humanizer.TimeUnit,Humanizer.Tense,int).timeUnitTense'></a>

`timeUnitTense` [Tense](Humanizer.Tense.md 'Humanizer\.Tense')

Is time unit in future or past

<a name='Humanizer.ResourceKeys.DateHumanize.GetResourceKey(Humanizer.TimeUnit,Humanizer.Tense,int).count'></a>

`count` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number of units, default is One\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Resource key, like DateHumanize\_SingleMinuteAgo