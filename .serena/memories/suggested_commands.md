```bash
# Inspect repo status / files (Linux environment)
git status
ls -a
rg "search term"

# Restore + build
cd /home/claire/local-dev/Humanizer/src
DOTNET_CLI_UI_LANGUAGE=en dotnet restore
DOTNET_CLI_UI_LANGUAGE=en dotnet build Humanizer/Humanizer.csproj -c Release /t:PackNuSpecs /p:PackageOutputPath=<out>

# Run tests (Linux skips net48)
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0

# Verify packages after building
pwsh ./verify-packages.ps1 -PackagesPath <out> -MinimumPassingSdkVersion 9.0.200

# DocFX docs (from repo root)
dotnet tool restore
dotnet tool run docfx build docs/docfx.json
# Preview docs
dotnet tool run docfx serve docs/_site

# Benchmarks (example)
dotnet run --project src/Benchmarks/Benchmarks.csproj -c Release
```