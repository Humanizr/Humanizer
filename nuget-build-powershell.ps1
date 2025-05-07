# Restore .NET projects
Write-Host "Restoring .NET projects..."
dotnet restore .\src\Humanizer.sln 

# Build the solution
Write-Host "Building the solution..."
dotnet build .\src\Humanizer.sln /p:Configuration=Release

# Install NuGet tool
Write-Host "Installing NuGet tool..."
Install-PackageProvider -Name NuGet -MinimumVersion 2.8.5.201 -Force -Scope CurrentUser

# Create packages
Write-Host "Creating packages..."


if (Test-Path -Path ".\Packages") {
    Remove-Item -Path ".\Packages" -Recurse -Force
}
New-Item -ItemType Directory -Path ".\Packages"

$nuspecs = Get-ChildItem .\NuSpecs\*.nuspec 

foreach ($item in $nuspecs) {
    nuget pack $item.FullName `
        -OutputDirectory ".\Packages" `
        -BasePath ".\src" `
        -NoPackageAnalysis `
        -Properties "version=3.0.1;RepositoryType=git;RepositoryUrl=https://github.com/Humanizr/Humanizer"
}

# Prevent the window from closing immediately
Read-Host -Prompt "Press Enter to exit"