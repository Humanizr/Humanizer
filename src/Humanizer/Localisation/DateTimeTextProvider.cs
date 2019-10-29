namespace Humanizer.Localisation
{
    public class DateTimeExpressionProvider
    {
        private static DateTimeExpressionProvider _dateTimeTextProvider;

        public static DateTimeExpressionProvider Instance 
        { 
            get {
                if (_dateTimeTextProvider == null)
                {
                    _dateTimeTextProvider = new DateTimeExpressionProvider();
                }

                return _dateTimeTextProvider;
            } 
        }

        public DateTimeExpressionProvider()
        {
            //You cannot instance this
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
