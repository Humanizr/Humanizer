using System.Collections;
using System.Resources;
using System.Text;
using Xunit.Abstractions;

public class MergeResources(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Foo()
    {
        var builder = new StringBuilder();
        var files = Directory.EnumerateFiles(@"C:\Code\Humanizer\src\Humanizer\Properties","*.resx");
         foreach (var file in files)
        {
            testOutputHelper.WriteLine(file);
            var reader = new ResXResourceReader (file);
            foreach (DictionaryEntry entry in reader)
            {
                testOutputHelper.WriteLine($"    {entry.Key} {entry.Value}");
            }
        }
    }
}