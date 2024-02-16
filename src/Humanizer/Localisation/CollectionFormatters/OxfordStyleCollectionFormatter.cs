namespace Humanizer
{
    class OxfordStyleCollectionFormatter(string defaultSeparator) :
        DefaultCollectionFormatter(defaultSeparator ?? "and")
    {
        protected override string GetConjunctionFormatString(int itemCount) => itemCount > 2 ? "{0}, {1} {2}" : "{0} {1} {2}";
    }
}
