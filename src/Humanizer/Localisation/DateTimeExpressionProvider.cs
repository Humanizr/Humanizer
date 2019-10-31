using System;

namespace Humanizer.Localisation
{
    public class DateTimeExpressionProvider
    {
        private static readonly DateTimeExpressionProvider _dateTimeExpressionProvider = new DateTimeExpressionProvider();

        public static DateTimeExpressionProvider Default
        {
            get { return _dateTimeExpressionProvider; }
        }

        public virtual TimeExpressionFuture GetFutureTimeExpression()
        {
            return TimeExpressionFuture.FromNow;
        }

        public virtual TimeExpressionPast GetPastTimeExpression()
        {
            return TimeExpressionPast.Ago;
        }
    }
}
