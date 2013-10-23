namespace Humanizer
{
    public class ToUpperCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return input.ToUpper();
        }
    }
}