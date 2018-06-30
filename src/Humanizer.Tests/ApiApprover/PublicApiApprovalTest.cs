#if NET46
using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using PublicApiGenerator;
using Xunit;
using Xunit.Abstractions;

namespace Humanizer.Tests.ApiApprover
{

    public class PublicApiApprovalTest
    {
        public PublicApiApprovalTest(ITestOutputHelper output)
        {
            DiffPlexReporter.Instance.Output = output;
        }

        [Fact]
        [UseCulture("en-US")]
#if DEBUG
        [UseReporter(typeof(DiffReporter))]
#else
        [UseReporter(typeof(DiffPlexReporter))]
#endif
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
