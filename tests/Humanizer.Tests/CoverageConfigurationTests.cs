using System.IO;

public class CoverageConfigurationTests
{
    [Fact]
    public void AzurePipelineCoverageReportReadsCoberturaFromRepositoryTestResults()
    {
        var pipeline = File.ReadAllText(GetRepositoryFilePath("azure-pipelines.yml"));

        Assert.Contains("reports: '$(Build.SourcesDirectory)/**/TestResults/**/*.cobertura.xml'", pipeline);
        Assert.DoesNotContain("reports: '$(Agent.TempDirectory)/**/*.cobertura.xml'", pipeline);
    }

    static string GetRepositoryFilePath(string relativePath)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var candidatePath = Path.Combine(directory.FullName, relativePath);
            if (File.Exists(candidatePath))
            {
                return candidatePath;
            }

            directory = directory.Parent;
        }

        throw new FileNotFoundException($"Could not locate repository file '{relativePath}' starting from '{AppContext.BaseDirectory}'.");
    }
}
