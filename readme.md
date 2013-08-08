Humanizer is a small framework that helps .Net developer turn their otherwise geeky strings, type names, enum fields, date fields ETC into a human friendly format.

##Installation
You can install Humanizer as [a nuget package](https://nuget.org/packages/Humanizer): `Install-Package Humanizer`

##Usage
Humanizer is a set of extension methods, currently available on `String`, `Enum`, `DateTime` and `int` and turns them from computer friendly into human friendly format (and vice versa).

###Humanize Strings###
String extensions are at the heart of this micro-framework. The foundation of this was set in the [BDDfy framework](https://github.com/TestStack/TestStack.BDDfy) where class names, method names and properties are turned into human readable sentences. 

    "PascalCaseInputStringIsTurnedIntoSentence".Humanize() => "Pascal case input string is turned into sentence"
    
    "Underscored_input_string_is_turned_into_sentence".Humanize() => "Underscored input string is turned into sentence"
    
    "Underscored_input_String_is_turned_INTO_sentence".Humanize() => "Underscored input String is turned INTO sentence"
    
    "HTML".Humanize() => "HTML" // acronyms are left intact

You may also specify the desired letter casing:

    "CanReturnTitleCase".Humanize(LetterCasing.Title) => "Can Return Title Case"
    
    "Can_return_title_Case".Humanize(LetterCasing.Title) => "Can Return Title Case"
    
    "CanReturnLowerCase".Humanize(LetterCasing.LowerCase) => "can return lower case"
    
    "CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps) => "CAN HUMANIZE INTO UPPER CASE"

####Dehumanize Strings####
Much like you can humanize a computer friendly into human friendly string you can dehumanize a human friendly string into a computer friendly one:

    "Pascal case input string is turned into sentence".Humanize() => "PascalCaseInputStringIsTurnedIntoSentence"

###Humanize Enums###
Calling `ToString` directly on enum members usually results in less than ideal output for users. The solution to this is usually to use `DescriptionAttribute` data annotation and then read that at runtime to get a more friendly output. That is a great solution; but more often than not we only need to put some space between words of an enum member - which is what `String.Humanize()` does well. For an enum like:

    public enum EnumUnderTest
    {
        [Description("Custom description")]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute,
        ALLCAPITALS
    }

You will get:

    EnumUnderTest.MemberWithDescriptionAttribute.Humanize() => "Custom description"; // DescriptionAttribute is honored
    
    EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize() => "Member without description attribute"; // in the absence of Description attribute string.Humanizer kicks in
    
    EnumUnderTest.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.Title) => "Member Without Description Attribute"; // an of course you can still apply letter casing 

Hopefully this will help avoid littering enums with unnecessary attributes!

####Dehumanize Enums####
Dehumanizes a string into the Enum it was originally Humanized from! The API looks like:

    public static Enum DehumanizeTo<TTargetEnum>(this string input) 

And the usage is:

	"Member without description attribute".Dehumanize() => EnumUnderTest.MemberWithoutDescriptionAttribute

And just like the Humanize API it honors the `Description` attribute. You don't have to provide the casing you provided during humanization: it figures it out.

###Humanize Dates###
This is borrowed from [StackOverFlow algorithm](http://stackoverflow.com/a/12/141101) - although I had to apply some minor fixes on top of it. I am not going to bore you with all the examples as I am sure you know what this does: you basically give it an instance of `DateTime` and get back a string telling how far back in time that is:

    DateTime.UtcNow.AddHours(-30).Humanize() => "yesterday"

Humanizer supports local as well as UTC dates. You could also provide the date you want the input date to be compared against. If null, it will use the current date as comparison base. Here is the API signature:

    public static string Humanize(this DateTime input, bool utcDate = true, DateTime? dateToCompareAgainst = null)

For dates Humanizer also supports localization.

**No dehumanization for dates as the human friendly date is not reversible**

###Inflector methods###
There are also a few inflector methods:

 * Pluralize: pluralizes the provided input considering irregular words; e.g. `"Man".Pluralize()` -> `"Men"` & `"string".Pluralize()` -> `"strings"`
 * Singularize: singularizes the provided input considering irregular words; e.g. `"Men".Singularize()` -> `"Man"` & `"strings".Singularize()` -> `"string"`
 * Ordinalize numbers: turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th; e.g. `1.Ordinalize()` -> `"1st"`, `5.Ordinalize()` -> `"5th"`
 * Ordinalize strings: Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th; e.g. `"21".Ordinalize()` -> `"21st"`
 * Underscore: separates the input words with underscore; e.g. `"SomeTitle".Underscore()` -> `"some_title"`
 * Dasherize: replaces underscores with dashes in the string; e.g. `"some_title".Dasherize()` -> `"some-title"`

##What else?##
This is just a baseline and you can use this to simplify your day to day job. For example, in Asp.Net MVC we keep chucking `Display` attribute on ViewModel properties so `HtmlHelper` can generate correct labels for us; but, just like enums, in vast majority of cases we just need a space between the words in property name - so why not use string.Humanizer for that?! 

You may find an Asp.Net MVC sample [in the code][5] that does that (although the project is excluded from the solution file to make the nuget package available for .Net 3.5 too). 

This is achieved using a custom `DataAnnotationsModelMetadataProvider` I called `[HumanizerMetadataProvider](https://github.com/MehdiK/Humanizer/blob/master/src/Humanizer.MvcSample/HumanizerMetadataProvider.cs)`. It is small enough to repeat here; so here we go:

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

This class calls the base class to extract the metadata and then, if required, humanizes the property name. It is checking if the property already has a `DisplayName` or `Display` attribute on it in which case the metadata provider will just honor the attribute and leave the property alone. For other properties it will Humanize the property name. That is all.

Now I need to register this metadata provider with Asp.Net MVC:

    ModelMetadataProviders.Current = new HumanizerMetadataProvider();

... and now I can replace:

    public class RegisterModel
    {
        [Display(Name = "User name")]
        public string UserName { get; set; }
    
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }

with:

    public class RegisterModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ConfirmPassword { get; set; }
    }

... and the "metadata humanizer" will take care of the rest.

No need to mention that if you want title casing for your labels you may call the other overload of `Humanize` method:

    modelMetadata.DisplayName = modelMetadata.PropertyName.Humanize(LetterCasing.Title));

### Building & running the Tests
You can build the whole solution, run the tests and create a local nuget package for the library by running the `go.cmd` on the root of the repo.

###Author
Mehdi Khalili (@MehdiK)

###License
Humanizer is released under the MIT License. See the bundled LICENSE file for details.