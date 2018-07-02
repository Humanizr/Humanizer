using Xunit;

namespace Humanizer.Tests.Localisation.nb
{
    [UseCulture("nb-NO")]
    public class NumberToWordsTests
    {
        [Theory]
        [InlineData(0, "null")]
        [InlineData(1, "en")]
        [InlineData(2, "to")]
        [InlineData(3, "tre")]
        [InlineData(4, "fire")]
        [InlineData(5, "fem")]
        [InlineData(6, "seks")]
        [InlineData(7, "sju")]
        [InlineData(8, "åtte")]
        [InlineData(9, "ni")]
        [InlineData(10, "ti")]
        [InlineData(20, "tjue")]
        [InlineData(30, "tretti")]
        [InlineData(40, "førti")]
        [InlineData(50, "femti")]
        [InlineData(60, "seksti")]
        [InlineData(70, "sytti")]
        [InlineData(80, "åtti")]
        [InlineData(90, "nitti")]
        [InlineData(98, "nittiåtte")]
        [InlineData(99, "nittini")]
        [InlineData(100, "hundre")]
        [InlineData(200, "tohundre")]
        [InlineData(1000, "tusen")]
        [InlineData(100000, "hundretusen")]
        [InlineData(1000000, "en million")]
        [InlineData(10000000, "ti millioner")]
        [InlineData(100000000, "hundre millioner")]
        [InlineData(1000000000, "en milliard")]
        [InlineData(2000000000, "to milliarder")]
        [InlineData(122, "hundreogtjueto")]
        [InlineData(3501, "tretusenfemhundreogen")]
        [InlineData(111, "hundreogelleve")]
        [InlineData(1001, "tusenogen")]
        [InlineData(1099, "tusenognittini")]
        [InlineData(1100, "ettusenethundre")]
        [InlineData(1112, "ettusenethundreogtolv")]
        [InlineData(11213, "ellevetusentohundreogtretten")]
        [InlineData(121314, "hundreogtjueentusentrehundreogfjorten")]
        [InlineData(2132415, "to millioner hundreogtrettitotusenfirehundreogfemten")]
        [InlineData(12345516, "tolv millioner trehundreogførtifemtusenfemhundreogseksten")]
        [InlineData(751633617, "sjuhundreogfemtien millioner sekshundreogtrettitretusensekshundreogsytten")]
        [InlineData(1111111118, "en milliard hundreogelleve millioner hundreogellevetusenethundreogatten")]
        [InlineData(-751633619, "minus sjuhundreogfemtien millioner sekshundreogtrettitretusensekshundreognitten")]
        [InlineData(1000010, "en million og ti")]
        [InlineData(1001009, "en million tusenogni")]
        [InlineData(1000099, "en million og nittini")]
        [InlineData(1000000010, "en milliard og ti")]
        [InlineData(1000110, "en million ethundreogti")]
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "nullte")]
        [InlineData(1, "første")]
        [InlineData(2, "andre")]
        [InlineData(3, "tredje")]
        [InlineData(4, "fjerde")]
        [InlineData(5, "femte")]
        [InlineData(6, "sjette")]
        [InlineData(7, "sjuende")]
        [InlineData(8, "åttende")]
        [InlineData(9, "niende")]
        [InlineData(10, "tiende")]
        [InlineData(20, "tjuende")]
        [InlineData(30, "trettiende")]
        [InlineData(40, "førtiende")]
        [InlineData(50, "femtiende")]
        [InlineData(60, "sekstiende")]
        [InlineData(70, "syttiende")]
        [InlineData(80, "åttiende")]
        [InlineData(90, "nittiende")]
        [InlineData(98, "nittiåttende")]
        [InlineData(99, "nittiniende")]
        [InlineData(100, "hundrede")]
        [InlineData(200, "tohundrede")]
        [InlineData(1000, "tusende")]
        [InlineData(10000, "titusende")]
        [InlineData(100000, "hundretusende")]
        [InlineData(1000000, "millionte")]
        [InlineData(10000000, "ti millionte")]
        [InlineData(100000000, "hundre millionte")]
        [InlineData(1000000000, "milliardte")]
        [InlineData(2000000000, "to milliardte")]
        [InlineData(122, "hundreogtjueandre")]
        [InlineData(3501, "tretusenfemhundreogførste")]
        [InlineData(111, "hundreogellevte")]
        [InlineData(1112, "ettusenethundreogtolvte")]
        [InlineData(11213, "ellevetusentohundreogtrettende")]
        [InlineData(121314, "hundreogtjueentusentrehundreogfjortende")]
        [InlineData(2132415, "to millioner hundreogtrettitotusenfirehundreogfemtende")]
        [InlineData(12345516, "tolv millioner trehundreogførtifemtusenfemhundreogsekstende")]
        [InlineData(751633617, "sjuhundreogfemtien millioner sekshundreogtrettitretusensekshundreogsyttende")]
        [InlineData(1111111118, "en milliard hundreogelleve millioner hundreogellevetusenethundreogattende")]
        [InlineData(-751633619, "minus sjuhundreogfemtien millioner sekshundreogtrettitretusensekshundreognittende")]
        [InlineData(1000010, "en million og tiende")]
        [InlineData(1001009, "en million tusenogniende")]
        [InlineData(1000099, "en million og nittiniende")]
        [InlineData(1000000010, "en milliard og tiende")]
        [InlineData(1000110, "en million ethundreogtiende")]
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }

        [Theory]
        [InlineData(2, "to")]
        [InlineData(1, "ei")]
        [InlineData(0, "null")]
        [InlineData(-1, "minus ei")]
        [InlineData(-2, "minus to")]
        public void ToWordsFeminine(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Feminine));
        }

        [Theory]
        [InlineData(2, "to")]
        [InlineData(1, "et")]
        [InlineData(0, "null")]
        [InlineData(-1, "minus et")]
        [InlineData(-2, "minus to")]
        public void ToWordsNeuter(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords(GrammaticalGender.Neuter));
        }
    }
}
