//The Inflector class was cloned from Inflector (https://github.com/srkirkland/Inflector)

//The MIT License (MIT)

//Copyright (c) 2013 Scott Kirkland

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections;

public class InflectorTests
{
    public readonly IList<object[]> PluralTestData = [];

    [Theory]
    [ClassData(typeof(PluralTestSource))]
    public void Pluralize(string singular, string plural) =>
        Assert.Equal(plural, singular.Pluralize());

    [Theory]
    [ClassData(typeof(PluralTestSource))]
    public void PluralizeWordsWithUnknownPlurality(string singular, string plural)
    {
        Assert.Equal(plural, plural.Pluralize(false));
        Assert.Equal(plural, singular.Pluralize(false));
    }

    [Theory]
    [ClassData(typeof(PluralTestSource))]
    public void Singularize(string singular, string plural) =>
        Assert.Equal(singular, plural.Singularize());

    [Theory]
    [ClassData(typeof(PluralTestSource))]
    public void SingularizeWordsWithUnknownSingularity(string singular, string plural)
    {
        Assert.Equal(singular, singular.Singularize(false));
        Assert.Equal(singular, plural.Singularize(false));
    }

    [Theory]
    [InlineData("tires", "tires")]
    [InlineData("body", "bodies")]
    [InlineData("traxxas", "traxxas")]
    public void SingularizeSkipSimpleWords(string singular, string plural) =>
        Assert.Equal(singular, plural.Singularize(skipSimpleWords: true));

    [Theory]
    [InlineData("a")]
    [InlineData("A")]
    [InlineData("s")]
    [InlineData("S")]
    [InlineData("z")]
    [InlineData("Z")]
    [InlineData("1")]
    public void SingularizeSingleLetter(string input) =>
        Assert.Equal(input, input.Singularize());

    //Uppercases individual words and removes some characters
    [Theory]
    [InlineData("some title", "Some Title")]
    [InlineData("some-title", "Some Title")]
    [InlineData("sometitle", "Sometitle")]
    [InlineData("some-title: The beginning", "Some Title: The Beginning")]
    [InlineData("some_title:_the_beginning", "Some Title: the Beginning")]
    [InlineData("some title: The_beginning", "Some Title: The Beginning")]
    public void Titleize(string input, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Titleize());

    // Issue #385: Titleize should preserve input with no recognized letters
    [Theory]
    [InlineData("Майк", "Майк")]
    [InlineData("@@", "@@")]
    [InlineData("?", "?")]
    [InlineData("123", "123")]
    public void TitleizeShouldPreserveUnrecognizedCharacters(string input, string expected) =>
        Assert.Equal(expected, input.Titleize());

    [InlineData("some_title", "some-title")]
    [InlineData("some-title", "some-title")]
    [InlineData("some_title_goes_here", "some-title-goes-here")]
    [InlineData("some_title and_another", "some-title and-another")]
    [Theory]
    public void Dasherize(string input, string expectedOutput) =>
        Assert.Equal(input.Dasherize(), expectedOutput);

    [InlineData("some_title", "some-title")]
    [InlineData("some-title", "some-title")]
    [InlineData("some_title_goes_here", "some-title-goes-here")]
    [InlineData("some_title and_another", "some-title and-another")]
    [Theory]
    public void Hyphenate(string input, string expectedOutput) =>
        Assert.Equal(input.Hyphenate(), expectedOutput);

    [Theory]
    [InlineData("customer", "Customer")]
    [InlineData("CUSTOMER", "CUSTOMER")]
    [InlineData("CUStomer", "CUStomer")]
    [InlineData("customer_name", "CustomerName")]
    [InlineData("customer_first_name", "CustomerFirstName")]
    [InlineData("customer_first_name goes here", "CustomerFirstNameGoesHere")]
    [InlineData("customer name", "CustomerName")]
    [InlineData("customer   name", "CustomerName")]
    [InlineData("customer-first-name", "CustomerFirstName")]
    [InlineData("_customer-first-name", "CustomerFirstName")]
    [InlineData(" customer__first--name", "CustomerFirstName")]
    public void Pascalize(string input, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Pascalize());

    // Same as pascalize, except first char is lowercase
    [Theory]
    [InlineData("customer", "customer")]
    [InlineData("CUSTOMER", "cUSTOMER")]
    [InlineData("CUStomer", "cUStomer")]
    [InlineData("customer_name", "customerName")]
    [InlineData("customer_first_name", "customerFirstName")]
    [InlineData("customer_first_name goes here", "customerFirstNameGoesHere")]
    [InlineData("customer name", "customerName")]
    [InlineData("customer   name", "customerName")]
    [InlineData("", "")]
    public void Camelize(string input, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Camelize());

    //Makes an underscored lowercase string
    [Theory]
    [InlineData("SomeTitle", "some_title")]
    [InlineData("someTitle", "some_title")]
    [InlineData("some title", "some_title")]
    [InlineData("some title that will be underscored", "some_title_that_will_be_underscored")]
    [InlineData("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored")]
    [InlineData("SomeForeignWordsLikeÄgyptenÑu", "some_foreign_words_like_ägypten_ñu")]
    [InlineData("Some wordsTo be Underscored", "some_words_to_be_underscored")]
    public void Underscore(string input, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Underscore());

    // transform words into lowercase and separate with a -
    [Theory]
    [InlineData("SomeWords", "some-words")]
    [InlineData("SOME words TOGETHER", "some-words-together")]
    [InlineData("A spanish word EL niño", "a-spanish-word-el-niño")]
    [InlineData("SomeForeignWords ÆgÑuÄgypten", "some-foreign-words-æg-ñu-ägypten")]
    [InlineData("A VeryShortSENTENCE", "a-very-short-sentence")]
    public void Kebaberize(string input, string expectedOutput) =>
        Assert.Equal(expectedOutput, input.Kebaberize());
}

class PluralTestSource : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["search", "searches"];
        yield return ["switch", "switches"];
        yield return ["fix", "fixes"];
        yield return ["box", "boxes"];
        yield return ["process", "processes"];
        yield return ["address", "addresses"];
        yield return ["case", "cases"];
        yield return ["stack", "stacks"];
        yield return ["wish", "wishes"];
        yield return ["fish", "fish"];

        yield return ["category", "categories"];
        yield return ["query", "queries"];
        yield return ["ability", "abilities"];
        yield return ["agency", "agencies"];
        yield return ["movie", "movies"];

        yield return ["archive", "archives"];

        yield return ["index", "indices"];

        yield return ["wife", "wives"];
        yield return ["safe", "saves"];
        yield return ["half", "halves"];

        yield return ["glove", "gloves"];
        yield return ["move", "moves"];

        yield return ["salesperson", "salespeople"];
        yield return ["person", "people"];

        yield return ["spokesman", "spokesmen"];
        yield return ["man", "men"];
        yield return ["woman", "women"];
        yield return ["freshman", "freshmen"];
        yield return ["chairman", "chairmen"];
        yield return ["human", "humans"];
        yield return ["personnel", "personnel"];
        yield return ["staff", "staff"];
        yield return ["training", "training"];

        yield return ["basis", "bases"];
        yield return ["diagnosis", "diagnoses"];

        yield return ["datum", "data"];
        yield return ["medium", "media"];
        yield return ["analysis", "analyses"];

        yield return ["node_child", "node_children"];
        yield return ["child", "children"];

        yield return ["experience", "experiences"];
        yield return ["day", "days"];

        yield return ["comment", "comments"];
        yield return ["foobar", "foobars"];
        yield return ["newsletter", "newsletters"];

        yield return ["old_news", "old_news"];
        yield return ["news", "news"];

        yield return ["series", "series"];
        yield return ["species", "species"];

        yield return ["quiz", "quizzes"];

        yield return ["perspective", "perspectives"];

        yield return ["ox", "oxen"];
        yield return ["photo", "photos"];
        yield return ["buffalo", "buffaloes"];
        yield return ["tomato", "tomatoes"];
        yield return ["dwarf", "dwarves"];
        yield return ["elf", "elves"];
        yield return ["information", "information"];
        yield return ["equipment", "equipment"];
        yield return ["bus", "buses"];
        yield return ["status", "statuses"];
        yield return ["status_code", "status_codes"];
        yield return ["mouse", "mice"];

        yield return ["louse", "lice"];
        yield return ["house", "houses"];
        yield return ["octopus", "octopi"];
        yield return ["alias", "aliases"];
        yield return ["portfolio", "portfolios"];
        yield return ["criterion", "criteria"];

        yield return ["vertex", "vertices"];
        yield return ["matrix", "matrices"];

        yield return ["axis", "axes"];
        yield return ["testis", "testes"];
        yield return ["crisis", "crises"];

        yield return ["corn", "corn"];
        yield return ["milk", "milk"];
        yield return ["rice", "rice"];
        yield return ["shoe", "shoes"];

        yield return ["horse", "horses"];
        yield return ["prize", "prizes"];
        yield return ["edge", "edges"];

        /* Tests added by Bas Jansen */
        yield return ["goose", "geese"];
        yield return ["deer", "deer"];
        yield return ["sheep", "sheep"];
        yield return ["wolf", "wolves"];
        yield return ["volcano", "volcanoes"];
        yield return ["aircraft", "aircraft"];
        yield return ["alumna", "alumnae"];
        yield return ["alumnus", "alumni"];
        yield return ["fungus", "fungi"];

        yield return ["water", "water"];
        yield return ["waters", "waters"];
        yield return ["semen", "semen"];
        yield return ["sperm", "sperm"];

        yield return ["wave", "waves"];

        yield return ["campus", "campuses"];

        yield return ["is", "are"];

        yield return ["addendum", "addenda"];
        yield return ["alga", "algae"];
        yield return ["appendix", "appendices"];
        yield return ["bias", "biases"];
        yield return ["bison", "bison"];
        yield return ["blitz", "blitzes"];
        yield return ["buzz", "buzzes"];
        yield return ["cactus", "cacti"];
        yield return ["corps", "corps"];
        yield return ["curriculum", "curricula"];
        yield return ["die", "dice"];
        yield return ["echo", "echoes"];
        yield return ["ellipsis", "ellipses"];
        yield return ["elk", "elk"];
        yield return ["emphasis", "emphases"];
        yield return ["embargo", "embargoes"];
        yield return ["focus", "foci"];
        yield return ["foot", "feet"];
        yield return ["fuse", "fuses"];
        yield return ["grass", "grass"];
        yield return ["hair", "hair"];
        yield return ["hero", "heroes"];
        yield return ["hippopotamus", "hippopotami"];
        yield return ["hoof", "hooves"];
        yield return ["iris", "irises"];
        yield return ["larva", "larvae"];
        yield return ["leaf", "leaves"];
        yield return ["loaf", "loaves"];
        yield return ["luggage", "luggage"];
        yield return ["means", "means"];
        yield return ["mail", "mail"];
        yield return ["millennium", "millennia"];
        yield return ["moose", "moose"];
        yield return ["mosquito", "mosquitoes"];
        yield return ["mud", "mud"];
        yield return ["nucleus", "nuclei"];
        yield return ["neurosis", "neuroses"];
        yield return ["oasis", "oases"];
        yield return ["offspring", "offspring"];
        yield return ["paralysis", "paralyses"];
        yield return ["phenomenon", "phenomena"];
        yield return ["potato", "potatoes"];
        yield return ["radius", "radii"];
        yield return ["salmon", "salmon"];
        yield return ["scissors", "scissors"];
        yield return ["shrimp", "shrimp"];
        yield return ["someone", "someone"];
        yield return ["stimulus", "stimuli"];
        yield return ["swine", "swine"];
        yield return ["syllabus", "syllabi"];
        yield return ["that", "those"];
        yield return ["thief", "thieves"];
        yield return ["this", "these"];
        yield return ["tie", "ties"];
        yield return ["tooth", "teeth"];
        yield return ["torpedo", "torpedoes"];
        yield return ["trellis", "trellises"];
        yield return ["trout", "trout"];
        yield return ["tuna", "tuna"];
        yield return ["vertebra", "vertebrae"];
        yield return ["veto", "vetoes"];
        yield return ["virus", "viruses"];
        yield return ["walrus", "walruses"];
        yield return ["waltz", "waltzes"];
        yield return ["zombie", "zombies"];

        yield return ["cookie", "cookies"];
        yield return ["bookie", "bookies"];
        yield return ["rookie", "rookies"];
        yield return ["roomie", "roomies"];
        yield return ["smoothie", "smoothies"];

        //Issue #789
        yield return ["cache", "caches"];

        //Issue #975, added by Alex Boutin
        yield return ["ex", "exes"];
        yield return ["", ""];

        //Issue #1100
        yield return ["doe", "does"];
        yield return ["hoe", "hoes"];
        yield return ["toe", "toes"];
        yield return ["woe", "woes"];

        //Issue #1132
        yield return ["metadata", "metadata"];

        //Issue #1154
        yield return ["a", "as"];
        yield return ["A", "As"];
        yield return ["s", "ss"];
        yield return ["S", "Ss"];
        yield return ["z", "zs"];
        yield return ["Z", "Zs"];
        yield return ["1", "1s"];

        //Issue #1252
        yield return ["pliers", "pliers"];
        yield return ["sheers", "sheers"];
        yield return ["valve", "valves"];
        yield return ["clothes", "clothes"];
        yield return ["lens", "lenses"];
        yield return ["apparatus", "apparatus"];
        yield return ["clove", "cloves"];
        yield return ["chassis", "chassis"];
        yield return ["explosive", "explosives"];
        yield return ["debris", "debris"];

        //Issue #1042
        yield return ["database", "databases"];
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}