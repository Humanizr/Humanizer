namespace Humanizer;

/// <summary>
/// Formats collections using Oxford-comma punctuation for lists with three or more items.
/// </summary>
class OxfordStyleCollectionFormatter() :
    DefaultCollectionFormatter("and")
{
    /// <summary>
    /// Uses an Oxford comma when there are three or more displayable items.
    /// </summary>
    protected override string GetConjunctionFormatString(int itemCount) => itemCount > 2 ? "{0}, {1} {2}" : "{0} {1} {2}";
}
