using Consumer;
using Humanizer;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents();

var app = builder.Build();

if (args.Contains("--humanizer-smoke-exit", StringComparer.Ordinal))
{
    Console.WriteLine(2.ToWords(new CultureInfo("fr")));
    return;
}

app.MapRazorComponents<App>();
app.Run();
