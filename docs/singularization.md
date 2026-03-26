# Singularization

Convert plural English words to their singular form, with built-in support for irregular and uncountable words.

## Basic Usage

```csharp
using Humanizer;

"cats".Singularize()     // => "cat"
"men".Singularize()      // => "man"
"people".Singularize()   // => "person"
"boxes".Singularize()    // => "box"
"children".Singularize() // => "child"
"geese".Singularize()    // => "goose"
```

## Irregular Words

The default vocabulary handles common irregular singulars:

```csharp
"people".Singularize()      // => "person"
"salespeople".Singularize() // => "salesperson"
"men".Singularize()         // => "man"
"women".Singularize()       // => "woman"
"children".Singularize()    // => "child"
"oxen".Singularize()        // => "ox"
"mice".Singularize()        // => "mouse"
"teeth".Singularize()       // => "tooth"
"feet".Singularize()        // => "foot"
"data".Singularize()        // => "datum"
"criteria".Singularize()    // => "criterion"
```

## The inputIsKnownToBePlural Parameter

By default, `Singularize` assumes the input is plural. If you are not sure whether the input is already singular, pass `false` to avoid incorrect singularization:

```csharp
"man".Singularize(inputIsKnownToBePlural: false) // => "man" (correct)
"men".Singularize(inputIsKnownToBePlural: false) // => "man"
```

When `inputIsKnownToBePlural` is `false`, the method first checks whether the word is already in singular form. If it is, the word is returned unchanged.

## The skipSimpleWords Parameter

When `skipSimpleWords` is `true`, the method skips singularization of simple words that merely end in 's'. This helps avoid incorrectly singularizing words like "traxxas" to "traxxa":

```csharp
"tires".Singularize(skipSimpleWords: true)   // => "tires"
"bodies".Singularize(skipSimpleWords: true)  // => "body"
"traxxas".Singularize(skipSimpleWords: true) // => "traxxas"
```

Words with recognized plural suffixes (like "-ies") are still singularized normally. Only words where the plural form is simply the base word plus 's' are skipped.

## Single Letters

Single-letter inputs are returned unchanged:

```csharp
"s".Singularize() // => "s"
"A".Singularize() // => "A"
```

## Related Topics

- [Pluralization](pluralization.md) - Convert singular words to plural form
- [To Quantity](to-quantity.md) - Combine numbers with properly pluralized words
- [Custom Vocabularies](custom-vocabularies.md) - Add irregular, uncountable, or custom rules
- [Collection Humanization](collection-humanization.md) - Format lists of items
