namespace Humanizer
{
    internal class ToSentenceCase : IStringTransformer
    {
        public string Transform(string input)
        {
            if (input.Length >= 1)
            {
                return string.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));
            }

            return input.ToUpper();
        }
    }
}