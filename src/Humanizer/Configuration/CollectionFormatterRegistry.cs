﻿using Humanizer.Localisation.CollectionFormatters;

namespace Humanizer.Configuration
{
    internal class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
    {
        public CollectionFormatterRegistry()
            : base(new DefaultCollectionFormatter("&"))
        {
            Register("en", new OxfordStyleCollectionFormatter("and"));
            Register("it", new DefaultCollectionFormatter("e"));
            Register("de", new DefaultCollectionFormatter("und"));
            Register("dk", new DefaultCollectionFormatter("og"));
            Register("nl", new DefaultCollectionFormatter("en"));
            Register("pt", new DefaultCollectionFormatter("e"));
            Register("nn", new DefaultCollectionFormatter("og"));
            Register("nb", new DefaultCollectionFormatter("og"));
        }
    }
}