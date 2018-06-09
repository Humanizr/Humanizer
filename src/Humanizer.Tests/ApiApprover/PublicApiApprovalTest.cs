#if NET46
using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using Xunit;

namespace Humanizer.Tests.ApiApprover
{

    public class PublicApiApprovalTest
    {
        [Fact]
        [UseCulture("en-US")]
        [UseReporter(typeof(DiffReporter))] 
        [IgnoreLineEndings(true)]
        public void approve_public_api()
        {

            var publicApi = Filter(ApiGenerator.GeneratePublicApi(typeof(StringHumanizeExtensions).Assembly));
            Approvals.Verify(publicApi);
        }

        private static string Filter(string text)
        {
            return string.Join(Environment.NewLine, text.Split(new[]
                                                        {
                                                            Environment.NewLine
                                                        }, StringSplitOptions.RemoveEmptyEntries)
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyFileVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: AssemblyInformationalVersion("))
                                                        .Where(l => !l.StartsWith("[assembly: System.Reflection.AssemblyMetadataAttribute(\"CommitHash\""))
                                                        .Where(l => !string.IsNullOrWhiteSpace(l))
            );
        }
    }
}
#endif
