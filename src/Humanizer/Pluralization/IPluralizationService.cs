namespace Humanizer.Pluralization
{
    public interface IPluralizationService
    {
        string Pluralize(string word);

        string Singularize(string word);
    }
}
