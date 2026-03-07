using Humanizer;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (args.Contains("--humanizer-smoke-exit", StringComparer.Ordinal))
{
    Console.WriteLine(2.ToWords(new CultureInfo("fr")));
    return;
}

app.MapGet("/", () => "ok");
app.Run();
