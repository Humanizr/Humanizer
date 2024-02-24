namespace Humanizer;

class OxfordStyleCollectionFormatter() :
    DefaultCollectionFormatter("and")
{
    protected override string GetConjunctionFormatString(int itemCount) => itemCount > 2 ? "{0}, {1} {2}" : "{0} {1} {2}";
}