# Inflector Methods

Inflector methods convert strings between common naming conventions used in programming. These extension methods handle transformations like PascalCase, camelCase, snake_case, and kebab-case.

## Basic Usage

```csharp
using Humanizer;

"some_title".Titleize()    // => "Some Title"
"some_title".Pascalize()   // => "SomeTitle"
"some_title".Camelize()    // => "someTitle"
"SomeTitle".Underscore()   // => "some_title"
"some_title".Dasherize()   // => "some-title"
"SomeText".Kebaberize()    // => "some-text"
```

## Titleize

Converts a string to Title Case by humanizing it first and then capitalizing each word. Works with underscores, dashes, and PascalCase input.

```csharp
"some title".Titleize()                      // => "Some Title"
"some-title".Titleize()                      // => "Some Title"
"some-title: The beginning".Titleize()       // => "Some Title: The Beginning"
"some_title:_the_beginning".Titleize()       // => "Some Title: the Beginning"
```

Strings with no recognized letters are preserved as-is:

```csharp
"@@".Titleize()   // => "@@"
"123".Titleize()  // => "123"
```

## Pascalize

Converts a string to PascalCase (UpperCamelCase) by capitalizing the first letter of each word and removing spaces, underscores, and dashes. This is commonly used for class and type names in .NET.

```csharp
"customer".Pascalize()                            // => "Customer"
"customer_name".Pascalize()                       // => "CustomerName"
"customer_first_name".Pascalize()                 // => "CustomerFirstName"
"customer_first_name goes here".Pascalize()       // => "CustomerFirstNameGoesHere"
"customer name".Pascalize()                       // => "CustomerName"
"customer-first-name".Pascalize()                 // => "CustomerFirstName"
```

Strings that are already cased are left unchanged:

```csharp
"CUSTOMER".Pascalize()   // => "CUSTOMER"
"CUStomer".Pascalize()   // => "CUStomer"
```

## Camelize

Converts a string to camelCase (lowerCamelCase). This is the same as Pascalize except the first character is lowercase. Commonly used for variable and parameter names.

```csharp
"customer".Camelize()                            // => "customer"
"customer_name".Camelize()                       // => "customerName"
"customer_first_name".Camelize()                 // => "customerFirstName"
"customer_first_name goes here".Camelize()       // => "customerFirstNameGoesHere"
"customer name".Camelize()                       // => "customerName"
"CUSTOMER".Camelize()                            // => "cUSTOMER"
```

## Underscore

Converts PascalCase, camelCase, spaces, and dashes into lowercase underscore-separated text (snake_case). Commonly used for database column names and some API conventions.

```csharp
"SomeTitle".Underscore()                                  // => "some_title"
"someTitle".Underscore()                                  // => "some_title"
"some title".Underscore()                                 // => "some_title"
"SomeTitleThatWillBeUnderscored".Underscore()             // => "some_title_that_will_be_underscored"
"SomeForeignWordsLikeÄgyptenÑu".Underscore()             // => "some_foreign_words_like_ägypten_ñu"
```

## Dasherize and Hyphenate

Replaces underscores in a string with dashes. `Hyphenate` is an alias for `Dasherize`.

```csharp
"some_title".Dasherize()           // => "some-title"
"some_title_goes_here".Dasherize() // => "some-title-goes-here"

"some_title".Hyphenate()           // => "some-title"
"some_title_goes_here".Hyphenate() // => "some-title-goes-here"
```

Note that `Dasherize` and `Hyphenate` only replace underscores. To convert PascalCase directly to dashes, use `Kebaberize` instead.

## Kebaberize

Converts a string to kebab-case (lowercase words separated by hyphens). This is equivalent to calling `Underscore` followed by `Dasherize`. Commonly used for CSS class names, HTML attributes, and URL slugs.

```csharp
"SomeWords".Kebaberize()                          // => "some-words"
"SOME words TOGETHER".Kebaberize()                // => "some-words-together"
"A spanish word EL niño".Kebaberize()             // => "a-spanish-word-el-niño"
"SomeForeignWords ÆgÑuÄgypten".Kebaberize()      // => "some-foreign-words-æg-ñu-ägypten"
"A VeryShortSENTENCE".Kebaberize()                // => "a-very-short-sentence"
```

## Related Topics

- [String Humanization](string-humanization.md) - Convert programming identifiers to human-readable text
- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [String Transformations](string-transformations.md) - Custom casing and transformations
