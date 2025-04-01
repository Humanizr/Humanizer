using System.Globalization;

[MemoryDiagnoser(false)]
public class PolishTimeSpanBenchmarks
{
    static CultureInfo culture = new("pl");

    [Benchmark(Description = "PolishTimeSpan.Milliseconds")]
    public void Milliseconds()
    {
        foreach (var number in milliseconds)
        {
            TimeSpan.FromMilliseconds(number).Humanize(culture: culture);
        }
    }

    static int[] milliseconds = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Seconds")]
    public void Seconds()
    {
        foreach (var number in seconds)
        {
            TimeSpan.FromSeconds(number).Humanize(culture: culture); 
        }
    }

    static int[] seconds = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Minutes")]
    public void Minutes()
    {
        foreach (var number in minutes)
        {
            TimeSpan.FromMinutes(number).Humanize(culture: culture);
        }
    }

    static int[] minutes = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Hours")]
    public void Hours()
    {
        foreach (var number in hours)
        {
            TimeSpan.FromHours(number).Humanize(culture: culture);
        }
    }

    static int[] hours = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Days")]
    public void Days()
    {
        foreach (var number in days)
        {
            TimeSpan.FromDays(number).Humanize(culture: culture);
        }
    }

    static int[] days = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Weeks")]
    public void Weeks()
    {
        foreach (var number in weeks)
        {
            TimeSpan.FromDays(number * 7).Humanize(culture: culture);
        }
    }

    static int[] weeks = [1, 2, 3, 4, 5, 6, 10];

    [Benchmark(Description = "PolishTimeSpan.Months")]
    public void Months()
    {
        foreach (var number in months)
        {
            TimeSpan.FromDays(number).Humanize(culture: culture);
        }
    }

    static int[] months = [31, 61, 92, 122, 153, 183, 335];

    [Benchmark(Description = "PolishTimeSpan.Years")]
    public void Years()
    {
        foreach (var number in years)
        {
            TimeSpan.FromDays(number).Humanize(culture: culture);
        }
    }

    static int[] years = [366, 731, 1096, 1461, 1826, 2191, 3651];
}