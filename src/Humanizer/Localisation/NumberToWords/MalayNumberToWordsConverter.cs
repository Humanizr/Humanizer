namespace Humanizer;

class MalayNumberToWordsConverter : MalayNumberToWordsConverterBase
{
    protected override string ZeroWord => "kosong";
    protected override string MinusWord => "minus";
    protected override string ThousandOneWord => "seribu";
    protected override string[] Units { get; } = ["kosong", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "lapan", "sembilan", "sepuluh", "sebelas", "dua belas", "tiga belas", "empat belas", "lima belas", "enam belas", "tujuh belas", "lapan belas", "sembilan belas"];
    protected override string[] Tens { get; } = ["", "", "dua puluh", "tiga puluh", "empat puluh", "lima puluh", "enam puluh", "tujuh puluh", "lapan puluh", "sembilan puluh"];
    protected override Scale[] Scales { get; } =
    {
        new(1_000_000_000_000_000_000, "kuintilion"),
        new(1_000_000_000_000_000, "kuadrilion"),
        new(1_000_000_000_000, "trilion"),
        new(1_000_000_000, "bilion"),
        new(1_000_000, "juta"),
        new(1_000, "ribu")
    };
}
