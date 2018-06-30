using Xunit;

namespace Humanizer.Tests.Localisation.fiFI
{
    [UseCulture("fi-FI")]
    public class NumberToWordsTests
    {

        [Theory]
        [InlineData(0, "nolla")]
        [InlineData(1, "yksi")]
        [InlineData(11, "yksitoista")]
        [InlineData(15, "viisitoista")]
        [InlineData(19, "yhdeksäntoista")]
        [InlineData(20, "kaksikymmentä")]
        [InlineData(25, "kaksikymmentäviisi")]
        [InlineData(50, "viisikymmentä")]
        [InlineData(90, "yhdeksänkymmentä")]
        [InlineData(100, "sata")]
        [InlineData(101, "satayksi")]
        [InlineData(345, "kolmesataaneljäkymmentäviisi")]
        [InlineData(678, "kuusisataaseitsemänkymmentäkahdeksan")]
        [InlineData(1000, "tuhat")]
        [InlineData(1001, "tuhat yksi")]
        [InlineData(1234, "tuhat kaksisataakolmekymmentäneljä")]
        [InlineData(4567, "neljätuhatta viisisataakuusikymmentäseitsemän")]
        [InlineData(10000, "kymmenentuhatta")]
        [InlineData(100000, "satatuhatta")]
        [InlineData(1000000, "miljoona")]
        [InlineData(10000000, "kymmenenmiljoonaa")]
        [InlineData(100000000, "satamiljoonaa")]
        [InlineData(1000000000, "miljardi")]
        [InlineData(2147483647, "kaksimiljardia sataneljäkymmentäseitsemänmiljoonaa neljäsataakahdeksankymmentäkolmetuhatta kuusisataaneljäkymmentäseitsemän")]  // int.MaxValue
        [InlineData(-2147483647, "miinus kaksimiljardia sataneljäkymmentäseitsemänmiljoonaa neljäsataakahdeksankymmentäkolmetuhatta kuusisataaneljäkymmentäseitsemän")]  // int.MinValue + 1
        public void ToWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }

        [Theory]
        [InlineData(0, "nollas")]
        [InlineData(1, "ensimmäinen")]
        [InlineData(2, "toinen")]
        [InlineData(10, "kymmenes")]
        [InlineData(11, "yhdestoista")]
        [InlineData(12, "kahdestoista")]
        [InlineData(19, "yhdeksästoista")]
        [InlineData(20, "kahdeskymmenes")]
        [InlineData(21, "kahdeskymmenesensimmäinen")]
        [InlineData(22, "kahdeskymmenestoinen")]
        [InlineData(28, "kahdeskymmeneskahdeksas")]
        [InlineData(75, "seitsemäskymmenesviides")]
        [InlineData(100, "sadas")]
        [InlineData(101, "sadasensimmäinen")]
        [InlineData(111, "sadasyhdestoista")]
        [InlineData(1000, "tuhannes")]
        [InlineData(1101, "tuhannessadasensimmäinen")]
        [InlineData(10000, "kymmenestuhannes")]
        [InlineData(100000, "sadastuhannes")]
        [InlineData(1000000, "miljoonas")]
        [InlineData(10000000, "kymmenesmiljoonas")]
        [InlineData(100000000, "sadasmiljoonas")]
        [InlineData(1000000000, "miljardis")]
        [InlineData(1000000001, "miljardisensimmäinen")]
        [InlineData(2147483647, "kahdesmiljardissadasneljäskymmenesseitsemäsmiljoonasneljässadaskahdeksaskymmeneskolmastuhanneskuudessadasneljäskymmenesseitsemäs")] // int.MaxValue
        public void ToOrdinalWords(int number, string expected)
        {
            Assert.Equal(expected, number.ToOrdinalWords());
        }
    }
}
