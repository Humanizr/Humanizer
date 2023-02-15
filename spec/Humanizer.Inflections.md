## `Vocabularies`

Container for registered Vocabularies.  At present, only a single vocabulary is supported: Default.
```csharp
public static class Humanizer.Inflections.Vocabularies

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Vocabulary` | Default | The default vocabulary used for singular/plural irregularities.  Rules can be added to this vocabulary and will be picked up by called to Singularize() and Pluralize().  At this time, multiple vocabularies and removing existing rules are not supported. | 


## `Vocabulary`

A container for exceptions to simple pluralization/singularization rules.  Vocabularies.Default contains an extensive list of rules for US English.  At this time, multiple vocabularies and removing existing rules are not supported.
```csharp
public class Humanizer.Inflections.Vocabulary

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | AddIrregular(`String` singular, `String` plural, `Boolean` matchEnding = True) | Adds a word to the vocabulary which cannot easily be pluralized/singularized by RegEx, e.g. "person" and "people". | 
| `void` | AddPlural(`String` rule, `String` replacement) | Adds a rule to the vocabulary that does not follow trivial rules for pluralization, e.g. "bus" -&gt; "buses" | 
| `void` | AddSingular(`String` rule, `String` replacement) | Adds a rule to the vocabulary that does not follow trivial rules for singularization, e.g. "vertices/indices -&gt; "vertex/index" | 
| `void` | AddUncountable(`String` word) | Adds an uncountable word to the vocabulary, e.g. "fish".  Will be ignored when plurality is changed. | 
| `String` | Pluralize(`String` word, `Boolean` inputIsKnownToBeSingular = True) | Pluralizes the provided input considering irregular words | 
| `String` | Singularize(`String` word, `Boolean` inputIsKnownToBePlural = True, `Boolean` skipSimpleWords = False) | Singularizes the provided input considering irregular words | 


