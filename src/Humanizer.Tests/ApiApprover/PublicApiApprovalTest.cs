#if !NET35
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
            // arrange
            var assembly = typeof(StringHumanizeExtensions).Assembly;

            // act
            var publicApi = PublicApiGenerator.CreatePublicApiForAssembly(assembly);

            // assert
            Approvals.Verify(publicApi);
        }
    }
}
#endif