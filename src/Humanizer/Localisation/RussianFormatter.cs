namespace Humanizer.Localisation
{
    internal class RussianFormatter : DefaultFormatter
    {
        private const string Ago = "{0} {1} назад";

        private static string Select(int number, string singular, string paucal, string plural)
        {
            var mod100 = number % 100;
            if (mod100 / 10 != 1)
            {
                var mod10 = number % 10;

                if (mod10 == 1) // 1, 21, 31, 41 ... 91, 101, 121 ..
                {
                    return singular;
                }

                if (mod10 > 1 && mod10 < 5) // 2, 3, 4, 22, 23, 24 ...
                {
                    return paucal;
                }
            }

            return plural;
        }

        public override string DateHumanize_one_second_ago()
        {
            return "секунду назад";
        }

        public override string DateHumanize__seconds_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "секунду", "секунды", "секунд"));
        }

        public override string DateHumanize_a_minute_ago()
        {
            return "минуту назад";
        }

        public override string DateHumanize__minutes_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "минуту", "минуты", "минут"));
        }

        public override string DateHumanize_an_hour_ago()
        {
            return "час назад";
        }

        public override string DateHumanize__hours_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "час", "часа", "часов"));
        }

        public override string DateHumanize_yesterday()
        {
            return "вчера";
        }

        public override string DateHumanize__days_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "день", "дня", "дней"));
        }

        public override string DateHumanize_one_month_ago()
        {
            return "месяц назад";
        }

        public override string DateHumanize__months_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "месяц", "месяца", "месяцев"));
        }

        public override string DateHumanize_one_year_ago()
        {
            return "год назад";
        }

        public override string DateHumanize__years_ago(int number)
        {
            return string.Format(Ago, number, Select(number, "год", "года", "лет"));
        }

        public override string DateHumanize_not_yet()
        {
            return "в будущем";
        }
    }
}
