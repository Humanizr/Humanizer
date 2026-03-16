using System.Xml.Linq;

public class LocaleResourceParityTests
{
    [Fact]
    public void LocalizedResourceFilesContainAllNeutralKeys()
    {
        var repoRoot = FindRepositoryRoot();
        var propertiesDirectory = Path.Combine(repoRoot, "src", "Humanizer", "Properties");
        var neutralPath = Path.Combine(propertiesDirectory, "Resources.resx");
        var neutralKeys = ReadResourceKeys(neutralPath);

        var failures = Directory.EnumerateFiles(propertiesDirectory, "Resources.*.resx")
            .Select(path => new
            {
                FileName = Path.GetFileName(path),
                MissingKeys = neutralKeys.Except(ReadResourceKeys(path)).OrderBy(key => key).ToArray()
            })
            .Where(result => result.MissingKeys.Length > 0)
            .Select(result => $"{result.FileName}: {string.Join(", ", result.MissingKeys)}")
            .ToArray();

        Assert.True(failures.Length == 0, string.Join(Environment.NewLine, failures));
    }

    static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "src", "Humanizer", "Properties", "Resources.resx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }

    static HashSet<string> ReadResourceKeys(string path) =>
        XDocument.Load(path)
            .Root!
            .Elements("data")
            .Select(element => (string?)element.Attribute("name"))
            .OfType<string>()
            .ToHashSet(StringComparer.Ordinal);
}
