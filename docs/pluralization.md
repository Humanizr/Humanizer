# Pluralization

Convert singular English words to their plural form, with built-in support for irregular and uncountable words.

## Basic Usage

```csharp
using Humanizer;

"cat".Pluralize()    // => "cats"
"man".Pluralize()    // => "men"
"person".Pluralize() // => "people"
"box".Pluralize()    // => "boxes"
"child".Pluralize()  // => "children"
"goose".Pluralize()  // => "geese"
```

## Irregular Words

The default vocabulary includes common irregular plurals that do not follow standard rules:

```csharp
"person".Pluralize()      // => "people"
"salesperson".Pluralize() // => "salespeople"
"man".Pluralize()         // => "men"
"woman".Pluralize()       // => "women"
"child".Pluralize()       // => "children"
"ox".Pluralize()          // => "oxen"
"mouse".Pluralize()       // => "mice"
"tooth".Pluralize()       // => "teeth"
"foot".Pluralize()        // => "feet"
"datum".Pluralize()       // => "data"
"criterion".Pluralize()   // => "criteria"
```

## Uncountable Words

Words that are the same in singular and plural form are handled automatically:

```csharp
"fish".Pluralize()        // => "fish"
"sheep".Pluralize()       // => "sheep"
"deer".Pluralize()        // => "deer"
"equipment".Pluralize()   // => "equipment"
"information".Pluralize() // => "information"
"rice".Pluralize()        // => "rice"
"species".Pluralize()     // => "species"
"news".Pluralize()        // => "news"
```

## The inputIsKnownToBeSingular Parameter

By default, `Pluralize` assumes the input is singular. If you are not sure whether the input is already plural, pass `false` to avoid double-pluralization:

```csharp
"men".Pluralize()                                // => "mens" (incorrect)
"men".Pluralize(inputIsKnownToBeSingular: false)  // => "men" (correct)

"string".Pluralize(inputIsKnownToBeSingular: false) // => "strings"
```

When `inputIsKnownToBeSingular` is `false`, the method first checks whether the word is already in plural form. If it is, the word is returned unchanged.

## Compound Words

Pluralization also works with compound words that end in an irregular root:

```csharp
"salesperson".Pluralize() // => "salespeople"
"spokesman".Pluralize()   // => "spokesmen"
"node_child".Pluralize()  // => "node_children"
```

## Related Topics

- [Singularization](singularization.md) - Convert plural words to singular form
- [To Quantity](to-quantity.md) - Combine numbers with properly pluralized words
- [Custom Vocabularies](custom-vocabularies.md) - Add irregular, uncountable, or custom rules
- [Collection Humanization](collection-humanization.md) - Format lists of items
