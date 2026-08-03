namespace Humanizer;

readonly struct CultureResolutionRecord(
    string acceptedName,
    string localeProfileOwner,
    string? inflectionOwner,
    InflectionStatus? inflectionTerminal)
{
    public string AcceptedName { get; } = acceptedName;
    public string LocaleProfileOwner { get; } = localeProfileOwner;
    public string? InflectionOwner { get; } = inflectionOwner;
    public InflectionStatus? InflectionTerminal { get; } = inflectionTerminal;
}