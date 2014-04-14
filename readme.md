Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

###Table of contents
 - [Install](#install)
 - [Features](#features)
   - [Humanize String](#humanize-string)
   - [Dehumanize String](#dehumanize-string)
   - [Transform String](#transform-string)
   - [Truncate String](#truncate-string)
   - [Format String](#format-string)  
   - [Humanize Enums](#humanize-enums)
   - [Dehumanize Enums](#dehumanize-enums)
   - [Humanize DateTime](#humanize-datetime)
   - [Humanize TimeSpan](#humanize-timespan)
   - [Inflector methods](#inflector-methods)
     - [Pluralize](#pluralize)
     - [Singularize](#singularize)
     - [ToQuantity](#toquantity)
     - [Ordinalize](#ordinalize)
     - [Underscore](#underscore)
     - [Dasherize & Hyphenate](#dasherize--hyphenate)
   - [Fluent date](#fluent-date)
   - [Number to words](#number-to-words)
   - [Number to ordinal words](#number-to-ordinal-words)
   - [Roman numerals](#roman-numerals)
   - [ByteSize](#bytesize)
 - [Mix this into your framework to simplify your life](#mix-this-into-your-framework-to-simplify-your-life)
 - [How to contribute?](#how-to-contribute)
   - [Contribution guideline](#contribution-guideline)
   - [Need your help with localisation](#need-your-help-with-localisation)
 - [Continuous Integration from TeamCity](#continuous-integration-from-teamcity)
 - [Author](#author)
 - [Main contributors](#main-contributors)
 - [License](#license)

##<a id="install">Install</a>
You can install Humanizer as [a nuget package](https://nuget.org/packages/Humanizer): `Install-Package Humanizer`

Humanizer is a Portable Class Library with support for .Net 4+, SilverLight 5, Windows Phone 8 and Win Store applications. 
Also Humanizer [symbols nuget package](http://www.symbolsource.org/Public/Metadata/NuGet/Project/Humanizer) is published so you can [step through Humanizer code](http://www.symbolsource.org/Public/Home/VisualStudio) while debugging your code.

##<a id="features">Features</a>

###<a id="humanize-string">Humanize String</a>
`Humanize` string extensions allow you turn an otherwise computerized string into a more readable human-friendly one. 
The foundation of this was set in the [BDDfy framework](https://github.com/TestStack/TestStack.BDDfy) where class names, method names and properties are turned into human readable sentences. 

```C#
"PascalCaseInputStringIsTurnedIntoSentence".Humanize() => "Pascal case input string is turned into sentence"
    
"Underscored_input_string_is_turned_into_sentence".Humanize() => "Underscored input string is turned into sentence"
    
"Underscored_input_String_is_turned_INTO_sentence".Humanize() => "Underscored input String is turned INTO sentence"
    
// acronyms are left intact
"HTML".Humanize() => "HTML" 
```

You may also specify the desired letter casing:

```C#
"CanReturnTitleCase".Humanize(LetterCasing.Title) => "Can Return Title Case"
    
"Can_return_title_Case".Humanize(LetterCasing.Title) => "Can Return Title Case"
    
"CanReturnLowerCase".Humanize(LetterCasing.LowerCase) => "can return lower case"
    
"CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps) => "CAN HUMANIZE INTO UPPER CASE"
```

 > The `LetterCasing` API and the methods accepting it are legacy from V0.2 era and will be deprecated in the future. Instead of that, you can use `Transform` method explained below.

###<a id="dehumanize-string">Dehumanize String</a>
Much like you can humanize a computer friendly into human friendly string you can dehumanize a human friendly string into a computer friendly one:

```C#
"Pascal case input string is turned into sentence".Dehumanize() => "PascalCaseInputStringIsTurnedIntoSentence"
```

###<a id="transform-string">Transform String</a>
There is a `Transform` method that supercedes `LetterCasing`, `ApplyCase` and `Humanize` overloads that accept `LetterCasing`. 
Transform method signatue is as follows:

```C#
string Transform(this string input, params IStringTransformer[] transformers)
```

And there are some out of the box implemenations of `IStringTransformer` for letter casing:

```C#
"Sentence casing".Transform(To.LowerCase) => "sentence casing"
"Sentence casing".Transform(To.SentenceCase) => "Sentence casing"
"Sentence casing".Transform(To.TitleCase) => "Sentence Casing"
"Sentence casing".Transform(To.UpperCase) => "SENTENCE CASING"
```

`LowerCase` is a public static property on `To` class that returns an instance of private `ToLowerCase` class that implements `IStringTransformer` and knows how to turn a string into lower case.

The benefit of using `Transform` and `IStringTransformer` over `ApplyCase` and `LetterCasing` is that `LetterCasing` is an enum and you're limited to use what's in the framework
while `IStringTransformer` is an interface you can implement in your codebase once and use it with `Transform` method allowing for easy extension. 

###<a id="truncate-string">Truncate String</a>
You can truncate a `string` using the `Truncate` method:

```c#
"Long text to truncate".Truncate(10) => "Long text…"
```

By default the `'…'` character is used to truncate strings. The advantage of using the `'…'` character instead of `"..."` is that the former only takes a single character and thus allows more text to be shown before truncation. If you want, you can also provide your own truncation string:

```c#
"Long text to truncate".Truncate(10, "---") => "Long te---"
```

The default truncation strategy, `Truncator.FixedLength`, is to truncate the input string to a specific length, including the truncation string length. There are two more truncator strategies available: one for a fixed number of (alpha-numerical) characters and one for a fixed number of words. To use a specific truncator when truncating, the two `Truncate` methods shown in the previous examples both have an overload that allow you to specify the `ITruncator` instance to use for the truncation. Here are examples on how to use the three provided truncators:

```c#
"Long text to truncate".Truncate(10, Truncator.FixedLength) => "Long text…"
"Long text to truncate".Truncate(10, "---", Truncator.FixedLength) => "Long te---"

"Long text to truncate".Truncate(6, Truncator.FixedNumberOfCharacters) => "Long t…"
"Long text to truncate".Truncate(6, "---", Truncator.FixedNumberOfCharacters) => "Lon---"

"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords) => "Long text…"
"Long text to truncate".Truncate(2, "---", Truncator.FixedNumberOfWords) => "Long text---"
```

Note that you can also use create your own truncator by having a class implement the `ITruncator` interface.

###<a id="format-string">Format String</a>
You can format a `string` using the `FormatWith()` method:

```c#
"To be formatted -> {0}/{1}.".FormatWith(1, "A") => "To be formated -> 1/A."
```

This is an extension method based on `String.Format`, so exact rules applies to it.
If `format` is null, it'll throw `ArgumentNullException`.
If passed a fewer number for arguments, it'll throw `String.FormatException` exception.

###<a id="humanize-enums">Humanize Enums</a>
Calling `ToString` directly on enum members usually results in less than ideal output for users. The solution to this is usually to use `DescriptionAttribute` data annotation and then read that at runtime to get a more friendly output. That is a great solution; but more often than not we only need to put some space between words of an enum member - which is what `String.Humanize()` does well. For an enum like:

```C#
public enum EnumUnderTest
{
    [Description("Custom description")]
    MemberWithDescriptionAttribute,
    MemberWithoutDescriptionAttribute,
    ALLCAPITALS
}
```

You will get:

```C#
// DescriptionAttribute is honored
EnumUnderTest.MemberWithDescriptionAttribute.Humanize() => "Custom description"
    
// In the absence of Description attribute string.Humanizer kicks in
EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize() => "Member without description attribute" 
    
// Of course you can still apply letter casing 
EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize().Transform(To.TitleCase) => "Member Without Description Attribute"
```

You are not limited to `DescriptionAttribute` for custom description. Any attribute applied on enum members with a `string Description` property counts. 
This is to help with platforms with missing `DescriptionAttribute` and also for allowing subclasses of the `DescriptionAttribute`.

Hopefully this will help avoid littering enums with unnecessary attributes!

###<a id="dehumanize-enums">Dehumanize Enums</a>
Dehumanizes a string into the Enum it was originally Humanized from! The API looks like:

```C#
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input) 
```

And the usage is:

```C#
"Member without description attribute".DehumanizeTo<EnumUnderTest>() => EnumUnderTest.MemberWithoutDescriptionAttribute
```

And just like the Humanize API it honors the `Description` attribute. You don't have to provide the casing you provided during humanization: it figures it out.

There is also a non-generic counterpart for when the original Enum is not known at compile time:

```C#
public static Enum DehumanizeTo(this string input, Type targetEnum, NoMatch onNoMatch = NoMatch.ThrowsException) 
```

which can be used like:

```C#
"Member without description attribute".DehumanizeTo(typeof(EnumUnderTest)) => EnumUnderTest.MemberWithoutDescriptionAttribute
```

By default both methods throw a `NoMatchFoundException` when they cannot match the provided input against the target enum. 
In the non-generic method you can also ask the method to return null by setting the second optiona parameter to `NoMatch.ReturnsNull`.

###<a id="humanize-datetime">Humanize DateTime</a>
You can `Humanize` an instance of `DateTime` and get back a string telling how far back or forward in time that is:

```C#
DateTime.UtcNow.AddHours(-30).Humanize() => "yesterday"
DateTime.UtcNow.AddHours(-2).Humanize() => "2 hours ago"

DateTime.UtcNow.AddHours(30).Humanize() => "tomorrow"
DateTime.UtcNow.AddHours(2).Humanize() => "2 hours from now"
```

Humanizer supports local as well as UTC dates. You could also provide the date you want the input date to be compared against. If null, it will use the current date as comparison base. Here is the API signature:

```C#
public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null)
```

Many localisations are availalbe for this method. Here is a few examples:

```C#
// In ar culture
DateTime.UtcNow.AddDays(-1).Humanize() => "أمس"
DateTime.UtcNow.AddDays(-2).Humanize() => "منذ يومين"
DateTime.UtcNow.AddDays(-3).Humanize() => "منذ 3 أيام"
DateTime.UtcNow.AddDays(-11).Humanize() => "منذ 11 يوم"

// In ru-RU culture
DateTime.UtcNow.AddMinutes(-1).Humanize() => "минуту назад"
DateTime.UtcNow.AddMinutes(-2).Humanize() => "2 минуты назад"
DateTime.UtcNow.AddMinutes(-10).Humanize() => "10 минут назад"
DateTime.UtcNow.AddMinutes(-21).Humanize() => "21 минуту назад"
DateTime.UtcNow.AddMinutes(-22).Humanize() => "22 минуты назад"
DateTime.UtcNow.AddMinutes(-40).Humanize() => "40 минут назад"
```

There are two strategies for `DateTime.Humanize`: the default one as seen above and a precision based one. 
To use the precision based strategy you need to configure it: 

`Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision = .75)`.

The default precision is set to .75 but you can pass your desired precision too. With precision set to 0.75:

```C#
44 seconds => 44 seconds ago/from now
45 seconds => one minute ago/from now
104 seconds => one minute ago/from now
105 seconds => two minutes ago/from now

25 days => a month ago/from now
```

**No dehumanization for dates as `Humanize` is a lossy transformation and the human friendly date is not reversible**

###<a id="humanize-timespan">Humanize TimeSpan</a>
You can call `Humanize` on a `TimeSpan` to a get human friendly representation for it:

```C#
TimeSpan.FromMilliseconds(1).Humanize() => "1 millisecond"
TimeSpan.FromMilliseconds(2).Humanize() => "2 milliseconds"
TimeSpan.FromDays(1).Humanize() => "1 day"
TimeSpan.FromDays(16).Humanize() => "2 weeks"
```

There is an optional `precision` parameter for `TimeSpan.Humanize` which allows you to specify the precision of the returned value. 
The default value of `precision` is 1 which means only the largest time unit is returned like you saw in `TimeSpan.FromDays(16).Humanize()`.
Here is a few examples of specifying precision:

```C#
TimeSpan.FromDays(1).Humanize(precision:2) => "1 day" // no difference when there is only on unit in the provided TimeSpan
TimeSpan.FromDays(16).Humanize(2) => "2 weeks, 2 days"

// the same TimeSpan value with different precision returns different results
TimeSpan.FromMilliseconds(1299630020).Humanize() => "2 weeks"
TimeSpan.FromMilliseconds(1299630020).Humanize(3) => "2 weeks, 1 day, 1 hour"
TimeSpan.FromMilliseconds(1299630020).Humanize(4) => "2 weeks, 1 day, 1 hour, 30 seconds"
TimeSpan.FromMilliseconds(1299630020).Humanize(5) => "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds"
```

Many localisations are availalbe for this method:

```C#
// in de-DE culture
TimeSpan.FromDays(1).Humanize() => "Ein Tag"
TimeSpan.FromDays(2).Humanize() => "2 Tage"

// in sk-SK culture
TimeSpan.FromMilliseconds(1).Humanize() => "1 milisekunda"
TimeSpan.FromMilliseconds(2).Humanize() => "2 milisekundy"
TimeSpan.FromMilliseconds(5).Humanize() => "5 milisekúnd"
```

###<a id="inflector-methods">Inflector methods</a>
There are also a few inflector methods:

####<a id="pluralize">Pluralize</a>
`Pluralize` pluralizes the provided input while taking irregular and uncountable words into consideration:

```C#
"Man".Pluralize() => "Men" 
"string".Pluralize() => "strings"
```

Normally you would call `Pluralize` on a singular word but if you're unsure about the singularity of the word you can call the method with the optional `plurality` argument:

```C#
"Men".Pluralize(Plurality.CouldBeEither) => "Men" 
"Man".Pluralize(Plurality.CouldBeEither) => "Men" 
"string".Pluralize(Plurality.CouldBeEither) => "strings"
```

####<a id="singularize">Singularize</a>
`Singularize` singularizes the provided input while taking irregular and uncountable words into consideration:

```C#
"Men".Singularize() => "Man" 
"strings".Singularize() => "string"
```

Normally you would call `Singularize` on a plural word but if you're unsure about the pluralit of the word you can call the method with the optional `plurality` argument:

```C#
"Men".Singularize(Plurality.CouldBeEither) => "Man" 
"Man".Singularize(Plurality.CouldBeEither) => "Man" 
"strings".Singularize(Plurality.CouldBeEither) => "string"
```

####<a id="toquantity">ToQuantity</a>
Many times you want to call `Singularize` and `Pluralize` to prefix a word with a number; e.g. "2 requests", "3 men". `ToQuantity` prefixes the provided word with the number and accordingly pluralizes or singularizes the word:

```C#
"case".ToQuantity(0) => "0 cases"
"case".ToQuantity(1) => "1 case"
"case".ToQuantity(5) => "5 cases"
"man".ToQuantity(0) => "0 men"
"man".ToQuantity(1) => "1 man"
"man".ToQuantity(2) => "2 men"
```

`ToQuantity` has smarts to figure out whether your input word is plural or singular and changes or leaves it depending on the input quantity:

```C#
"men".ToQuantity(2) => "2 men"
"process".ToQuantity(2) => "2 processes"
"process".ToQuantity(1) => "1 process"
"processes".ToQuantity(2) => "2 processes"
"processes".ToQuantity(1) => "1 process"
```

You can also pass a second argument, `ShowQuantityAs`, to `ToQuantity` to specify how you want the provided quantity to be outputted. The default value is `ShowQuantityAs.Numeric` which is what we saw above. The other two values are `ShowQuantityAs.Words` and `ShowQuantityAs.None`.

```C#
"case".ToQuantity(5, ShowQuantityAs.Words) => "five cases"
"case".ToQuantity(5, ShowQuantityAs.None) => "cases"
```

####<a id="ordinalize">Ordinalize</a>
`Ordinalize` turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th: 

```C#
1.Ordinalize() => "1st"
5.Ordinalize() => "5th"
```

You can also call `Ordinalize` on a numeric string and achieve the same result: `"21".Ordinalize()` => `"21st"`

####<a id="underscore">Underscore</a>
`Underscore` separates the input words with underscore; e.g. `"SomeTitle".Underscore()` => `"some_title"`

####<a id="dasherize--hyphenate">Dasherize & Hyphenate</a>
`Dasherize` and `Hyphenate` replace underscores with dashes in the string:

```C3
"some_title".Dasherize() => "some-title"
"some_title".Hyphenate() => "some-title"
```

###<a id="fluent-date">Fluent Date</a>
Humanizer provides a fluent API to deal with `DateTime` and `TimeSpan` as follows:

`TimeSpan` methods:

```C#
2.Milliseconds() => TimeSpan.FromMilliseconds(2)
2.Seconds() => TimeSpan.FromSeconds(2)
2.Minutes() => TimeSpan.FromMinutes(2)
2.Hours() => TimeSpan.FromHours(2)
2.Days() => TimeSpan.FromDays(2)
2.Weeks() => TimeSpan.FromDays(14)
```

<small>There are no fluent APIs for month or year as a month could have between 28 to 31 days and a year could be 365 or 366 days.</small>

You could use these methods to, for example, replace
 
```C#
DateTime.Now.AddDays(2).AddHours(3).AddMinutes(-5)
```

with 

```C#
DateTime.Now + 2.Days() + 3.Hours() - 5.Minutes()
```

There are also three categories of fluent methods to deal with `DateTime`:

```C#
In.TheYear(2010) // Returns the first of January of 2010
In.January // Returns 1st of January of the current year
In.FebruaryOf(2009) // Returns 1st of February of 2009

In.One.Second //  DateTime.UtcNow.AddSeconds(1);
In.Two.SecondsFrom(DateTime dateTime)
In.Three.Minutes // With corresponding From method
In.Three.Hours // With corresponding From method
In.Three.Days // With corresponding From method
In.Three.Weeks // With corresponding From method
In.Three.Months // With corresponding From method
In.Three.Years // With corresponding From method

On.January.The4th // Returns 4th of January of the current year
On.February.The(12) // Returns 12th of Feb of the current year
```

and some extension methods:

```C#
var someDateTime = new DateTime(2011, 2, 10, 5, 25, 45, 125);

// Returns new DateTime(2008, 2, 10, 5, 25, 45, 125) changing the year to 2008
someDateTime.In(2008) 

// Returns new DateTime(2011, 2, 10, 2, 25, 45, 125) changing the hour to 2:25:45.125
someDateTime.At(2) 

// Returns new DateTime(2011, 2, 10, 2, 20, 15, 125) changing the time to 2:20:15.125
someDateTime.At(2, 20, 15) 

// Returns new DateTime(2011, 2, 10, 12, 0, 0) changing the time to 12:00:00.000
someDateTime.AtNoon() 

// Returns new DateTime(2011, 2, 10, 0, 0, 0) changing the time to 00:00:00.000
someDateTime.AtMidnight() 
```

Obviously you could chain the methods too; e.g. `On.November.The13th.In(2010).AtNoon + 5.Minutes()`

###<a id="number-towords">Number to words</a>
Humanizer can change numbers to words using the `ToWords` extension:

```C#
1.ToWords() => "one"
10.ToWords() => "ten"
11.ToWords() => "eleven"
122.ToWords() => "one hundred and twenty-two"
3501.ToWords() => "three thousand five hundred and one"
```

You can also pass a second argument, `GrammaticalGender`, to `ToWords` to specify which gender the number should be outputted in. 
The possible values are `GrammaticalGender.Masculine`, `GrammaticalGender.Feminine` and `GrammaticalGender.Neuter`:

```C#
// for Russian locale
1.ToWords(GrammaticalGender.Masculine) => "один"
1.ToWords(GrammaticalGender.Feminine) => "одна"
1.ToWords(GrammaticalGender.Neuter) => "одно"
```

Obviously this only applies to some cultures. For others passing gender in doesn't make any difference in the result.

###<a id="number-toordinalwords">Number to ordinal words</a>
This is kind of mixing `ToWords` with `Ordinalize`. You can call `ToOrdinalWords` on a number to get an ordinal representation of the number in words!! Let me show that with an example:

```C#
0.ToOrdinalWords() => "zeroth"
1.ToOrdinalWords() => "first"
2.ToOrdinalWords() => "second"
8.ToOrdinalWords() => "eighth"
10.ToOrdinalWords() => "tenth"
11.ToOrdinalWords() => "eleventh"
12.ToOrdinalWords() => "twelfth"
20.ToOrdinalWords() => "twentieth"
21.ToOrdinalWords() => "twenty first"
121.ToOrdinalWords() => "hundred and twenty first"
```

`ToOrdinalWords` also supports grammatical gender. 
You can pass a second argument to `ToOrdinalWords` to specify which gender the number should be outputted in. 
The possible values are `GrammaticalGender.Masculine`, `GrammaticalGender.Feminine` and `GrammaticalGender.Neuter`:

```C#
// for Brazilian Portuguese locale
1.ToOrdinalWords(GrammaticalGender.Masculine) => "primeiro"
1.ToOrdinalWords(GrammaticalGender.Feminine) => "primeira"
1.ToOrdinalWords(GrammaticalGender.Neuter) => "primeiro"
2.ToOrdinalWords(GrammaticalGender.Masculine) => "segundo"
2.ToOrdinalWords(GrammaticalGender.Feminine) => "segunda"
2.ToOrdinalWords(GrammaticalGender.Neuter) => "segundo"
```

Obviously this only applies to some cultures. For others passing gender in doesn't make any difference in the result.

###<a id="roman-numerals">Roman numerals</a>
Humanizer can change numbers to Roman numerals using the `ToRoman` extension. The numbers 1 to 10 can be expressed in Roman numerals as follows:

```C#
1.ToRoman() => "I"
2.ToRoman() => "II"
3.ToRoman() => "III"
4.ToRoman() => "IV"
5.ToRoman() => "V"
6.ToRoman() => "VI"
7.ToRoman() => "VII"
8.ToRoman() => "VIII"
9.ToRoman() => "IX"
10.ToRoman() => "X"
```

Also the reverse operation using the `FromRoman` extension.

```C#
"I".FromRoman() => 1
"II".FromRoman() => 2
"III".FromRoman() => 3
"IV".FromRoman() => 4
"V".FromRoman() => 5
```

###<a id="bytesize">ByteSize</a>
Humanizer includes a port of the brilliant [ByteSize](https://github.com/omar/ByteSize) library.
Quite a few changes and additions are made on `ByteSize` to make the interaction with `ByteSize` easier and more consistent with the Humanizer API. 
Here is a few examples of how you can convert from numbers to byte sizes and between size magnitudes:

```c#
var fileSize = (10).Kilobytes();

fileSize.Bits      => 81920
fileSize.Bytes     => 10240
fileSize.Kilobytes => 10
fileSize.Megabytes => 0.009765625
fileSize.Gigabytes => 9.53674316e-6
fileSize.Terabytes => 9.31322575e-9
```

There are a few extension methods that allow you to turn a number into a ByteSize instance:

```C#
3.Bits();
5.Bytes();
(10.5).Kilobytes();
(2.5).Megabytes();
(10.2).Gigabytes();
(4.7).Terabytes();
```

You can also add/subtract the values using +/- operators and Add/Subtract methods:

```C#
var total = (10).Gigabytes() + (512).Megabytes() - (2.5).Gigabytes();
total.Subtract((2500).Kilobytes()).Add((25).Megabytes());
```

A `ByteSize` object contains two properties that represent the largest metric prefix symbol and value:

```C#
var maxFileSize = (10).Kilobytes();

maxFileSize.LargestWholeNumberSymbol;  // "KB"
maxFileSize.LargestWholeNumberValue;   // 10
```

If you want a string representation you can call `ToString` or `Humanize` interchangeably on the `ByteSize` instance:

```C#
7.Bits().ToString();         // 7 b
8.Bits().ToString();         // 1 B
(.5).Kilobytes().Humanize();   // 512 B
(1000).Kilobytes().ToString(); // 1000 KB
(1024).Kilobytes().Humanize(); // 1 MB
(.5).Gigabytes().Humanize();   // 512 MB
(1024).Gigabytes().ToString(); // 1 TB
```

You can also optionally provide a format for the expected string representation. 
The formatter can contain the symbol of the value to display: `b`, `B`, `KB`, `MB`, `GB`, `TB`. 
The formatter uses the built in [`double.ToString` method](http://msdn.microsoft.com/en-us/library/kfsatb94\(v=vs.110\).aspx) with `#.##` as the default format which rounds the number to two decimal places:

```C#
var b = (10.505).Kilobytes();

// Default number format is #.##
b.ToString("KB");         // 10.52 KB
b.Humanize("MB");         // .01 MB
b.Humanize("b");          // 86057 b

// Default symbol is the largest metric prefix value >= 1
b.ToString("#.#");        // 10.5 KB

// All valid values of double.ToString(string format) are acceptable
b.ToString("0.0000");     // 10.5050 KB
b.Humanize("000.00");     // 010.51 KB

// You can include number format and symbols
b.ToString("#.#### MB");  // .0103 MB
b.Humanize("0.00 GB");    // 0 GB
b.Humanize("#.## B");     // 10757.12 B
```

There isn't a `Dehumanize` method to turn a string representation back into a `ByteSize` instance; but you can use `Parse` and `TryParse` on `ByteSize` to do that.
Like other `TryParse` methods, `ByteSize.TryParse` returns `boolean` value indicating whether or not the parsing was successful. 
If the value is parsed it is output to the `out` parameter supplied:

```
ByteSize output;
ByteSize.TryParse("1.5mb", out output);

// Invalid
ByteSize.Parse("1.5 b");   // Can't have partial bits

// Valid
ByteSize.Parse("5b");
ByteSize.Parse("1.55B");
ByteSize.Parse("1.55KB");
ByteSize.Parse("1.55 kB "); // Spaces are trimmed
ByteSize.Parse("1.55 kb");
ByteSize.Parse("1.55 MB");
ByteSize.Parse("1.55 mB");
ByteSize.Parse("1.55 mb");
ByteSize.Parse("1.55 GB");
ByteSize.Parse("1.55 gB");
ByteSize.Parse("1.55 gb");
ByteSize.Parse("1.55 TB");
ByteSize.Parse("1.55 tB");
ByteSize.Parse("1.55 tb");
```

##<a id="mix-this-into-your-framework-to-simplify-your-life">Mix this into your framework to simplify your life</a>
This is just a baseline and you can use this to simplify your day to day job. For example, in Asp.Net MVC we keep chucking `Display` attribute on ViewModel properties so `HtmlHelper` can generate correct labels for us; but, just like enums, in vast majority of cases we just need a space between the words in property name - so why not use `"string".Humanize` for that?! 

You may find an Asp.Net MVC sample [in the code](https://github.com/MehdiK/Humanizer/tree/master/src/Humanizer.MvcSample) that does that (although the project is excluded from the solution file to make the nuget package available for .Net 3.5 too). 

This is achieved using a custom `DataAnnotationsModelMetadataProvider` I called [HumanizerMetadataProvider](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.MvcSample/HumanizerMetadataProvider.cs). It is small enough to repeat here; so here we go:

```C#
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Humanizer;

namespace YourApp
{
    public class HumanizerMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
            IEnumerable<Attribute> attributes,
            Type containerType,
            Func<object> modelAccessor,
            Type modelType,
            string propertyName)
        {
            var propertyAttributes = attributes.ToList();
            var modelMetadata = base.CreateMetadata(propertyAttributes, containerType, modelAccessor, modelType, propertyName);
        
            if (IsTransformRequired(modelMetadata, propertyAttributes))
                modelMetadata.DisplayName = modelMetadata.PropertyName.Humanize();
        
            return modelMetadata;
        }
        
        private static bool IsTransformRequired(ModelMetadata modelMetadata, IList<Attribute> propertyAttributes)
        {
            if (string.IsNullOrEmpty(modelMetadata.PropertyName))
                return false;
        
            if (propertyAttributes.OfType<DisplayNameAttribute>().Any())
                return false;
        
            if (propertyAttributes.OfType<DisplayAttribute>().Any())
                return false;
        
            return true;
        }
    }
}
```

This class calls the base class to extract the metadata and then, if required, humanizes the property name. 
It is checking if the property already has a `DisplayName` or `Display` attribute on it in which case the metadata provider will just honor the attribute and leave the property alone. 
For other properties it will Humanize the property name. That is all.

Now you need to register this metadata provider with Asp.Net MVC. 
Make sure you use `System.Web.Mvc.ModelMetadataProviders`, and not `System.Web.ModelBinding.ModelMetadataProviders`:

```C#
ModelMetadataProviders.Current = new HumanizerMetadataProvider();
```

... and now you can replace:

```C#
public class RegisterModel
{
    [Display(Name = "User name")]
    public string UserName { get; set; }
    
    [Display(Name = "Email address")]
    public string EmailAddress { get; set; }
    
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }
}
```

with:

```C#
public class RegisterModel
{
    public string UserName { get; set; }
    public string EmailAddress { get; set; }
    public string ConfirmPassword { get; set; }
}
```

... and the "metadata humanizer" will take care of the rest.

No need to mention that if you want title casing for your labels you can chain the method with `Transform`:

```C#
modelMetadata.DisplayName = modelMetadata.PropertyName.Humanize().Transform(To.TitleCase);
```

##<a id="how-to-contribute">How to contribute?</a>
Your contribution to Humanizer would be very welcome. 
If you find a bug, please raise it as an issue. 
Even better fix it and send me a pull request. 
If you like to help me out with existing bugs and feature requests just check out the list of [issues](https://github.com/MehdiK/Humanizer/issues) and grab and fix one. 
I have also flagged some of the easier issues as 'jump in' so you can start with easier tasks.

###<a id="contribution-guideline">Contribution guideline</a>
I use [GitHub flow](http://scottchacon.com/2011/08/31/github-flow.html) for pull requests. 
So if you want to contribute, fork the repo, preferrably create a local branch to avoid conflicts with other activities, fix an issue and send a PR.

Pull requests are code reviewed. Here is a checklist you should tick through before submitting a pull request:

 - Implementation is clean
 - Code adheres to the existing coding standards; e.g. no curlies for one-line blocks & no redundant empty lines between methods or code blocks
 - No ReSharper warnings
 - There is proper unit test coverage
 - If the code is copied from StackOverflow (or a blog or OSS) full disclosure is included. That includes required license files and/or file headers explaining where the code came from with proper attribution
 - There is very little or no comments (because comments shouldn't be needed if you write clean code)
 - Xml documentation is added/updated for the addition/change
 - Your PR is (re)based on top of the latest commits (more info below)
 - Link to the issue(s) you're fixing from your PR description. Use `fixes #<the issue number>`
 - Readme is updated if you change an existing feature or add a new one
 - An entry is added in the release_notes.md file in the 'In Development' section with a link to your PR and a description of what's changed. Please follow the wording style for the description.

Please rebase your code on top of the latest commits. 
Before working on your fork make sure you pull the latest so you work on top of the latest commits to avoid merge conflicts. 
Also before sending the pull request pleast rebase your code as there is a chance there has been a few commits pushed up after you pulled last. 
Please refer to [this guide](https://gist.github.com/jbenet/ee6c9ac48068889b0912#the-workflow) if you're new to git.

###<a id="need-your-help-with-localisation">Need your help with localisation</a>
One area Humanizer could always use your help is localisation. 
Currently Humanizer supports quite a few localisations for `DateTime.Humanize`, `TimeSpan.Humanize`, `ToWords` and `ToOrdinalWords`. 

Humanizer could definitely do with more translations. 
To add a translation for `DateTime.Humanize` and `TimeSpan.Humanize`, 
fork the repository if you haven't done yet, duplicate the [resources.resx](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Properties/Resources.resx) file, add your target [locale code](http://msdn.microsoft.com/en-us/library/hh441729.aspx) 
to the end (e.g. resources.ru.resx for Russian), translate the values to your language, write unit tests for the translation, commit, and send a pull request for it. Thanks.

Some languages have complex rules when it comes to dealing with numbers; for example, in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and in Arabic "2 days" is "يومين" not "2 يوم".
Obviously a normal resource file doesn't cut it in these cases as a more complex mapping is required.
In cases like this in addition to creating a resource file you should also subclass [`DefaultFormatter`](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/Formatters/DefaultFormatter.cs) in a class that represents your language; 
e.g. [RomanianFormatter](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/Formatters/RomanianFormatter.cs) and then override the methods that need the complex rules. 
We think overriding the `GetResourceKey` method should be enough. 
To see how to do that check out `RomanianFormatter` and `RussianFormatter`. 
Then you return an instance of your class in the [Configurator](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Configuration/Configurator.cs) class in the getter of the [Formatter property](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Configuration/Configurator.cs) based on the current culture.

Translations for `ToWords` and `ToOrdinalWords` methods are currently done in code as there is a huge difference between the way different languages deal with number words.
Check out [Dutch](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/NumberToWords/DutchNumberToWordsConverter.cs) and 
[Russian](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/NumberToWords/RussianNumberToWordsConverter.cs) localisations for examples of how you can write a Converter for your language.
You should then register your converter in the [ConverterFactory](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/NumberToWordsExtension.cs#L13) for it to kick in on your locale.

Don't forget to write tests for your localisations. Check out the existing [DateHumanizeTests](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.Tests/Localisation/ru-RU/DateHumanizeTests.cs), [TimeSpanHumanizeTests](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.Tests/Localisation/ru-RU/TimeSpanHumanizeTests.cs) and [NumberToWordsTests](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.Tests/Localisation/ru-RU/NumberToWordsTests.cs).

##<a id="continuous-integration-from-teamcity">Continuous Integration from TeamCity</a>
Humanizer project is built & tested continuously by TeamCity (more details [here](http://www.mehdi-khalili.com/continuous-integration-delivery-github-teamcity)). That applies to pull requests too. Shortly after you submit a PR you can check the build and test status notification on your PR. I would appreciate if you could send me green PRs.

The current build status on the CI server is <a href="http://teamcity.ginnivan.net/viewType.html?buildTypeId=Humanizer_CI&guest=1">
<img src="http://teamcity.ginnivan.net/app/rest/builds/buildType:(id:Humanizer_CI)/statusIcon"/></a>

##<a id="author">Author</a>
Mehdi Khalili ([@MehdiKhalili](http://twitter.com/MehdiKhalili))

##<a id="main-contributors">Main contributors</a>
Alexander I. Zaytsev ([@hazzik](https://github.com/hazzik))

##<a id="license">License</a>
Humanizer is released under the MIT License. See the [bundled LICENSE](https://github.com/MehdiK/Humanizer/blob/master/LICENSE) file for details.

