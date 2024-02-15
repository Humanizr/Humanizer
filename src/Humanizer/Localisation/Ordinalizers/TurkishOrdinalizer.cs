namespace Humanizer
{
    internal class TurkishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return numberString + ".";
        }
    }
}
