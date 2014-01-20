namespace Humanizer
{
    interface IChangeTense
    {
        bool Applies();
        string Apply();
    }
}