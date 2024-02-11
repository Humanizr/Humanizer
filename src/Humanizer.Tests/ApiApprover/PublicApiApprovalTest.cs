﻿using PublicApiGenerator;

namespace Humanizer.Tests.ApiApprover
{
    public class PublicApiApprovalTest
    {
        [Fact]
        [UseCulture("en-US")]
        public Task Approve_Public_Api()
        {
            var publicApi = typeof(StringHumanizeExtensions).Assembly.GeneratePublicApi();

            return Verifier.Verify(publicApi)
                .ScrubLinesContaining("CommitHash", "RepositoryUrl")
                .UniqueForTargetFrameworkAndVersion();
        }
    }
}
