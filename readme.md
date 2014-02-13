Humanizer meets all your .NET needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities.

###Installation
You can install Humanizer as [a nuget package](https://nuget.org/packages/Humanizer): `Install-Package Humanizer`

Humanizer is a Portable Class Library with support for .Net 4+, SilverLight 5, Windows Phone 8 and Win Store applications. 
Also Humanizer [symbols nuget package](http://www.symbolsource.org/Public/Metadata/NuGet/Project/Humanizer) is published so you can [step through Humanizer code](http://www.symbolsource.org/Public/Home/VisualStudio) while debugging your code.

###Humanize String
String extensions are at the heart of this micro-framework. The foundation of this was set in the [BDDfy framework](https://github.com/TestStack/TestStack.BDDfy) where class names, method names and properties are turned into human readable sentences. 

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

####Dehumanize String
Much like you can humanize a computer friendly into human friendly string you can dehumanize a human friendly string into a computer friendly one:

```C#
"Pascal case input string is turned into sentence".Dehumanize() => "PascalCaseInputStringIsTurnedIntoSentence"
```

###Transform
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

###Humanize Enums
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

Hopefully this will help avoid littering enums with unnecessary attributes!

####Dehumanize Enums
Dehumanizes a string into the Enum it was originally Humanized from! The API looks like:

```C#
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input) 
```

And the usage is:

```C#
"Member without description attribute".DehumanizeTo<EnumUnderTest>() => EnumUnderTest.MemberWithoutDescriptionAttribute
```

And just like the Humanize API it honors the `Description` attribute. You don't have to provide the casing you provided during humanization: it figures it out.

###Humanize DateTime
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

Quite a few translations are available for humanized dates.

**No dehumanization for dates as the human friendly date is not reversible**

###Humanize TimeSpan
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

###Inflector methods
There are also a few inflector methods:

####Pluralize 
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

####Singularize 
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

####ToQuantity
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

####Ordinalize numbers & strings
`Ordinalize` turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th: 

```C#
1.Ordinalize() => "1st"
5.Ordinalize() => "5th"
```

You can also call `Ordinalize` on a numeric string and achieve the same result: `"21".Ordinalize()` => `"21st"`

####Underscore 
`Underscore` separates the input words with underscore; e.g. `"SomeTitle".Underscore()` => `"some_title"`

####Dasherize & Hyphenate
`Dasherize` and `Hyphenate` replace underscores with dashes in the string:

```C3
"some_title".Dasherize() => "some-title"
"some_title".Hyphenate() => "some-title"
```

###Fluent Date
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

###Number to words 
Humanizer can change numbers to words using the `ToWords` extension:

```C#
1.ToWords() => "one"
10.ToWords() => "ten"
11.ToWords() => "eleven"
122.ToWords() => "one hundred and twenty-two"
3501.ToWords() => "three thousand five hundred and one"
```

###Number to ordinal words
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

###Roman numerals
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

###Mix this into your framework to simplify your life
This is just a baseline and you can use this to simplify your day to day job. For example, in Asp.Net MVC we keep chucking `Display` attribute on ViewModel properties so `HtmlHelper` can generate correct labels for us; but, just like enums, in vast majority of cases we just need a space between the words in property name - so why not use `"string".Humanize` for that?! 

You may find an Asp.Net MVC sample [in the code](https://github.com/MehdiK/Humanizer/tree/master/src/Humanizer.MvcSample) that does that (although the project is excluded from the solution file to make the nuget package available for .Net 3.5 too). 

This is achieved using a custom `DataAnnotationsModelMetadataProvider` I called [HumanizerMetadataProvider](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.MvcSample/HumanizerMetadataProvider.cs). It is small enough to repeat here; so here we go:

```C#
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
```

This class calls the base class to extract the metadata and then, if required, humanizes the property name. It is checking if the property already has a `DisplayName` or `Display` attribute on it in which case the metadata provider will just honor the attribute and leave the property alone. For other properties it will Humanize the property name. That is all.

Now I need to register this metadata provider with Asp.Net MVC:

```C#
ModelMetadataProviders.Current = new HumanizerMetadataProvider();
```

... and now I can replace:

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

##How to contribute?
Your contribution to Humanizer would be very welcome. 
If you find a bug, please raise it as an issue. 
Even better fix it and send me a pull request. 
If you like to help me out with existing bugs and feature requests just check out the list of [issues](https://github.com/MehdiK/Humanizer/issues) and grab and fix one. 
I have also flagged some of the easier issues as 'jump in' so you can start with easier tasks.

###Contribution guideline
I use [GitHub flow](http://scottchacon.com/2011/08/31/github-flow.html) for pull requests. 
So if you want to contribute, fork the repo, preferrably create a local branch to avoid conflicts with other activities, fix an issue and send a PR.

Pull requests are code reviewed. Here is what I look for in your pull request:

 - Clean implementation
 - Very little or no comments because comments shouldn't be needed if you write clean code
 - Xml documentation for new extensions or updating the existing documentation when you make a change
 - Proper unit test coverage 
 - Adherence to the existing coding styles
 - Updated readme if you change an existing feature or add something
 - Add an entry in the release_notes.md file in the 'In Development' section with a link to your PR link and description of what's changed. Please follow the wording style for the description.

Also please link to the issue(s) you're fixing from your PR description.

###Need your help with localisation
One area Humanizer could always use your help is localisation. 
Currently Humanizer [supports](https://github.com/MehdiK/Humanizer/tree/master/src/Humanizer/Properties) quite a few localisations for `Date.Humanize` and a few for `TimeSpan.Humanize` methods. 
There is also ongoing effort to add localisation to `ToWords` extension method which currently only supports English and Arabic.

Humanizer could definitely do with more translations. To add a translation, fork the repository if you haven't done yet, duplicate the [resources.resx](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Properties/Resources.resx) file, add your target [locale code](http://msdn.microsoft.com/en-us/library/hh441729.aspx) to the end (e.g. resources.ru.resx for Russian), translate the values to your language, commit, and send a pull request for it. Thanks.

Some languages have complex rules when it comes to dealing with numbers; for example, in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and in Arabic "2 days" is "يومين" not "2 يوم".
Obviously a normal resource file doesn't cut it in these cases as a more complex mapping is required.
In cases like this in addition to creating a resource file you should also subclass [`DefaultFormatter`](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/DefaultFormatter.cs) in a class that represents your language; 
e.g. [RomanianFormatter](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Localisation/RomanianFormatter.cs) and then override the methods that need involve the complex rules. We think overriding the `GetResourceKey` method should be enough. To see how to do that check out `RomanianFormatter` and `RussianFormatter`. 
Then you return an instance of your class in the [Configurator](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Configuration/Configurator.cs) class in the getter of the [Formatter property](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer/Configuration/Configurator.cs#L11) based on the current culture.

Translations for ToWords method are currently done in code as there is a huge difference between the way different languages deal with number words.

### Continuous Integration from TeamCity
Humanizer project is built & tested continuously by TeamCity (more details [here](http://www.mehdi-khalili.com/continuous-integration-delivery-github-teamcity)). That applies to pull requests too. Shortly after you submit a PR you can check the build and test status notification on your PR. I would appreciate if you could send me green PRs.

The current build status on the CI server is <a href="http://teamcity.ginnivan.net/viewType.html?buildTypeId=Humanizer_CI&guest=1">
<img src="http://teamcity.ginnivan.net/app/rest/builds/buildType:(id:Humanizer_CI)/statusIcon"/></a>

###Author
Mehdi Khalili ([@MehdiKhalili](http://twitter.com/MehdiKhalili))

###License
Humanizer is released under the MIT License. See the [bundled LICENSE](https://github.com/MehdiK/Humanizer/blob/master/LICENSE) file for details.

