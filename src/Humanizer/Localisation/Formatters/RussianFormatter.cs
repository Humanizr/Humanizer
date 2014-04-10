namespace Humanizer.Localisation.Formatters
{
    internal class RussianFormatter : DefaultFormatter
    {
        private const string SingularPostfix = "_Singular";
        private const string PaucalPostfix = "_Paucal";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var mod100 = number%100;
            if (mod100/10 != 1)
            {
                var mod10 = number%10;

                if (mod10 == 1) // 1, 21, 31, 41 ... 91, 101, 121 ..
                    return resourceKey + SingularPostfix;

                if (mod10 > 1 && mod10 < 5) // 2, 3, 4, 22, 23, 24 ...
                    return resourceKey + PaucalPostfix;
            }
            
            return resourceKey;
        }
    }
}
