## Vocabularies Class

Container for registered vocabularies\. At present, only a single vocabulary is supported: Default\.

```csharp
public static class Vocabularies
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Vocabularies
### Properties

<a name='Humanizer.Vocabularies.Default'></a>

## Vocabularies\.Default Property

The default vocabulary used for singular/plural irregularities and custom acronym casing\.
Rules and acronyms added to this vocabulary are used by Singularize\(\), Pluralize\(\), and Humanize\(\)\.
At this time, multiple vocabularies and removing existing rules are not supported\.

```csharp
public static Humanizer.Vocabulary Default { get; }
```

#### Property Value
[Vocabulary](Humanizer.Vocabulary.md 'Humanizer\.Vocabulary')