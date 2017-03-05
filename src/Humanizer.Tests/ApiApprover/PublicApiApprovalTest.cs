#if NET46
using System.IO;
using ApiApprover;
using ApprovalTests;
using ApprovalTests.Reporters;
using Mono.Cecil;
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
            var assemblyPath = typeof(StringHumanizeExtensions).Assembly.Location;

            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            var readSymbols = File.Exists(Path.ChangeExtension(assemblyPath, ".pdb"));
            var asm = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters(ReadingMode.Deferred)
            {
                ReadSymbols = readSymbols,
                AssemblyResolver = assemblyResolver
            });

            var publicApi = PublicApiGenerator.CreatePublicApiForAssembly(asm);
            Approvals.Verify(publicApi);
        }
    }
}
#endif