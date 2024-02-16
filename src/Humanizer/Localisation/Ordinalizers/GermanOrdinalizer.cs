namespace Humanizer
{
    internal class GermanOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString) =>
            numberString + ".";
    }
}
