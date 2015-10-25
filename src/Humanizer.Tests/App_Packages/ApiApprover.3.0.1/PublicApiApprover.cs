using System.IO;
using ApprovalTests;
using ApprovalTests.Namers;
using Mono.Cecil;

namespace ApiApprover
{
    public static class PublicApiApprover
    {
        public static void ApprovePublicApi(string assemblyPath)
        {
            var assemblyResolver = new DefaultAssemblyResolver();
            assemblyResolver.AddSearchDirectory(Path.GetDirectoryName(assemblyPath));

            var readSymbols = File.Exists(Path.ChangeExtension(assemblyPath, ".pdb"));
            var asm = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters(ReadingMode.Deferred)
            {
                ReadSymbols = readSymbols,
                AssemblyResolver = assemblyResolver,
            });

            var publicApi = PublicApiGenerator.CreatePublicApiForAssembly(asm);
            var writer = new ApprovalTextWriter(publicApi, "cs");
            var approvalNamer = new AssemblyPathNamer(assemblyPath);
            ApprovalTests.Approvals.Verify(writer, approvalNamer, ApprovalTests.Approvals.GetReporter());
        }

        private class AssemblyPathNamer : UnitTestFrameworkNamer
        {
            private readonly string name;

            public AssemblyPathNamer(string assemblyPath)
            {
                name = Path.GetFileNameWithoutExtension(assemblyPath);
            }

            public override string Name
            {
                get { return name; }
            }
        }
    }
}