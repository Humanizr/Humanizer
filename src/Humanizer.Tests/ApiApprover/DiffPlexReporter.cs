#if NET6_0_OR_GREATER
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.IO;
using ApprovalTests.Core;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Xunit.Abstractions;

namespace Humanizer.Tests.ApiApprover
{
    public class DiffPlexReporter : IApprovalFailureReporter
    {
        public static readonly DiffPlexReporter Instance = new DiffPlexReporter();

        public ITestOutputHelper Output { get; set; }

        public void Report(string approved, string received)
        {
            var approvedText = File.Exists(approved) ? File.ReadAllText(approved) : string.Empty;
            var receivedText = File.ReadAllText(received);

            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(approvedText, receivedText);

            foreach (var line in diff.Lines)
            {
                if (line.Type == ChangeType.Unchanged)
                {
                    continue;
                }

                var prefix = "  ";
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        prefix = "+ ";
                        break;
                    case ChangeType.Deleted:
                        prefix = "- ";
                        break;
                }

                Output.WriteLine("{0}{1}", prefix, line.Text);
            }
        }
    }
}
#endif
