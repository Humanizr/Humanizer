namespace Humanizer
{
    /// <summary>
    /// MetricPrefix contains all supported metric prefixes and the corresponding powers of ten as underlying values.
    /// Unsupported: hecto, deca, deci and centi.
    /// </summary>
    public enum MetricPrefix
    {
        Yocto = -24,
        Zepto = -21,
        Atto = -18,
        Femto = -15,
        Pico = -12,
        Nano = -9,
        Micro = -6,
        Milli = -3,
        Kilo = 3,
        Mega = 6,
        Giga = 9,
        Tera = 12,
        Peta = 15,
        Exa = 18,
        Zetta = 21,
        Yotta = 24
    }
}
