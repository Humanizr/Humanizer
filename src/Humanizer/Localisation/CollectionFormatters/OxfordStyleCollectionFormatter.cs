namespace Humanizer.Localisation.CollectionFormatters
{
    internal class OxfordStyleCollectionFormatter : DefaultCollectionFormatter
    {
        public OxfordStyleCollectionFormatter(string defaultSeparator)
            : base(defaultSeparator ?? "and")
        {
        }

        protected override string GetConjunctionFormatString(int itemCount) => itemCount > 2 ? "{0}, {1} {2}" : "{0} {1} {2}";
    }
}