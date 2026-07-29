namespace Humanizer;

/// <summary>
/// Decimal operands defined by Unicode CLDR cardinal plural rules.
/// </summary>
readonly struct CardinalPluralOperands
{
    CardinalPluralOperands(decimal n, decimal i, int v, int w, decimal f, decimal t)
    {
        N = n;
        I = i;
        V = v;
        W = w;
        F = f;
        T = t;
    }

    public decimal N { get; }
    public decimal I { get; }
    public int V { get; }
    public int W { get; }
    public decimal F { get; }
    public decimal T { get; }

    public static CardinalPluralOperands Create(decimal value)
    {
        var n = Math.Abs(value);
        var i = decimal.Truncate(n);
        var v = (decimal.GetBits(value)[3] >> 16) & 0xFF;
        var f = (n - i) * PowerOfTen(v);
        var t = f;
        var w = v;
        while (w > 0 && t % 10 == 0)
        {
            t /= 10;
            w--;
        }

        return new CardinalPluralOperands(n, i, v, w, f, t);
    }

    static decimal PowerOfTen(int exponent)
    {
        var result = 1m;
        for (var index = 0; index < exponent; index++)
        {
            result *= 10;
        }

        return result;
    }
}