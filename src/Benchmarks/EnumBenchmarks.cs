[MemoryDiagnoser(false)]
public class EnumBenchmarks
{
    public enum EnumUnderTest
    {
        [Description("The description")]
        MemberWithDescriptionAttribute,
        MemberWithoutDescriptionAttribute,

        [Display(Description = "Display value")]
        MemberWithDisplayAttribute,
    }

    [Benchmark]
    public string Humanize() => EnumUnderTest.MemberWithDisplayAttribute.Humanize();
}