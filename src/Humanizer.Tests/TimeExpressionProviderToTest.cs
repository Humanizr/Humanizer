using Humanizer.Localisation;

namespace Humanizer.Tests
{
    public class TimeExpressionProviderToTest : DateTimeExpressionProvider
    {
        public override TimeExpressionPast GetPastTimeExpression()
        {
            return TimeExpressionPast.For;
        }

        public override TimeExpressionFuture GetFutureTimeExpression()
        {
            return TimeExpressionFuture.In;
        }
    }
}
