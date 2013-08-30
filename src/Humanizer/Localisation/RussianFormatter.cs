namespace Humanizer.Localisation
{
    internal class RussianFormatter : DefaultFormatter
    {
        private static string Format(string resourceKey, int number)
        {
            var format = Resources.GetResource(GetResourceKey(resourceKey, number));

            return string.Format(format, number);
        }

        private static string GetResourceKey(string resourceKey, int number)
        {
            var mod100 = number%100;
            if (mod100/10 != 1)
            {
                var mod10 = number%10;

                if (mod10 == 1) // 1, 21, 31, 41 ... 91, 101, 121 ..
                {
                    return resourceKey + "_singular";
                }

                if (mod10 > 1 && mod10 < 5) // 2, 3, 4, 22, 23, 24 ...
                {
                    return resourceKey + "_paucal";
                }
            }
            return resourceKey;
        }

        public override string DateHumanize__seconds_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__seconds_ago, number);
        }

        public override string DateHumanize__minutes_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__minutes_ago, number);
        }

        public override string DateHumanize__hours_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__hours_ago, number);
        }

        public override string DateHumanize__days_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__days_ago, number);
        }

        public override string DateHumanize__months_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__months_ago, number);
        }

        public override string DateHumanize__years_ago(int number)
        {
            return Format(ResourceKeys.DateHumanize__years_ago, number);
        }
    }
}
