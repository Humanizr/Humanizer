## `ByteSizeExtensions`

Provides extension methods for ByteSize
```csharp
public static class Humanizer.ByteSizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `ByteSize` | Bits(this `Byte` input) | Considers input as bits | 
| `ByteSize` | Bits(this `SByte` input) | Considers input as bits | 
| `ByteSize` | Bits(this `Int16` input) | Considers input as bits | 
| `ByteSize` | Bits(this `UInt16` input) | Considers input as bits | 
| `ByteSize` | Bits(this `Int32` input) | Considers input as bits | 
| `ByteSize` | Bits(this `UInt32` input) | Considers input as bits | 
| `ByteSize` | Bits(this `Int64` input) | Considers input as bits | 
| `ByteSize` | Bytes(this `Byte` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `SByte` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `Int16` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `UInt16` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `Int32` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `UInt32` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `Double` input) | Considers input as bytes | 
| `ByteSize` | Bytes(this `Int64` input) | Considers input as bytes | 
| `ByteSize` | Gigabytes(this `Byte` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `SByte` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `Int16` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `UInt16` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `Int32` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `UInt32` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `Double` input) | Considers input as gigabytes | 
| `ByteSize` | Gigabytes(this `Int64` input) | Considers input as gigabytes | 
| `String` | Humanize(this `ByteSize` input, `String` format = null) | Turns a byte quantity into human readable form, eg 2 GB | 
| `String` | Humanize(this `ByteSize` input, `IFormatProvider` formatProvider) | Turns a byte quantity into human readable form, eg 2 GB | 
| `String` | Humanize(this `ByteSize` input, `String` format, `IFormatProvider` formatProvider) | Turns a byte quantity into human readable form, eg 2 GB | 
| `ByteSize` | Kilobytes(this `Byte` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `SByte` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `Int16` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `UInt16` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `Int32` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `UInt32` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `Double` input) | Considers input as kilobytes | 
| `ByteSize` | Kilobytes(this `Int64` input) | Considers input as kilobytes | 
| `ByteSize` | Megabytes(this `Byte` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `SByte` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `Int16` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `UInt16` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `Int32` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `UInt32` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `Double` input) | Considers input as megabytes | 
| `ByteSize` | Megabytes(this `Int64` input) | Considers input as megabytes | 
| `ByteRate` | Per(this `ByteSize` size, `TimeSpan` interval) | Turns a quantity of bytes in a given interval into a rate that can be manipulated | 
| `ByteSize` | Terabytes(this `Byte` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `SByte` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `Int16` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `UInt16` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `Int32` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `UInt32` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `Double` input) | Considers input as terabytes | 
| `ByteSize` | Terabytes(this `Int64` input) | Considers input as terabytes | 


## `CasingExtensions`

ApplyCase method to allow changing the case of a sentence easily
```csharp
public static class Humanizer.CasingExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ApplyCase(this `String` input, `LetterCasing` casing) | Changes the casing of the provided input | 


## `ClockNotationRounding`

```csharp
public enum Humanizer.ClockNotationRounding
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | None |  | 
| `1` | NearestFiveMinutes |  | 


## `CollectionHumanizeExtensions`

Humanizes an IEnumerable into a human readable list
```csharp
public static class Humanizer.CollectionHumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(this `IEnumerable<T>` collection) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 
| `String` | Humanize(this `IEnumerable<T>` collection, `Func<T, String>` displayFormatter) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 
| `String` | Humanize(this `IEnumerable<T>` collection, `Func<T, Object>` displayFormatter) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 
| `String` | Humanize(this `IEnumerable<T>` collection, `String` separator) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 
| `String` | Humanize(this `IEnumerable<T>` collection, `Func<T, String>` displayFormatter, `String` separator) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 
| `String` | Humanize(this `IEnumerable<T>` collection, `Func<T, Object>` displayFormatter, `String` separator) | Formats the collection for display, calling ToString() on each object and  using the default separator for the current culture. | 


## `DateHumanizeExtensions`

Humanizes DateTime into human readable sentence
```csharp
public static class Humanizer.DateHumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(this `DateTime` input, `Nullable<Boolean>` utcDate = null, `Nullable<DateTime>` dateToCompareAgainst = null, `CultureInfo` culture = null) | Turns the current or provided date into a human readable sentence | 
| `String` | Humanize(this `Nullable<DateTime>` input, `Nullable<Boolean>` utcDate = null, `Nullable<DateTime>` dateToCompareAgainst = null, `CultureInfo` culture = null) | Turns the current or provided date into a human readable sentence | 
| `String` | Humanize(this `DateTimeOffset` input, `Nullable<DateTimeOffset>` dateToCompareAgainst = null, `CultureInfo` culture = null) | Turns the current or provided date into a human readable sentence | 
| `String` | Humanize(this `Nullable<DateTimeOffset>` input, `Nullable<DateTimeOffset>` dateToCompareAgainst = null, `CultureInfo` culture = null) | Turns the current or provided date into a human readable sentence | 


## `DateToOrdinalWordsExtensions`

Humanizes DateTime into human readable sentence
```csharp
public static class Humanizer.DateToOrdinalWordsExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToOrdinalWords(this `DateTime` input) | Turns the provided date into ordinal words | 
| `String` | ToOrdinalWords(this `DateTime` input, `GrammaticalCase` grammaticalCase) | Turns the provided date into ordinal words | 


## `EnglishArticle`

Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article.
```csharp
public static class Humanizer.EnglishArticle

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String[]` | AppendArticlePrefix(`String[]` items) | Removes the prefixed article and appends it to the same string. | 
| `String[]` | PrependArticleSuffix(`String[]` appended) | Removes the previously appended article and prepends it to the same string. | 


## `EnglishArticles`

Definite and Indefinite English Articles
```csharp
public enum Humanizer.EnglishArticles
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | A | A | 
| `1` | An | An | 
| `2` | The | The | 


## `EnumDehumanizeExtensions`

Contains extension methods for dehumanizing Enum string values.
```csharp
public static class Humanizer.EnumDehumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `TTargetEnum` | DehumanizeTo(this `String` input) | Dehumanizes a string into the Enum it was originally Humanized from! | 
| `Enum` | DehumanizeTo(this `String` input, `Type` targetEnum, `OnNoMatch` onNoMatch = ThrowsException) | Dehumanizes a string into the Enum it was originally Humanized from! | 


## `EnumHumanizeExtensions`

Contains extension methods for humanizing Enums
```csharp
public static class Humanizer.EnumHumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(this `Enum` input) | Turns an enum member into a human readable string; e.g. AnonymousUser -&gt; Anonymous user. It also honors DescriptionAttribute data annotation | 
| `String` | Humanize(this `Enum` input, `LetterCasing` casing) | Turns an enum member into a human readable string; e.g. AnonymousUser -&gt; Anonymous user. It also honors DescriptionAttribute data annotation | 


## `GrammaticalCase`

Options for specifying the desired grammatical case for the output words
```csharp
public enum Humanizer.GrammaticalCase
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Nominative | Indicates the subject of a finite verb | 
| `1` | Genitive | Indicates the possessor of another noun | 
| `2` | Dative | Indicates the indirect object of a verb | 
| `3` | Accusative | Indicates the direct object of a verb | 
| `4` | Instrumental | Indicates an object used in performing an action | 
| `5` | Prepositional | Indicates the object of a preposition | 


## `GrammaticalGender`

Options for specifying the desired grammatical gender for the output words
```csharp
public enum Humanizer.GrammaticalGender
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Masculine | Indicates masculine grammatical gender | 
| `1` | Feminine | Indicates feminine grammatical gender | 
| `2` | Neuter | Indicates neuter grammatical gender | 


## `HeadingExtensions`

Contains extensions to transform a number indicating a heading into the  textual representation of the heading.
```csharp
public static class Humanizer.HeadingExtensions

```

Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `Char[]` | headingArrows |  | 
| `String[]` | headings |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Double` | FromAbbreviatedHeading(this `String` heading) | Returns a heading based on the short textual representation of the heading. | 
| `Double` | FromAbbreviatedHeading(this `String` heading, `CultureInfo` culture = null) | Returns a heading based on the short textual representation of the heading. | 
| `Double` | FromHeadingArrow(this `Char` heading) | Returns a heading based on the heading arrow. | 
| `Double` | FromHeadingArrow(this `String` heading) | Returns a heading based on the heading arrow. | 
| `String` | ToHeading(this `Double` heading, `HeadingStyle` style = Abbreviated, `CultureInfo` culture = null) | Returns a textual representation of the heading.    This representation has a maximum deviation of 11.25 degrees. | 
| `Char` | ToHeadingArrow(this `Double` heading) | Returns a char arrow indicating the heading.    This representation has a maximum deviation of 22.5 degrees. | 


## `HeadingStyle`

Style for the cardinal direction humanization
```csharp
public enum Humanizer.HeadingStyle
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Abbreviated | Returns an abbreviated format | 
| `1` | Full | Returns the full format | 


## `ICulturedStringTransformer`

Can transform a string with the given culture
```csharp
public interface Humanizer.ICulturedStringTransformer
    : IStringTransformer

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Transform(`String` input, `CultureInfo` culture) | Transform the input | 


## `In`

```csharp
public class Humanizer.In

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `DateTime` | April | Returns 1st of April of the current year | 
| `DateTime` | August | Returns 1st of August of the current year | 
| `DateTime` | December | Returns 1st of December of the current year | 
| `DateTime` | February | Returns 1st of February of the current year | 
| `DateTime` | January | Returns 1st of January of the current year | 
| `DateTime` | July | Returns 1st of July of the current year | 
| `DateTime` | June | Returns 1st of June of the current year | 
| `DateTime` | March | Returns 1st of March of the current year | 
| `DateTime` | May | Returns 1st of May of the current year | 
| `DateTime` | November | Returns 1st of November of the current year | 
| `DateTime` | October | Returns 1st of October of the current year | 
| `DateTime` | September | Returns 1st of September of the current year | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `DateTime` | AprilOf(`Int32` year) | Returns 1st of April of the year passed in | 
| `DateTime` | AugustOf(`Int32` year) | Returns 1st of August of the year passed in | 
| `DateTime` | DecemberOf(`Int32` year) | Returns 1st of December of the year passed in | 
| `DateTime` | FebruaryOf(`Int32` year) | Returns 1st of February of the year passed in | 
| `DateTime` | JanuaryOf(`Int32` year) | Returns 1st of January of the year passed in | 
| `DateTime` | JulyOf(`Int32` year) | Returns 1st of July of the year passed in | 
| `DateTime` | JuneOf(`Int32` year) | Returns 1st of June of the year passed in | 
| `DateTime` | MarchOf(`Int32` year) | Returns 1st of March of the year passed in | 
| `DateTime` | MayOf(`Int32` year) | Returns 1st of May of the year passed in | 
| `DateTime` | NovemberOf(`Int32` year) | Returns 1st of November of the year passed in | 
| `DateTime` | OctoberOf(`Int32` year) | Returns 1st of October of the year passed in | 
| `DateTime` | SeptemberOf(`Int32` year) | Returns 1st of September of the year passed in | 
| `DateTime` | TheYear(`Int32` year) | Returns the first of January of the provided year | 


## `InflectorExtensions`

Inflector extensions
```csharp
public static class Humanizer.InflectorExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Camelize(this `String` input) | Same as Pascalize except that the first character is lower case | 
| `String` | Dasherize(this `String` underscoredWord) | Replaces underscores with dashes in the string | 
| `String` | Hyphenate(this `String` underscoredWord) | Replaces underscores with hyphens in the string | 
| `String` | Kebaberize(this `String` input) | Separates the input words with hyphens and all the words are converted to lowercase | 
| `String` | Pascalize(this `String` input) | By default, pascalize converts strings to UpperCamelCase also removing underscores | 
| `String` | Pluralize(this `String` word, `Boolean` inputIsKnownToBeSingular = True) | Pluralizes the provided input considering irregular words | 
| `String` | Singularize(this `String` word, `Boolean` inputIsKnownToBePlural = True, `Boolean` skipSimpleWords = False) | Singularizes the provided input considering irregular words | 
| `String` | Titleize(this `String` input) | Humanizes the input with Title casing | 
| `String` | Underscore(this `String` input) | Separates the input words with underscore | 


## `IStringTransformer`

Can transform a string
```csharp
public interface Humanizer.IStringTransformer

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Transform(`String` input) | Transform the input | 


## `ITruncator`

Can truncate a string.
```csharp
public interface Humanizer.ITruncator

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Truncate(`String` value, `Int32` length, `String` truncationString, `TruncateFrom` truncateFrom = Right) | Truncate a string | 


## `LetterCasing`

Options for specifying the desired letter casing for the output string
```csharp
public enum Humanizer.LetterCasing
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Title | SomeString -&gt; Some String | 
| `1` | AllCaps | SomeString -&gt; SOME STRING | 
| `2` | LowerCase | SomeString -&gt; some string | 
| `3` | Sentence | SomeString -&gt; Some string | 


## `MetricNumeralExtensions`

Contains extension methods for changing a number to Metric representation (ToMetric)  and from Metric representation back to the number (FromMetric)
```csharp
public static class Humanizer.MetricNumeralExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Double` | <BuildMetricNumber>g__getExponent|12_0(`List<Char>` symbols, `<>c__DisplayClass12_0&`  = null) |  | 
| `Boolean` | <IsOutOfRange>g__outside|17_0(`Double` min, `Double` max, `<>c__DisplayClass17_0&`  = null) |  | 
| `Double` | FromMetric(this `String` input) | Converts a Metric representation into a number. | 
| `String` | ToMetric(this `Int32` input, `Nullable<MetricNumeralFormats>` formats = null, `Nullable<Int32>` decimals = null) | Converts a number into a valid and Human-readable Metric representation. | 
| `String` | ToMetric(this `Double` input, `Nullable<MetricNumeralFormats>` formats = null, `Nullable<Int32>` decimals = null) | Converts a number into a valid and Human-readable Metric representation. | 


## `MetricNumeralFormats`

Flags for formatting the metric representation of numerals.
```csharp
public enum Humanizer.MetricNumeralFormats
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `1` | UseLongScaleWord | Use the metric prefix <a href="https://en.wikipedia.org/wiki/Long_and_short_scales">long scale word</a>. | 
| `2` | UseName | Use the metric prefix <a href="https://en.wikipedia.org/wiki/Metric_prefix#List_of_SI_prefixes">name</a> instead of the symbol. | 
| `4` | UseShortScaleWord | Use the metric prefix <a href="https://en.wikipedia.org/wiki/Long_and_short_scales">short scale word</a>. | 
| `8` | WithSpace | Include a space after the numeral. | 


## `NoMatchFoundException`

This is thrown on String.DehumanizeTo enum when the provided string cannot be mapped to the target enum
```csharp
public class Humanizer.NoMatchFoundException
    : Exception, ISerializable, _Exception

```

## `NumberToNumberExtensions`

Number to Number extensions
```csharp
public static class Humanizer.NumberToNumberExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | Billions(this `Int32` input) | 1.Billions() == 1000000000 (short scale) | 
| `UInt32` | Billions(this `UInt32` input) | 1.Billions() == 1000000000 (short scale) | 
| `Int64` | Billions(this `Int64` input) | 1.Billions() == 1000000000 (short scale) | 
| `UInt64` | Billions(this `UInt64` input) | 1.Billions() == 1000000000 (short scale) | 
| `Double` | Billions(this `Double` input) | 1.Billions() == 1000000000 (short scale) | 
| `Int32` | Hundreds(this `Int32` input) | 4.Hundreds() == 400 | 
| `UInt32` | Hundreds(this `UInt32` input) | 4.Hundreds() == 400 | 
| `Int64` | Hundreds(this `Int64` input) | 4.Hundreds() == 400 | 
| `UInt64` | Hundreds(this `UInt64` input) | 4.Hundreds() == 400 | 
| `Double` | Hundreds(this `Double` input) | 4.Hundreds() == 400 | 
| `Int32` | Millions(this `Int32` input) | 2.Millions() == 2000000 | 
| `UInt32` | Millions(this `UInt32` input) | 2.Millions() == 2000000 | 
| `Int64` | Millions(this `Int64` input) | 2.Millions() == 2000000 | 
| `UInt64` | Millions(this `UInt64` input) | 2.Millions() == 2000000 | 
| `Double` | Millions(this `Double` input) | 2.Millions() == 2000000 | 
| `Int32` | Tens(this `Int32` input) | 5.Tens == 50 | 
| `UInt32` | Tens(this `UInt32` input) | 5.Tens == 50 | 
| `Int64` | Tens(this `Int64` input) | 5.Tens == 50 | 
| `UInt64` | Tens(this `UInt64` input) | 5.Tens == 50 | 
| `Double` | Tens(this `Double` input) | 5.Tens == 50 | 
| `Int32` | Thousands(this `Int32` input) | 3.Thousands() == 3000 | 
| `UInt32` | Thousands(this `UInt32` input) | 3.Thousands() == 3000 | 
| `Int64` | Thousands(this `Int64` input) | 3.Thousands() == 3000 | 
| `UInt64` | Thousands(this `UInt64` input) | 3.Thousands() == 3000 | 
| `Double` | Thousands(this `Double` input) | 3.Thousands() == 3000 | 


## `NumberToTimeSpanExtensions`

Number to TimeSpan extensions
```csharp
public static class Humanizer.NumberToTimeSpanExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `TimeSpan` | Days(this `Byte` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `SByte` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `Int16` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `UInt16` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `Int32` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `UInt32` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `Int64` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `UInt64` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Days(this `Double` days) | 2.Days() == TimeSpan.FromDays(2) | 
| `TimeSpan` | Hours(this `Byte` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `SByte` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `Int16` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `UInt16` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `Int32` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `UInt32` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `Int64` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `UInt64` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Hours(this `Double` hours) | 3.Hours() == TimeSpan.FromHours(3) | 
| `TimeSpan` | Milliseconds(this `Byte` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `SByte` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `Int16` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `UInt16` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `Int32` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `UInt32` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `Int64` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `UInt64` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Milliseconds(this `Double` ms) | 5.Milliseconds() == TimeSpan.FromMilliseconds(5) | 
| `TimeSpan` | Minutes(this `Byte` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `SByte` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `Int16` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `UInt16` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `Int32` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `UInt32` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `Int64` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `UInt64` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Minutes(this `Double` minutes) | 4.Minutes() == TimeSpan.FromMinutes(4) | 
| `TimeSpan` | Seconds(this `Byte` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `SByte` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `Int16` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `UInt16` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `Int32` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `UInt32` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `Int64` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `UInt64` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Seconds(this `Double` seconds) | 5.Seconds() == TimeSpan.FromSeconds(5) | 
| `TimeSpan` | Weeks(this `Byte` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `SByte` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `Int16` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `UInt16` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `Int32` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `UInt32` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `Int64` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `UInt64` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 
| `TimeSpan` | Weeks(this `Double` input) | 2.Weeks() == new TimeSpan(14, 0, 0, 0) | 


## `NumberToWordsExtension`

Transform a number into words; e.g. 1 =&gt; one
```csharp
public static class Humanizer.NumberToWordsExtension

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToOrdinalWords(this `Int32` number, `CultureInfo` culture = null) | 1.ToOrdinalWords() -&gt; "first" | 
| `String` | ToOrdinalWords(this `Int32` number, `WordForm` wordForm, `CultureInfo` culture = null) | 1.ToOrdinalWords() -&gt; "first" | 
| `String` | ToOrdinalWords(this `Int32` number, `GrammaticalGender` gender, `CultureInfo` culture = null) | 1.ToOrdinalWords() -&gt; "first" | 
| `String` | ToOrdinalWords(this `Int32` number, `GrammaticalGender` gender, `WordForm` wordForm, `CultureInfo` culture = null) | 1.ToOrdinalWords() -&gt; "first" | 
| `String` | ToTuple(this `Int32` number, `CultureInfo` culture = null) | 1.ToTuple() -&gt; "single" | 
| `String` | ToWords(this `Int32` number, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int32` number, `WordForm` wordForm, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int32` number, `Boolean` addAnd, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int32` number, `Boolean` addAnd, `WordForm` wordForm, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int32` number, `GrammaticalGender` gender, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int32` number, `WordForm` wordForm, `GrammaticalGender` gender, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int64` number, `CultureInfo` culture = null, `Boolean` addAnd = True) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int64` number, `WordForm` wordForm, `CultureInfo` culture = null, `Boolean` addAnd = False) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int64` number, `GrammaticalGender` gender, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 
| `String` | ToWords(this `Int64` number, `WordForm` wordForm, `GrammaticalGender` gender, `CultureInfo` culture = null) | 3501.ToWords() -&gt; "three thousand five hundred and one" | 


## `On`

```csharp
public class Humanizer.On

```

## `OnNoMatch`

Dictating what should be done when a match is not found - currently used only for DehumanizeTo
```csharp
public enum Humanizer.OnNoMatch
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | ThrowsException | This is the default behavior which throws a NoMatchFoundException | 
| `1` | ReturnsNull | If set to ReturnsNull the method returns null instead of throwing an exception | 


## `OrdinalizeExtensions`

Ordinalize extensions
```csharp
public static class Humanizer.OrdinalizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Ordinalize(this `String` numberString) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `CultureInfo` culture) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `CultureInfo` culture, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `GrammaticalGender` gender) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `GrammaticalGender` gender, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `GrammaticalGender` gender, `CultureInfo` culture) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `String` numberString, `GrammaticalGender` gender, `CultureInfo` culture, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `CultureInfo` culture) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `CultureInfo` culture, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `GrammaticalGender` gender) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `GrammaticalGender` gender, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `GrammaticalGender` gender, `CultureInfo` culture) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 
| `String` | Ordinalize(this `Int32` number, `GrammaticalGender` gender, `CultureInfo` culture, `WordForm` wordForm) | Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th. | 


## `Plurality`

Provides hint for Humanizer as to whether a word is singular, plural or with unknown plurality
```csharp
public enum Humanizer.Plurality
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Singular | The word is singular | 
| `1` | Plural | The word is plural | 
| `2` | CouldBeEither | I am unsure of the plurality | 


## `PrepositionsExtensions`

`System.DateTime` extensions related to spatial or temporal relations
```csharp
public static class Humanizer.PrepositionsExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `DateTime` | At(this `DateTime` date, `Int32` hour, `Int32` min = 0, `Int32` second = 0, `Int32` millisecond = 0) | Returns a new `System.DateTime` with the specifed hour and, optionally  provided minutes, seconds, and milliseconds. | 
| `DateTime` | AtMidnight(this `DateTime` date) | Returns a new instance of DateTime based on the provided date where the time is set to midnight | 
| `DateTime` | AtNoon(this `DateTime` date) | Returns a new instance of DateTime based on the provided date where the time is set to noon | 
| `DateTime` | In(this `DateTime` date, `Int32` year) | Returns a new instance of DateTime based on the provided date where the year is set to the provided year | 


## `RomanNumeralExtensions`

Contains extension methods for changing a number to Roman representation (ToRoman) and from Roman representation back to the number (FromRoman)
```csharp
public static class Humanizer.RomanNumeralExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int32` | FromRoman(this `String` input) | Converts Roman numbers into integer | 
| `String` | ToRoman(this `Int32` input) | Converts the input to Roman number | 


## `ShowQuantityAs`

Enumerates the ways of displaying a quantity value when converting  a word to a quantity string.
```csharp
public enum Humanizer.ShowQuantityAs
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | None | Indicates that no quantity will be included in the formatted string. | 
| `1` | Numeric | Indicates that the quantity will be included in the output, formatted  as its numeric value (e.g. "1"). | 
| `2` | Words | Incidates that the quantity will be included in the output, formatted as  words (e.g. 123 =&gt; "one hundred and twenty three"). | 


## `StringDehumanizeExtensions`

Contains extension methods for dehumanizing strings.
```csharp
public static class Humanizer.StringDehumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Dehumanize(this `String` input) | Dehumanizes a string; e.g. 'some string', 'Some String', 'Some string' -&gt; 'SomeString'  If a string is already dehumanized then it leaves it alone 'SomeStringAndAnotherString' -&gt; 'SomeStringAndAnotherString' | 


## `StringExtensions`

Extension methods for String type.
```csharp
public static class Humanizer.StringExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | FormatWith(this `String` format, `Object[]` args) | Extension method to format string with passed arguments. Current thread's current culture is used | 
| `String` | FormatWith(this `String` format, `IFormatProvider` provider, `Object[]` args) | Extension method to format string with passed arguments. Current thread's current culture is used | 


## `StringHumanizeExtensions`

Contains extension methods for humanizing string values.
```csharp
public static class Humanizer.StringHumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(this `String` input) | Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -&gt; 'Underscored input String is turned INTO sentence' | 
| `String` | Humanize(this `String` input, `LetterCasing` casing) | Humanizes the input string; e.g. Underscored_input_String_is_turned_INTO_sentence -&gt; 'Underscored input String is turned INTO sentence' | 


## `TimeSpanHumanizeExtensions`

Humanizes TimeSpan into human readable form
```csharp
public static class Humanizer.TimeSpanHumanizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(this `TimeSpan` timeSpan, `Int32` precision = 1, `CultureInfo` culture = null, `TimeUnit` maxUnit = Week, `TimeUnit` minUnit = Millisecond, `String` collectionSeparator = , , `Boolean` toWords = False) | Turns a TimeSpan into a human readable form. E.g. 1 day. | 
| `String` | Humanize(this `TimeSpan` timeSpan, `Int32` precision, `Boolean` countEmptyUnits, `CultureInfo` culture = null, `TimeUnit` maxUnit = Week, `TimeUnit` minUnit = Millisecond, `String` collectionSeparator = , , `Boolean` toWords = False) | Turns a TimeSpan into a human readable form. E.g. 1 day. | 


## `TimeUnitToSymbolExtensions`

Transform a time unit into a symbol; e.g. [Humanizer.Localisation.TimeUnit.Year](Humanizer.Localisation.TimeUnit#year) =&gt; "a"
```csharp
public static class Humanizer.TimeUnitToSymbolExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToSymbol(this `TimeUnit` unit, `CultureInfo` culture = null) | TimeUnit.Day.ToSymbol() -&gt; "d" | 


## `To`

A portal to string transformation using IStringTransformer
```csharp
public static class Humanizer.To

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ICulturedStringTransformer` | LowerCase | Changes the string to lower case | 
| `ICulturedStringTransformer` | SentenceCase | Changes the string to sentence case | 
| `ICulturedStringTransformer` | TitleCase | Changes string to title case | 
| `ICulturedStringTransformer` | UpperCase | Changes the string to upper case | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Transform(this `String` input, `IStringTransformer[]` transformers) | Transforms a string using the provided transformers. Transformations are applied in the provided order. | 
| `String` | Transform(this `String` input, `CultureInfo` culture, `ICulturedStringTransformer[]` transformers) | Transforms a string using the provided transformers. Transformations are applied in the provided order. | 


## `ToQuantityExtensions`

Provides extensions for formatting a `System.String` word as a quantity.
```csharp
public static class Humanizer.ToQuantityExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | ToQuantity(this `String` input, `Int32` quantity, `ShowQuantityAs` showQuantityAs = Numeric) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 
| `String` | ToQuantity(this `String` input, `Int32` quantity, `String` format, `IFormatProvider` formatProvider = null) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 
| `String` | ToQuantity(this `String` input, `Int64` quantity, `ShowQuantityAs` showQuantityAs = Numeric) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 
| `String` | ToQuantity(this `String` input, `Int64` quantity, `String` format, `IFormatProvider` formatProvider = null) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 
| `String` | ToQuantity(this `String` input, `Double` quantity, `String` format = null, `IFormatProvider` formatProvider = null) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 
| `String` | ToQuantity(this `String` input, `Double` quantity) | Prefixes the provided word with the number and accordingly pluralizes or singularizes the word | 


## `TruncateExtensions`

Allow strings to be truncated
```csharp
public static class Humanizer.TruncateExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Truncate(this `String` input, `Int32` length) | Truncate the string | 
| `String` | Truncate(this `String` input, `Int32` length, `ITruncator` truncator, `TruncateFrom` from = Right) | Truncate the string | 
| `String` | Truncate(this `String` input, `Int32` length, `String` truncationString, `TruncateFrom` from = Right) | Truncate the string | 
| `String` | Truncate(this `String` input, `Int32` length, `String` truncationString, `ITruncator` truncator, `TruncateFrom` from = Right) | Truncate the string | 


## `TruncateFrom`

Truncation location for humanizer
```csharp
public enum Humanizer.TruncateFrom
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Left | Truncate letters from the left (start) of the string | 
| `1` | Right | Truncate letters from the right (end) of the string | 


## `Truncator`

Gets a ITruncator
```csharp
public static class Humanizer.Truncator

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ITruncator` | FixedLength | Fixed length truncator | 
| `ITruncator` | FixedNumberOfCharacters | Fixed number of characters truncator | 
| `ITruncator` | FixedNumberOfWords | Fixed number of words truncator | 


## `TupleizeExtensions`

Convert int to named tuple strings (1 -&gt; 'single', 2-&gt; 'double' etc.).  Only values 1-10, 100, and 1000 have specific names. All others will return 'n-tuple'.
```csharp
public static class Humanizer.TupleizeExtensions

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Tupleize(this `Int32` input) | Converts integer to named tuple (e.g. 'single', 'double' etc.). | 


## `WordForm`

Options for specifying the form of the word when different variations of the same word exists.
```csharp
public enum Humanizer.WordForm
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Normal | Indicates the normal form of a written word. | 
| `1` | Abbreviation | Indicates the shortened form of a written word | 


