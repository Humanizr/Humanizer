using PublicApiGenerator;

public class PublicApiApprovalTest
{
    [Fact]
    public Task Approve_Public_Api()
    {
        var publicApi = typeof(StringHumanizeExtensions).Assembly.GeneratePublicApi();

        return Verify(publicApi)
            .ScrubLinesContaining("CommitHash", "RepositoryUrl", "InternalsVisibleTo", "CloudBuildNumber")
            .UniqueForTargetFrameworkAndVersion();
    }
}