<p><img src="https://raw.github.com/Humanizr/Humanizer/master/logo.png" alt="Logo" style="max-width:100%;" /></p>

[<img align="right" width="100px" src="https://dotnetfoundation.org/img/logo_big.svg" />](https://dotnetfoundation.org/projects?type=project&q=humanizer)

Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities. It is part of the [.NET Foundation](https://www.dotnetfoundation.org/), and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under the [MIT](https://opensource.org/licenses/MIT) (an OSI approved license).

[![Join the chat at https://gitter.im/Humanizr/Humanizer](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Humanizr/Humanizer)

### Table of contents
 - [Install](#install)
   - [Specifying Languages (Optional)](#specify-lang)
   - [Known Installation Issues](#known-issues)
   - [Use in ASP.NET 4.x MVC Views](#aspnet4mvc)
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
   - [Humanize Collections](#humanize-collections)
   - [Inflector methods](#inflector-methods)
     - [Pluralize](#pluralize)
     - [Singularize](#singularize)
     - [Adding Words](#adding-words)
     - [ToQuantity](#toquantity)
     - [Ordinalize](#ordinalize)
     - [Titleize](#titleize)
     - [Pascalize](#pascalize)
     - [Camelize](#camelize)
     - [Underscore](#underscore)
     - [Dasherize & Hyphenate](#dasherize--hyphenate)
	 - [Kebaberize](#kebaberize)
   - [Fluent date](#fluent-date)
   - [Number to Numbers](#number-to-numbers)
   - [Number to words](#number-to-words)
   - [Number to ordinal words](#number-to-ordinal-words)
   - [DateTime to ordinal words](#date-time-to-ordinal-words)
   - [TimeOnly to Clock Notation](#time-only-to-clock-notation)
   - [Roman numerals](#roman-numerals)
   - [Metric numerals](#metric-numerals)
   - [ByteSize](#bytesize)
   - [Heading to words](#heading-to-words)
   - [Tupleize](#tupleize)
   - [Time unit to symbol](#timeunit-to-symbol)
 - [Mix this into your framework to simplify your life](#mix-this-into-your-framework-to-simplify-your-life) -
 - [How to contribute?](#how-to-contribute)
 - [Continuous Integration from AppVeyor](#continuous-integration)
 - [Related Projects](#related-projects)
   - [Humanizer ReSharper Annotations](#humanizer-resharper-annotations)
   - [PowerShell Humanizer](#powershell-humanizer)
   - [Humanizer JVM](#humanizerjvm)
   - [Humanizer.node](#humanizernode)
 - [Main contributors](#main-contributors)
 - [License](#license)
 - [Icon](#icon)

## <a id="install">Install</a>
You can install Humanizer as [a nuget package](https://nuget.org/packages/Humanizer):

**English only**: `Humanizer.Core`

All languages: `Humanizer`

Humanizer is a .NET Standard Class Library with support for .NET Standard 1.0+ (.Net 4.5+, UWP, Xamarin, and .NET Core).

Also Humanizer symbols are source indexed with [SourceLink](https://github.com/dotnet/sourcelink) and are included in the package so you can step through Humanizer code while debugging your code.

For pre-release builds, [Azure Artifacts feed](https://dev.azure.com/dotnet/Humanizer/_packaging?_a=feed&feed=Humanizer) is available where you can pull down CI packages from the latest codebase. The feed URL is:

  - [![Humanizer package in Humanizer feed in Azure Artifacts](https://feeds.dev.azure.com/dotnet/5829eea4-55e5-4a15-ba8d-1de5daaafcea/_apis/public/Packaging/Feeds/b39738c7-8e60-4bfb-825f-29c47261a5cc/Packages/db81f806-d0b5-43a3-99f4-3d27606376b8/Badge)](https://dev.azure.com/dotnet/Humanizer/_packaging?_a=package&feed=b39738c7-8e60-4bfb-825f-29c47261a5cc&package=db81f806-d0b5-43a3-99f4-3d27606376b8&preferRelease=true) `https://pkgs.dev.azure.com/dotnet/Humanizer/_packaging/Humanizer/nuget/v3/index.json`

### <a id="specify-lang">Specify Languages (Optional)</a>
New in Humanizer 2.0 is the option to choose which localization packages you wish to use. You choose which packages
based on what NuGet package(s) you install. By default, the main `Humanizer` 2.0 package installs all supported languages
exactly like it does in 1.x. If you're not sure, then just use the main `Humanizer` package.

Here are the options:

  - **All languages**: use the main `Humanizer` package. This pulls in `Humanizer.Core` and all language packages.
  - **English**: use the `Humanizer.Core` package. Only the English language resources will be available
  - **Specific languages**: Use the language specific packages you'd like. For example for French, use `Humanizer.Core.fr`. You can include multiple languages by installing however many language packages you want.

The detailed explanation for how this works is in the comments [here](https://github.com/Humanizr/Humanizer/issues/59#issuecomment-152546079).

## <a id="features">Features</a>

### <a id="humanize-string">Humanize String</a>
`Humanize` string extensions allow you turn an otherwise computerized string into a more readable human-friendly one.
The foundation of this was set in the [BDDfy framework](https://github.com/TestStack/TestStack.BDDfy) where class names, method names and properties are turned into human readable sentences.

```C#
"PascalCaseInputStringIsTurnedIntoSentence".Humanize() => "Pascal case input string is turned into sentence"

"Underscored_input_string_is_turned_into_sentence".Humanize() => "Underscored input string is turned into sentence"

"Underscored_input_String_is_turned_INTO_sentence".Humanize() => "Underscored input String is turned INTO sentence"
```

Note that a string that contains only upper case letters, and consists only of one word, is always treated as an acronym (regardless of its length). To guarantee that any arbitrary string will always be humanized you must use a transform (see `Transform` method below):

```C#
// acronyms are left intact
"HTML".Humanize() => "HTML"

// any unbroken upper case string is treated as an acronym
"HUMANIZER".Humanize() => "HUMANIZER"
"HUMANIZER".Transform(To.LowerCase, To.TitleCase) => "Humanizer"
```

You may also specify the desired letter casing:

```C#
"CanReturnTitleCase".Humanize(LetterCasing.Title) => "Can Return Title Case"

"Can_return_title_Case".Humanize(LetterCasing.Title) => "Can Return Title Case"

"CanReturnLowerCase".Humanize(LetterCasing.LowerCase) => "can return lower case"

"CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps) => "CAN HUMANIZE INTO UPPER CASE"
```

 > The `LetterCasing` API and the methods accepting it are legacy from V0.2 era and will be deprecated in the future. Instead of that, you can use `Transform` method explained below.

### <a id="dehumanize-string">Dehumanize String</a>
Much like you can humanize a computer friendly into human friendly string you can dehumanize a human friendly string into a computer friendly one:

```C#
"Pascal case input string is turned into sentence".Dehumanize() => "PascalCaseInputStringIsTurnedIntoSentence"
```

### <a id="transform-string">Transform String</a>
There is a `Transform` method that supersedes `LetterCasing`, `ApplyCase` and `Humanize` overloads that accept `LetterCasing`.
Transform method signature is as follows:

```C#
string Transform(this string input, params IStringTransformer[] transformers)
```

And there are some out of the box implementations of `IStringTransformer` for letter casing:

```C#
"Sentence casing".Transform(To.LowerCase) => "sentence casing"
"Sentence casing".Transform(To.SentenceCase) => "Sentence casing"
"Sentence casing".Transform(To.TitleCase) => "Sentence Casing"
"Sentence casing".Transform(To.UpperCase) => "SENTENCE CASING"
```

`LowerCase` is a public static property on `To` class that returns an instance of private `ToLowerCase` class that implements `IStringTransformer` and knows how to turn a string into lower case.

The benefit of using `Transform` and `IStringTransformer` over `ApplyCase` and `LetterCasing` is that `LetterCasing` is an enum and you're limited to use what's in the framework
while `IStringTransformer` is an interface you can implement in your codebase once and use it with `Transform` method allowing for easy extension.

### <a id="truncate-string">Truncate String</a>
You can truncate a `string` using the `Truncate` method:

```c#
"Long text to truncate".Truncate(10) => "Long text…"
```

By default the `'…'` character is used to truncate strings. The advantage of using the `'…'` character instead of `"..."` is that the former only takes a single character and thus allows more text to be shown before truncation. If you want, you can also provide your own truncation string:

```c#
"Long text to truncate".Truncate(10, "---") => "Long te---"
```

The default truncation strategy, `Truncator.FixedLength`, is to truncate the input string to a specific length, including the truncation string length.
There are two more truncator strategies available: one for a fixed number of (alpha-numerical) characters and one for a fixed number of words.
To use a specific truncator when truncating, the two `Truncate` methods shown in the previous examples all have an overload that allow you to specify the `ITruncator` instance to use for the truncation.
Here are examples on how to use the three provided truncators:

```c#
"Long text to truncate".Truncate(10, Truncator.FixedLength) => "Long text…"
"Long text to truncate".Truncate(10, "---", Truncator.FixedLength) => "Long te---"

"Long text to truncate".Truncate(6, Truncator.FixedNumberOfCharacters) => "Long t…"
"Long text to truncate".Truncate(6, "---", Truncator.FixedNumberOfCharacters) => "Lon---"

"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords) => "Long text…"
"Long text to truncate".Truncate(2, "---", Truncator.FixedNumberOfWords) => "Long text---"
```

Note that you can also use create your own truncator by implementing the `ITruncator` interface.

There is also an option to choose whether to truncate the string from the beginning (`TruncateFrom.Left`) or the end (`TruncateFrom.Right`).
Default is the right as shown in the examples above. The examples below show how to truncate from the beginning of the string:

```c#
"Long text to truncate".Truncate(10, Truncator.FixedLength, TruncateFrom.Left) => "… truncate"
"Long text to truncate".Truncate(10, "---", Truncator.FixedLength, TruncateFrom.Left) => "---runcate"

"Long text to truncate".Truncate(10, Truncator.FixedNumberOfCharacters, TruncateFrom.Left) => "…o truncate"
"Long text to truncate".Truncate(16, "---", Truncator.FixedNumberOfCharacters, TruncateFrom.Left) => "---ext to truncate"

"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords, TruncateFrom.Left) => "…to truncate"
"Long text to truncate".Truncate(2, "---", Truncator.FixedNumberOfWords, TruncateFrom.Left) => "---to truncate"
```

### <a id="format-string">Format String</a>
You can format a `string` using the `FormatWith()` method:

```c#
"To be formatted -> {0}/{1}.".FormatWith(1, "A") => "To be formatted -> 1/A."
```

This is an extension method based on `String.Format`, so exact rules applies to it.
If `format` is null, it'll throw `ArgumentNullException`.
If passed a fewer number for arguments, it'll throw `String.FormatException` exception.

You also can specify the culture to use explicitly as the first parameter for the `FormatWith()` method:

```c#
"{0:N2}".FormatWith(new CultureInfo("ru-RU"), 6666.66) => "6 666,66"
```

If a culture is not specified, current thread's current culture is used.

### <a id="humanize-enums">Humanize Enums</a>
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

You can even configure the name of the property of attibute to use as description.

`Configurator.EnumDescriptionPropertyLocator = p => p.Name == "Info"`

If you need to provide localised descriptions you can use `DisplayAttribute` data annotation instead.

```C#
public enum EnumUnderTest
{
    [Display(Description = "EnumUnderTest_Member", ResourceType = typeof(Project.Resources))]
    Member
}
```

You will get:

```C#
EnumUnderTest.Member.Humanize() => "content" // from Project.Resources found under "EnumUnderTest_Member" resource key
```

Hopefully this will help avoid littering enums with unnecessary attributes!

### <a id="dehumanize-enums">Dehumanize Enums</a>
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
In the non-generic method you can also ask the method to return null by setting the second optional parameter to `NoMatch.ReturnsNull`.

### <a id="humanize-datetime">Humanize DateTime</a>
You can `Humanize` an instance of `DateTime` or `DateTimeOffset` and get back a string telling how far back or forward in time that is:

```C#
DateTime.UtcNow.AddHours(-30).Humanize() => "yesterday"
DateTime.UtcNow.AddHours(-2).Humanize() => "2 hours ago"

DateTime.UtcNow.AddHours(30).Humanize() => "tomorrow"
DateTime.UtcNow.AddHours(2).Humanize() => "2 hours from now"

DateTimeOffset.UtcNow.AddHours(1).Humanize() => "an hour from now"
```

Humanizer supports both local and UTC dates as well as dates with offset (`DateTimeOffset`). You could also provide the date you want the input date to be compared against. If null, it will use the current date as comparison base.
Also, culture to use can be specified explicitly. If it is not, current thread's current UI culture is used.
Here is the API signature:

```C#
public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null, CultureInfo culture = null)
public static string Humanize(this DateTimeOffset input, DateTimeOffset? dateToCompareAgainst = null, CultureInfo culture = null)
```

Many localizations are available for this method. Here is a few examples:

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

```C#
Configurator.DateTimeHumanizeStrategy = new PrecisionDateTimeHumanizeStrategy(precision: .75);
Configurator.DateTimeOffsetHumanizeStrategy = new PrecisionDateTimeOffsetHumanizeStrategy(precision: .75); // configure when humanizing DateTimeOffset
```

The default precision is set to .75 but you can pass your desired precision too. With precision set to 0.75:

```C#
44 seconds => 44 seconds ago/from now
45 seconds => one minute ago/from now
104 seconds => one minute ago/from now
105 seconds => two minutes ago/from now

25 days => a month ago/from now
```

**No dehumanization for dates as `Humanize` is a lossy transformation and the human friendly date is not reversible**

### <a id="humanize-timespan">Humanize TimeSpan</a>
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
TimeSpan.FromDays(1).Humanize(precision:2) => "1 day" // no difference when there is only one unit in the provided TimeSpan
TimeSpan.FromDays(16).Humanize(2) => "2 weeks, 2 days"

// the same TimeSpan value with different precision returns different results
TimeSpan.FromMilliseconds(1299630020).Humanize() => "2 weeks"
TimeSpan.FromMilliseconds(1299630020).Humanize(3) => "2 weeks, 1 day, 1 hour"
TimeSpan.FromMilliseconds(1299630020).Humanize(4) => "2 weeks, 1 day, 1 hour, 30 seconds"
TimeSpan.FromMilliseconds(1299630020).Humanize(5) => "2 weeks, 1 day, 1 hour, 30 seconds, 20 milliseconds"
```

By default when using `precision` parameter empty time units are not counted towards the precision of the returned value.
If this behavior isn't desired for you, you can use the overloaded `TimeSpan.Humanize` method with `countEmptyUnits` parameter. Leading empty time units never count.
Here is an example showing the difference of counting empty units:

```C#
TimeSpan.FromMilliseconds(3603001).Humanize(3) => "1 hour, 3 seconds, 1 millisecond"
TimeSpan.FromMilliseconds(3603001).Humanize(3, countEmptyUnits:true) => "1 hour, 3 seconds"
```

Many localizations are available for this method:

```C#
// in de-DE culture
TimeSpan.FromDays(1).Humanize() => "Ein Tag"
TimeSpan.FromDays(2).Humanize() => "2 Tage"

// in sk-SK culture
TimeSpan.FromMilliseconds(1).Humanize() => "1 milisekunda"
TimeSpan.FromMilliseconds(2).Humanize() => "2 milisekundy"
TimeSpan.FromMilliseconds(5).Humanize() => "5 milisekúnd"
```

Culture to use can be specified explicitly. If it is not, current thread's current UI culture is used. Example:

```C#
TimeSpan.FromDays(1).Humanize(culture: "ru-RU") => "один день"
```

In addition, a minimum unit of time may be specified to avoid rolling down to a smaller unit. For example:
  ```C#
  TimeSpan.FromMilliseconds(122500).Humanize(minUnit: TimeUnit.Second) => "2 minutes, 2 seconds"    // instead of 2 minutes, 2 seconds, 500 milliseconds
  TimeSpan.FromHours(25).Humanize(minUnit: TimeUnit.Day) => "1 Day"   //instead of 1 Day, 1 Hour
  ```

In addition, a maximum unit of time may be specified to avoid rolling up to the next largest unit. For example:
```C#
TimeSpan.FromDays(7).Humanize(maxUnit: TimeUnit.Day) => "7 days"    // instead of 1 week
TimeSpan.FromMilliseconds(2000).Humanize(maxUnit: TimeUnit.Millisecond) => "2000 milliseconds"    // instead of 2 seconds
```
The default maxUnit is `TimeUnit.Week` because it gives exact results. You can increase this value to `TimeUnit.Month` or `TimeUnit.Year` which will give you an approximation based on 365.2425 days a year and 30.436875 days a month. Therefore the months are alternating between 30 and 31 days in length and every fourth year is 366 days long.
```C#
TimeSpan.FromDays(486).Humanize(maxUnit: TimeUnit.Year, precision: 7) => "1 year, 3 months, 29 days" // One day further is 1 year, 4 month
TimeSpan.FromDays(517).Humanize(maxUnit: TimeUnit.Year, precision: 7) => "1 year, 4 months, 30 days" // This month has 30 days and one day further is 1 year, 5 months
```

When there are multiple time units, they are combined using the `", "` string:

```C#
TimeSpan.FromMilliseconds(1299630020).Humanize(3) => "2 weeks, 1 day, 1 hour"
```

When `TimeSpan` is zero, the default behavior will return "0" plus whatever the minimum time unit is. However, if you assign `true` to `toWords` when calling `Humanize`, then the method returns "no time". For example:
```C#
TimeSpan.Zero.Humanize(1) => "0 milliseconds"
TimeSpan.Zero.Humanize(1, toWords: true) => "no time"
TimeSpan.Zero.Humanize(1, minUnit: Humanizer.Localisation.TimeUnit.Second) => "0 seconds"
```

Using the `collectionSeparator` parameter, you can specify your own separator string:

```C#
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: " - ") => "2 weeks - 1 day - 1 hour"
````

It is also possible to use the current culture's collection formatter to combine the time units. To do so, specify `null` as the `collectionSeparator` parameter:

```C#
// in en-US culture
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: null) => "2 weeks, 1 day, and 1 hour"

// in de-DE culture
TimeSpan.FromMilliseconds(1299630020).Humanize(3, collectionSeparator: null) => "2 Wochen, Ein Tag und Eine Stunde"
```

If words are preferred to numbers, a `toWords: true` parameter can be set to convert the numbers in a humanized TimeSpan to words:
```C#
TimeSpan.FromMilliseconds(1299630020).Humanize(3, toWords: true) => "two weeks, one day, one hour"
````

### <a id="humanize-collections">Humanize Collections</a>
You can call `Humanize` on any `IEnumerable` to get a nicely formatted string representing the objects in the collection. By default `ToString()` will be called on each item to get its representation but a formatting function may be passed to `Humanize` instead. Additionally, a default separator is provided ("and" in English), but a different separator may be passed into `Humanize`.

For instance:

```C#
class SomeClass
{
    public string SomeString;
    public int SomeInt;
    public override string ToString()
    {
        return "Specific String";
    }
}

string FormatSomeClass(SomeClass sc)
{
    return string.Format("SomeObject #{0} - {1}", sc.SomeInt, sc.SomeString);
}

var collection = new List<SomeClass>
{
    new SomeClass { SomeInt = 1, SomeString = "One" },
    new SomeClass { SomeInt = 2, SomeString = "Two" },
    new SomeClass { SomeInt = 3, SomeString = "Three" }
};

collection.Humanize()                                    // "Specific String, Specific String, and Specific String"
collection.Humanize("or")                                // "Specific String, Specific String, or Specific String"
collection.Humanize(FormatSomeClass)                     // "SomeObject #1 - One, SomeObject #2 - Two, and SomeObject #3 - Three"
collection.Humanize(sc => sc.SomeInt.Ordinalize(), "or") // "1st, 2nd, or 3rd"
```

Items are trimmed and blank (NullOrWhitespace) items are skipped. This results in clean comma punctuation. (If there is a custom formatter function, this applies only to the formatter's output.)

You can provide your own collection formatter by implementing `ICollectionFormatter` and registering it with `Configurator.CollectionFormatters`.

### <a id="inflector-methods">Inflector methods</a>
There are also a few inflector methods:

#### <a id="pluralize">Pluralize</a>
`Pluralize` pluralizes the provided input while taking irregular and uncountable words into consideration:

```C#
"Man".Pluralize() => "Men"
"string".Pluralize() => "strings"
```

Normally you would call `Pluralize` on a singular word but if you're unsure about the singularity of the word you can call the method with the optional `inputIsKnownToBeSingular` argument:

```C#
"Men".Pluralize(inputIsKnownToBeSingular: false) => "Men"
"Man".Pluralize(inputIsKnownToBeSingular: false) => "Men"
"string".Pluralize(inputIsKnownToBeSingular: false) => "strings"
```


The overload of `Pluralize` with `plurality` argument is obsolete and was removed in version 2.0.

#### <a id="singularize">Singularize</a>
`Singularize` singularizes the provided input while taking irregular and uncountable words into consideration:

```C#
"Men".Singularize() => "Man"
"strings".Singularize() => "string"
```

Normally you would call `Singularize` on a plural word but if you're unsure about the plurality of the word you can call the method with the optional `inputIsKnownToBePlural` argument:

```C#
"Men".Singularize(inputIsKnownToBePlural: false) => "Man"
"Man".Singularize(inputIsKnownToBePlural: false) => "Man"
"strings".Singularize(inputIsKnownToBePlural: false) => "string"
```


The overload of `Singularize` with `plurality` argument is obsolete and was removed in version 2.0.

## <a id="adding-words">Adding Words</a>
Sometimes, you may need to add a rule from the singularization/pluralization vocabulary (the examples below are already in the `DefaultVocabulary` used by `Inflector`):

```C#
// Adds a word to the vocabulary which cannot easily be pluralized/singularized by RegEx.
// Will match both "salesperson" and "person".
Vocabularies.Default.AddIrregular("person", "people");

// To only match "person" and not "salesperson" you would pass false for the 'matchEnding' parameter.
Vocabularies.Default.AddIrregular("person", "people", matchEnding: false);

// Adds an uncountable word to the vocabulary.  Will be ignored when plurality is changed:
Vocabularies.Default.AddUncountable("fish");

// Adds a rule to the vocabulary that does not follow trivial rules for pluralization:
Vocabularies.Default.AddPlural("bus", "buses");

// Adds a rule to the vocabulary that does not follow trivial rules for singularization
// (will match both "vertices" -> "vertex" and "indices" -> "index"):
Vocabularies.Default.AddSingular("(vert|ind)ices$", "$1ex");

```

#### <a id="toquantity">ToQuantity</a>
Many times you want to call `Singularize` and `Pluralize` to prefix a word with a number; e.g. "2 requests", "3 men". `ToQuantity` prefixes the provided word with the number and accordingly pluralizes or singularizes the word:

```C#
"case".ToQuantity(0) => "0 cases"
"case".ToQuantity(1) => "1 case"
"case".ToQuantity(5) => "5 cases"
"man".ToQuantity(0) => "0 men"
"man".ToQuantity(1) => "1 man"
"man".ToQuantity(2) => "2 men"
```

`ToQuantity` can figure out whether the input word is singular or plural and will singularize or pluralize as necessary:

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

There is also an overload that allows you to format the number. You can pass in the format and the culture to be used.

```C#
"dollar".ToQuantity(2, "C0", new CultureInfo("en-US")) => "$2 dollars"
"dollar".ToQuantity(2, "C2", new CultureInfo("en-US")) => "$2.00 dollars"
"cases".ToQuantity(12000, "N0") => "12,000 cases"
```

#### <a id="ordinalize">Ordinalize</a>
`Ordinalize` turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th:

```C#
1.Ordinalize() => "1st"
5.Ordinalize() => "5th"
```

You can also call `Ordinalize` on a numeric string and achieve the same result: `"21".Ordinalize()` => `"21st"`

`Ordinalize` also supports grammatical gender for both forms.
You can pass an argument to `Ordinalize` to specify which gender the number should be outputted in.
The possible values are `GrammaticalGender.Masculine`, `GrammaticalGender.Feminine` and `GrammaticalGender.Neuter`:

```C#
// for Brazilian Portuguese locale
1.Ordinalize(GrammaticalGender.Masculine) => "1º"
1.Ordinalize(GrammaticalGender.Feminine) => "1ª"
1.Ordinalize(GrammaticalGender.Neuter) => "1º"
"2".Ordinalize(GrammaticalGender.Masculine) => "2º"
"2".Ordinalize(GrammaticalGender.Feminine) => "2ª"
"2".Ordinalize(GrammaticalGender.Neuter) => "2º"
```

Obviously this only applies to some cultures. For others passing gender in or not passing at all doesn't make any difference in the result.

In addition, `Ordinalize` supports variations some cultures apply depending on the position of the ordinalized number in a sentence.
Use the argument `wordForm` to get one result or another. Possible values are `WordForm.Abbreviation` and `WordForm.Normal`.
You can combine `wordForm` argument with gender but passing this argument in when it is not applicable will not make any difference in the result.

```C#
// Spanish locale
1.Ordinalize(WordForm.Abbreviation) => "1.er" // As in "Vivo en el 1.er piso"
1.Ordinalize(WordForm.Normal) => "1.º" // As in "He llegado el 1º"
"3".Ordinalize(GrammaticalGender.Feminine, WordForm.Abbreviation) => "3.ª"
"3".Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) => "3.ª"
"3".Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) => "3.er"
"3".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) => "3.º"
```

#### <a id="titleize">Titleize</a>
`Titleize` converts the input words to Title casing; equivalent to `"some title".Humanize(LetterCasing.Title)`

#### <a id="pascalize">Pascalize</a>
`Pascalize` converts the input words to UpperCamelCase, also removing underscores and spaces:

```C#
"some_title for something".Pascalize() => "SomeTitleForSomething"
```

#### <a id="camelize">Camelize</a>
`Camelize` behaves identically to `Pascalize`, except that the first character is lower case:

```C#
"some_title for something".Camelize() => "someTitleForSomething"
```

#### <a id="underscore">Underscore</a>
`Underscore` separates the input words with underscore:

```C#
"SomeTitle".Underscore() => "some_title"
```

#### <a id="dasherize--hyphenate">Dasherize & Hyphenate</a>
`Dasherize` and `Hyphenate` replace underscores with dashes in the string:

```C#
"some_title".Dasherize() => "some-title"
"some_title".Hyphenate() => "some-title"
```

#### <a id="kebaberize">Kebaberize</a>
`Kebaberize` separates the input words with hyphens and all words are converted to lowercase

```C#
"SomeText".Kebaberize() => "some-text"
```

### <a id="fluent-date">Fluent Date</a>
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

### <a id="number-to-numbers">Number to numbers</a>
Humanizer provides a fluent API that produces (usually big) numbers in a clearer fashion:

```C#
1.25.Billions() => 1250000000
3.Hundreds().Thousands() => 300000
```

### <a id="number-towords">Number to words</a>
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

```C#
// for Arabic locale
1.ToWords(GrammaticalGender.Masculine) => "واحد"
1.ToWords(GrammaticalGender.Feminine) => "واحدة"
1.ToWords(GrammaticalGender.Neuter) => "واحد"
(-1).ToWords() => "ناقص واحد"
```

Obviously this only applies to some cultures. For others passing gender in doesn't make any difference in the result.

Also, culture to use can be specified explicitly. If it is not, current thread's current UI culture is used. Here's an example:

```C#
11.ToWords(new CultureInfo("en")) => "eleven"
1.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru")) => "один"
```

Another overload of the method allow you to pass a bool to remove the "And" that can be added before the last number:

```C#
3501.ToWords(false) => "three thousand five hundred one"
102.ToWords(false) => "one hundred two"
```
This method can be useful for writing checks for example.

Furthermore, `ToWords` supports variations some cultures apply depending on the position of the number in a sentence.
Use the argument `wordForm` to get one result or another. Possible values are `WordForm.Abbreviation` and `WordForm.Normal`.
This argument can be combined with the rest of the arguments presented above.
Passing `wordForm` argument in when it is not applicable will not make any difference in the result.

```C#
// Spanish locale
21501.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) => "veintiún mil quinientos un"
21501.ToWords(WordForm.Normal, GrammaticalGender.Masculine) => "veintiún mil quinientos uno"
21501.ToWords(WordForm.Abbreviation, GrammaticalGender.Feminine) => "veintiuna mil quinientas una"
// English US locale
21501.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine, new CultureInfo("en-US")) => "twenty-one thousand five hundred and one"
```

### <a id="number-toordinalwords">Number to ordinal words</a>
This is kind of mixing `ToWords` with `Ordinalize`. You can call `ToOrdinalWords` on a number to get an ordinal representation of the number in words! For example:

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
You can pass a second argument to `ToOrdinalWords` to specify the gender of the output.
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

```C#
// for Arabic locale
1.ToOrdinalWords(GrammaticalGender.Masculine) => "الأول"
1.ToOrdinalWords(GrammaticalGender.Feminine) => "الأولى"
1.ToOrdinalWords(GrammaticalGender.Neuter) => "الأول"
2.ToOrdinalWords(GrammaticalGender.Masculine) => "الثاني"
2.ToOrdinalWords(GrammaticalGender.Feminine) => "الثانية"
2.ToOrdinalWords(GrammaticalGender.Neuter) => "الثاني"
```

Obviously this only applies to some cultures. For others passing gender in doesn't make any difference in the result.

Also, culture to use can be specified explicitly. If it is not, current thread's current UI culture is used. Here's an example:

```C#
10.ToOrdinalWords(new CultureInfo("en-US")) => "tenth"
1.ToOrdinalWords(GrammaticalGender.Masculine, new CulureInfo("pt-BR")) => "primeiro"
```

`ToOrdinalWords` also supports variations some cultures apply depending on the position of the ordinalized number in a sentence.
Use the argument `wordForm` to get one result or another. Possible values are `WordForm.Abbreviation` and `WordForm.Normal`.
Combine this argument with the rest of the arguments presented above.
Passing `wordForm` argument in when it is not applicable will not make any difference in the result.

```C#
// Spanish locale
43.ToOrdinalWords(WordForm.Normal, GrammaticalGender.Masculine) => "cuadragésimo tercero"
43.ToOrdinalWords(WordForm.Abbreviation, GrammaticalGender.Masculine) => "cuadragésimo tercer"
43.ToOrdinalWords(WordForm.Abbreviation, GrammaticalGender.Feminine) => "cuadragésima tercera"
// English locale
43.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation, new CultureInfo("en")) => "forty-third"
```

### <a id="date-time-to-ordinal-words">DateTime to ordinal words</a>
This is kind of an extension of Ordinalize
```C#
// for English UK locale
new DateTime(2015, 1, 1).ToOrdinalWords() => "1st January 2015"
new DateTime(2015, 2, 12).ToOrdinalWords() => "12th February 2015"
new DateTime(2015, 3, 22).ToOrdinalWords() => "22nd March 2015"
// for English US locale
new DateTime(2015, 1, 1).ToOrdinalWords() => "January 1st, 2015"
new DateTime(2015, 2, 12).ToOrdinalWords() => "February 12th, 2015"
new DateTime(2015, 3, 22).ToOrdinalWords() => "March 22nd, 2015"
```

`ToOrdinalWords` also supports grammatical case.
You can pass a second argument to `ToOrdinalWords` to specify the case of the output.
The possible values are `GrammaticalCase.Nominative`, `GrammaticalCase.Genitive`, `GrammaticalCase.Dative`, `GrammaticalCase.Accusative`, `GrammaticalCase.Instrumental` and `GrammaticalGender.Prepositional`:

```C#
```

Obviously this only applies to some cultures. For others passing case in doesn't make any difference in the result.

### <a id="time-only-to-clock-notation">TimeOnly to Clock Notation</a>
Extends TimeOnly to allow humanizing it to a clock notation
```C#
// for English US locale
new TimeOnly(3, 0).ToClockNotation() => "three o'clock"
new TimeOnly(12, 0).ToClockNotation() => "noon"
new TimeOnly(14, 30).ToClockNotation() => "half past two"

// for Brazilian Portuguese locale
new TimeOnly(3, 0).ToClockNotation() => "três em ponto"
new TimeOnly(12, 0).ToClockNotation() => "meio-dia"
new TimeOnly(14, 30).ToClockNotation() => "duas e meia"
```

### <a id="roman-numerals">Roman numerals</a>
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
Note that only integers smaller than 4000 can be converted to Roman numerals.

### <a id="metric-numerals">Metric numerals</a>
Humanizer can change numbers to Metric numerals using the `ToMetric` extension. The numbers 1, 1230 and 0.1 can be expressed in Metric numerals as follows:

```C#
1d.ToMetric() => "1"
1230d.ToMetric() => "1.23k"
0.1d.ToMetric() => "100m"
```

Also the reverse operation using the `FromMetric` extension.

```C#
"1".FromMetric() => 1
"1.23k".FromMetric() => 1230
"100m".FromMetric() => 0.1
```

### <a id="bytesize">ByteSize</a>
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
7.Bits().ToString();           // 7 b
8.Bits().ToString();           // 1 B
(.5).Kilobytes().Humanize();   // 512 B
(1000).Kilobytes().ToString(); // 1000 KB
(1024).Kilobytes().Humanize(); // 1 MB
(.5).Gigabytes().Humanize();   // 512 MB
(1024).Gigabytes().ToString(); // 1 TB
```

You can also optionally provide a format for the expected string representation.
The formatter can contain the symbol of the value to display: `b`, `B`, `KB`, `MB`, `GB`, `TB`.
The formatter uses the built in [`double.ToString` method](https://docs.microsoft.com/dotnet/api/system.double.tostring) with `#.##` as the default format which rounds the number to two decimal places:

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

If you want a string representation with full words you can call `ToFullWords` on the `ByteSize` instance:

```C#
7.Bits().ToFullWords();           // 7 bits
8.Bits().ToFullWords();           // 1 byte
(.5).Kilobytes().ToFullWords();   // 512 bytes
(1000).Kilobytes().ToFullWords(); // 1000 kilobytes
(1024).Kilobytes().ToFullWords(); // 1 megabyte
(.5).Gigabytes().ToFullWords();   // 512 megabytes
(1024).Gigabytes().ToFullWords(); // 1 terabyte
```

There isn't a `Dehumanize` method to turn a string representation back into a `ByteSize` instance; but you can use `Parse` and `TryParse` on `ByteSize` to do that.
Like other `TryParse` methods, `ByteSize.TryParse` returns `boolean` value indicating whether or not the parsing was successful.
If the value is parsed it is output to the `out` parameter supplied:

```C#
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

Finally, if you need to calculate the rate at which a quantity of bytes has been transferred, you can use the `Per` method of `ByteSize`. The `Per` method accepts one argument - the measurement interval for the bytes; this is the amount of time it took to transfer the bytes.

The `Per` method returns a `ByteRate` class which has a `Humanize` method. By default, rates are given in seconds (eg, MB/s). However, if desired, a TimeUnit may be passed to `Humanize` for an alternate interval. Valid intervals are `TimeUnit.Second`, `TimeUnit.Minute`, and `TimeUnit.Hour`. Examples of each interval and example byte rate usage is below.

```C#
var size = ByteSize.FromMegabytes(10);
var measurementInterval = TimeSpan.FromSeconds(1);

var text = size.Per(measurementInterval).Humanize();
// 10 MB/s

text = size.Per(measurementInterval).Humanize(TimeUnit.Minute);
// 600 MB/min

text = size.Per(measurementInterval).Humanize(TimeUnit.Hour);
// 35.15625 GB/hour
```

You can specify a format for the bytes part of the humanized output:

```C#
19854651984.Bytes().Per(1.Seconds()).Humanize("#.##");
// 18.49 GB/s
```

### <a id="heading-to-words">Heading to words</a>
Humanizer includes methods to change a numeric heading to words. The heading can be a `double` whereas the result will be a string. You can choose whether to return a full representation of the heading (e.g. north, east, south or west), a short representation (e.g. N, E, S, W) or a unicode arrow character (e.g. ↑, →, ↓, ←).

```C#
360.ToHeading();
// north
720.ToHeading();
// north
```

In order to retrieve a short version of the heading you can use the following call:

```C#
180.ToHeading(true);
// S
360.ToHeading(true);
// N
```

Please note that a textual representation has a maximum deviation of 11.25°.

The methods above all have an overload with which you can provide a `CultureInfo` object in order to determine the localized result to return.

To retrieve an arrow representing the heading use the following method:

```C#
90.ToHeadingArrow();
// →
225.ToHeadingArrow();
// ↙
```

The arrow representation of the heading has a maximum deviation of 22.5°.

In order to retrieve a heading based on the short text representation (e.g. N, E, S, W), the following method can be used:

```C#
"S".FromShortHeading();
// 180
"SW".FromShortHeading();
// 225
```

### <a id="tupleize">Tupleize</a>
Humanizer can change whole numbers into their 'tuple'  using `Tupleize`. For example:

```C#
1.Tupleize();
// single
3.Tupleize();
// triple
100.Tupleize();
// centuple
```

The numbers 1-10, 100 and 1000 will be converted into a 'named' tuple (i.e. "single", "double" etc.). Any other number "n" will be converted to "n-tuple".

### <a id="timeunit-to-symbol">Time unit to symbol</a>
Humanizer can translate time units to their symbols:

```C#
TimeUnit.Day.ToSymbol();
// d
TimeUnit.Week.ToSymbol();
// week
TimeUnit.Year.ToSymbol();
// y
```

## <a id="mix-this-into-your-framework-to-simplify-your-life">Mix this into your framework to simplify your life</a>
This is just a baseline and you can use this to simplify your day to day job. For example, in Asp.Net MVC we keep chucking `Display` attribute on ViewModel properties so `HtmlHelper` can generate correct labels for us; but, just like enums, in vast majority of cases we just need a space between the words in property name - so why not use `"string".Humanize` for that?!

You may find an Asp.Net MVC sample [in the code](https://github.com/Humanizr/Humanizer/tree/master/src/Humanizer.MvcSample) that does that (although the project is excluded from the solution file to make the nuget package available for .Net 3.5 too).

This is achieved using a custom `DataAnnotationsModelMetadataProvider` I called [HumanizerMetadataProvider](https://github.com/Humanizr/Humanizer/blob/master/src/Humanizer.MvcSample/HumanizerMetadataProvider.cs). It is small enough to repeat here; so here we go:

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

## <a id="known-issues">Known installation issues and workarounds</a>
Due to a [bug](https://github.com/dotnet/cli/issues/3396) in the CLI tools, the main `Humanizer` package and it's language packages will fail to install. As temporary workaround, until that bug is fixed, use `Humanizer.xproj` instead. It contains all of the languages.

## <a id="aspnet4mvc">Use in ASP.NET 4.x MVC Views</a>
Humanizer is a Portable Class Library. There is currently [an issue](https://stackoverflow.com/questions/16675171/what-does-the-web-config-compilation-assemblies-element-do) if you try to use PCL's in an MVC view since the MVC views do not share the same build system as the regular project. You must specify all references in the `web.config` file, including ones the project system normally automatically adds.

If you encounter errors saying that you must add a reference to either `System.Runtime` or `System.Globalization`, this applies to you. The solution is to add the contract references to your `web.config` as listed [here](https://stackoverflow.com/a/19942274/738188). Note that this applies to any PCL you use in an MVC view, not just Humanizer.

## <a id="how-to-contribute">How to contribute?</a>

Please see <a href="https://github.com/Humanizr/Humanizer/blob/master/CONTRIBUTING.md">CONTRIBUTING.md</a>.

## <a id="continuous-integration">Continuous Integration from Azure DevOps</a>
Humanizer project is built & tested continuously by Azure DevOps (more details [here](https://dev.azure.com/dotnet/Humanizer/_build?definitionId=14)). That applies to pull requests too. Shortly after you submit a PR you can check the build and test status notification on your PR. Feel free to jump in and <a href="https://github.com/Humanizr/Humanizer/blob/master/CONTRIBUTING.md">contribute</a> some green PRs!

The current build status on the CI server is [![Build status](https://dev.azure.com/dotnet/Humanizer/_apis/build/status/Humanizer-CI?branchName=master)](https://dev.azure.com/dotnet/Humanizer/_build?definitionId=14)

## <a id="related-projects">Related projects</a>
Below is a list of related open source projects:

### <a id="humanizer-resharper-annotations">Humanizer ReSharper Annotations</a>
If you use ReSharper, annotations for Humanizer are available in the [Humanizer.Annotations package](https://resharper-plugins.jetbrains.com/packages/Humanizer.Annotations/), which you can obtain via the ReSharper Extension Manager.
These annotations do not yet cover the entire library, but [pull requests are always welcome!](https://github.com/enduracode/humanizer-annotations).

### <a id="powershell-humanizer">PowerShell Humanizer</a>
[PowerShell Humanizer](https://github.com/dfinke/PowerShellHumanizer) is a PowerShell module that wraps Humanizer.

### <a id="humanizerjvm">Humanizer JVM</a>
[Humanizer.jvm](https://github.com/MehdiK/Humanizer.jvm) is an adaptation of the Humanizer framework for .Net which is made for the jvm and is written in Kotlin.
Humanizer.jvm meets all your jvm needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

### <a id="humanizernode">Humanizer.node</a>
[Humanizer.node](https://github.com/fakoua/humanizer.node) is a TypeScript port of the Humanizer framework.

## <a id="main-contributors">Main contributors</a>
 - Mehdi Khalili ([@MehdiKhalili](https://twitter.com/MehdiKhalili))
 - Claire Novotny ([@clairernovotny](https://twitter.com/clairernovotny))
 - Alexander I. Zaytsev ([@hazzik](https://github.com/hazzik))
 - Max Malook ([@mexx](https://github.com/mexx))

## <a id="license">License</a>
Humanizer is released under the MIT License. See the [bundled LICENSE](https://github.com/Humanizr/Humanizer/blob/master/LICENSE) file for details.

## <a id="icon">Icon</a>
Icon created by [Tyrone Rieschiek](https://twitter.com/Inkventive)
