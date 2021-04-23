using Xunit;

namespace Humanizer.Tests.Localisation.el
{
    [UseCulture("el")]
    public class NumberToWordsTests
    {
        [InlineData(1, "ένα")]
        [InlineData(10, "δέκα")]
        [InlineData(11, "έντεκα")]
        [InlineData(14, "δεκατέσσερα")]
        [InlineData(20, "είκοσι")]
        [InlineData(122, "εκατόν είκοσι δύο")]
        [InlineData(3501, "τρείς χιλιάδες πεντακόσια ένα")]
        [InlineData(100, "εκατό")]
        [InlineData(1000, "χίλια")]
        [InlineData(100000, "εκατό χιλιάδες")]
        [InlineData(13448, "δεκατρείς χιλιάδες τετρακόσια σαράντα οκτώ")]
        [InlineData(53, "πενήντα τρία")]
        [InlineData(123647, "εκατόν είκοσι τρείς χιλιάδες εξακόσια σαράντα επτά")]
        [InlineData(14000000, "δεκατέσσερα εκατομμύρια")]
        [InlineData(578412, "πεντακόσιες εβδομήντα οκτώ χιλιάδες τετρακόσια δώδεκα")]
        [InlineData(1000000000, "ένα δισεκατομμύριο")]
        [InlineData(1000000001, "ένα δισεκατομμύριο ένα")]
        [InlineData(1469, "χίλια τετρακόσια εξήντα εννέα")]
        [InlineData(69, "εξήντα εννέα")]
        [InlineData(619, "εξακόσια δεκαεννέα")]
        [InlineData(1190, "χίλια εκατόν ενενήντα")]

        [Theory]
        public void ToWordsInt(int number, string expected)
        {
            Assert.Equal(expected, number.ToWords());
        }
    }
}
