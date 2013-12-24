using ApiApprover;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace Humanizer.Tests.ApiApprover
{
    public class PublicApiApprovalTest
    {
        // This is set to run in Debug Only because it was failing on the CI server
        // ToDo: Would be nice to run this in CI as well
        [RunnableInDebugOnly]
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