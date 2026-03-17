namespace Humanizer;

class IndonesianNumberToWordsConverter : MalayNumberToWordsConverterBase
{
    protected override string ZeroWord => "nol";
    protected override string MinusWord => "minus";
    protected override string ThousandOneWord => "seribu";
    protected override string[] Units { get; } = ["nol", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas", "dua belas", "tiga belas", "empat belas", "lima belas", "enam belas", "tujuh belas", "delapan belas", "sembilan belas"];
    protected override string[] Tens { get; } = ["", "", "dua puluh", "tiga puluh", "empat puluh", "lima puluh", "enam puluh", "tujuh puluh", "delapan puluh", "sembilan puluh"];
    protected override Scale[] Scales { get; } =
    {
        new(1_000_000_000_000_000_000, "kuintiliun"),
        new(1_000_000_000_000_000, "kuadriliun"),
        new(1_000_000_000_000, "triliun"),
        new(1_000_000_000, "miliar"),
        new(1_000_000, "juta"),
        new(1_000, "ribu")
    };
}
