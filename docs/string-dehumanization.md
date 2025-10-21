# String Dehumanization

Convert human-friendly strings back to PascalCase format.

## Overview

Dehumanization reverses the humanization process, taking a humanized string and converting it back to PascalCase. This is useful when you need to convert user input or display text back into a format suitable for code identifiers.

## Basic Usage

```csharp
"Pascal case input string is turned into sentence".Dehumanize() 
    // => "PascalCaseInputStringIsTurnedIntoSentence"

"some string".Dehumanize() 
    // => "SomeString"

"Some String".Dehumanize() 
    // => "SomeString"
```

## Behavior

The dehumanization process:
1. Splits the input on spaces
2. Humanizes each word (to handle edge cases)
3. Pascalizes each word (capitalizing first letter)
4. Removes all spaces

If the input is already in PascalCase (contains no spaces), it is returned unchanged.

## Examples

```csharp
"SomeStringAndAnotherString".Dehumanize() 
    // => "SomeStringAndAnotherString" (unchanged)
```

## Related Topics

- [String Humanization](string-humanization.md)
- [Inflector Methods](inflector-methods.md) - Pascalize, Camelize
