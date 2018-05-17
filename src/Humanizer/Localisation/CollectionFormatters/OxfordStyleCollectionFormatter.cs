using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Humanizer.Localisation.CollectionFormatters
{
    internal class OxfordStyleCollectionFormatter : DefaultCollectionFormatter
    {
        public OxfordStyleCollectionFormatter([CanBeNull] string defaultSeparator)
            : base(defaultSeparator ?? "and")
        {
        }

        protected override string GetConjunctionFormatString(int itemCount) => itemCount > 2 ? "{0}, {1} {2}" : "{0} {1} {2}";
    }
}