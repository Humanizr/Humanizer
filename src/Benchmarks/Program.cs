using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;

var config = ManualConfig.Create(DefaultConfig.Instance)
                .AddJob(Job.Default
                    .WithRuntime(CoreRuntime.Core10_0))
                .AddJob(Job.Default
                    .AsBaseline()
                    .WithRuntime(CoreRuntime.Core80))
                .AddExporter(JsonExporter.Full)
                .AddExporter(MarkdownExporter.GitHub)
                .AddDiagnoser(MemoryDiagnoser.Default);

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);