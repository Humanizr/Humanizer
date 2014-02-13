using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Humanizer.Pluralization;

namespace Humanizer.Pluralization
{
    internal class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
    {
        internal StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary)
            : base(firstToSecondDictionary)
        {
        }

        internal override bool ExistsInFirst(string value)
        {
            return base.ExistsInFirst(value.ToLowerInvariant());
        }

        internal override bool ExistsInSecond(string value)
        {
            return base.ExistsInSecond(value.ToLowerInvariant());
        }

        internal override string GetFirstValue(string value)
        {
            return base.GetFirstValue(value.ToLowerInvariant());
        }

        internal override string GetSecondValue(string value)
        {
            return base.GetSecondValue(value.ToLowerInvariant());
        }
    }

}
