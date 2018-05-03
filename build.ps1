param(
    [string]$target = "test"
)

& dotnet $target ./src/Humanizer.Tests/Humanizer.Tests.csproj
