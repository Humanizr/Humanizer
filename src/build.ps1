param(
    [string]$target = "test"
)

& dotnet $target ./Humanizer.Tests/Humanizer.Tests.csproj
