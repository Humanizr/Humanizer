using ApiApprover;
using ApprovalTests;
using ApprovalTests.Reporters;
using Xunit;

namespace Humanizer.Tests.ApiApprover
{
    public class PublicApiApprovalTest
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))] 
        public void approve_public_api()
        {
            var assembly = typeof(StringHumanizeExtensions).Assembly;
            var publicApi = PublicApiGenerator.CreatePublicApiForAssembly(assembly);
            Approvals.Verify(publicApi);
        }
    }
}