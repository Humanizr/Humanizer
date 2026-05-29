using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddJob(Job.Default
                    .WithToolchain(InProcessEmitToolchain.Instance))
                .AddExporter(JsonExporter.Full)
                .AddExporter(MarkdownExporter.GitHub)
                .AddDiagnoser(MemoryDiagnoser.Default);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);