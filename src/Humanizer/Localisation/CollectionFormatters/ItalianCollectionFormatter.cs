using System;
using System.Collections.Generic;
using System.Linq;

namespace Humanizer.Localisation.CollectionFormatters
{
    internal class ItalianCollectionFormatter : EnglishCollectionFormatter
    {
        public ItalianCollectionFormatter()
            : base()
        {
            DefaultSeparator = "e";
        }
    }
}
