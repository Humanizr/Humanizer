```

BenchmarkDotNet v0.15.4, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.100-rc.2.25502.107
  [Host]   : .NET 10.0.0 (10.0.0-rc.2.25502.107, 10.0.25.50307), X64 RyuJIT x86-64-v3
  ShortRun : .NET 10.0.0 (10.0.0-rc.2.25502.107, 10.0.25.50307), X64 RyuJIT x86-64-v3

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                | Mean     | Error     | StdDev    | Gen0   | Allocated |
|---------------------- |---------:|----------:|----------:|-------:|----------:|
| &#39;Humanize PascalCase&#39; | 1.609 μs | 0.0492 μs | 0.0027 μs | 0.1583 |    2.6 KB |
