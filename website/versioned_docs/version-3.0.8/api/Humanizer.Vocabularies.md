## Vocabularies Class

Container for registered Vocabularies\.  At present, only a single vocabulary is supported: Default\.

```csharp
public static class Vocabularies
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Vocabularies
### Properties

<a name='Humanizer.Vocabularies.Default'></a>

## Vocabularies\.Default Property

The default vocabulary used for singular/plural irregularities\.
Rules can be added to this vocabulary and will be picked up by called to Singularize\(\) and Pluralize\(\)\.
At this time, multiple vocabularies and removing existing rules are not supported\.

```csharp
public static Humanizer.Vocabulary Default { get; }
```

#### Property Value
[Vocabulary](Humanizer.Vocabulary.md 'Humanizer\.Vocabulary')