<#
.SYNOPSIS
    Verifies the packed Humanizer NuGet package still ships analyzers correctly.

.DESCRIPTION
    This script validates the single Humanizer package produced by direct project packing.
    It focuses only on analyzer packaging:

    1. Verifies the expected analyzer/build assets exist inside the nupkg
    2. Restores and builds small consumer projects that should trigger HUMANIZER001

.PARAMETER PackageVersion
    The version of the Humanizer package to verify (for example, "3.0.0-rc.14").

.PARAMETER PackagesDirectory
    The directory containing the built Humanizer package.

.EXAMPLE
    .\tests\verify-packages.ps1 -PackageVersion "3.0.0" -PackagesDirectory ".\artifacts\packages"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$PackageVersion,

    [Parameter(Mandatory = $true)]
    [string]$PackagesDirectory
)

$ErrorActionPreference = "Stop"

function ConvertTo-AzureDevOpsCommandValue {
    param([string]$Value)

    if ([string]::IsNullOrEmpty($Value)) {
        return $Value
    }

    return $Value.Replace('%', '%25').Replace("`r", '%0D').Replace("`n", '%0A').Replace(';', '%3B').Replace(']', '%5D')
}

function Write-AzureDevOpsSection {
    param([string]$Message)

    Write-Host "##[section]$Message"
}

function Write-AzureDevOpsError {
    param([string]$Message)

    if (-not [string]::IsNullOrWhiteSpace($Message)) {
        Write-Host "##vso[task.logissue type=error;]$(ConvertTo-AzureDevOpsCommandValue $Message)"
    }
}

function Invoke-CommandText {
    param(
        [Parameter(Mandatory = $true)][string]$FilePath,
        [string[]]$ArgumentList = @(),
        [string]$WorkingDirectory
    )

    $currentLocation = Get-Location
    try {
        if ($WorkingDirectory) {
            Set-Location $WorkingDirectory
        }

        $output = & $FilePath @ArgumentList 2>&1 | ForEach-Object { $_.ToString() }
        $exitCode = $LASTEXITCODE
        $global:LASTEXITCODE = 0
        return [PSCustomObject]@{
            ExitCode = $exitCode
            Output   = ($output -join "`n")
        }
    } finally {
        if ($WorkingDirectory) {
            Set-Location $currentLocation
        }
    }
}

function Expand-TemplateDirectory {
    param(
        [Parameter(Mandatory = $true)][string]$TemplateDirectory,
        [Parameter(Mandatory = $true)][string]$DestinationDirectory,
        [Parameter(Mandatory = $true)][hashtable]$Tokens
    )

    New-Item -ItemType Directory -Path $DestinationDirectory -Force | Out-Null

    foreach ($item in Get-ChildItem -Path $TemplateDirectory -Recurse -Force) {
        $relativePath = $item.FullName.Substring($TemplateDirectory.Length).TrimStart('\', '/')
        if ([string]::IsNullOrWhiteSpace($relativePath)) {
            continue
        }

        if ($item.PSIsContainer) {
            New-Item -ItemType Directory -Path (Join-Path $DestinationDirectory $relativePath) -Force | Out-Null
            continue
        }

        $targetRelativePath = if ($relativePath.EndsWith('.template')) {
            $relativePath.Substring(0, $relativePath.Length - '.template'.Length)
        } else {
            $relativePath
        }

        $destinationPath = Join-Path $DestinationDirectory $targetRelativePath
        $destinationParent = Split-Path -Parent $destinationPath
        if (-not [string]::IsNullOrWhiteSpace($destinationParent)) {
            New-Item -ItemType Directory -Path $destinationParent -Force | Out-Null
        }

        if (-not $relativePath.EndsWith('.template')) {
            Copy-Item -Path $item.FullName -Destination $destinationPath -Force
            continue
        }

        $content = Get-Content -Raw $item.FullName
        foreach ($token in $Tokens.GetEnumerator()) {
            $content = $content.Replace($token.Key, [string]$token.Value)
        }

        Set-Content -Path $destinationPath -Value $content -Encoding UTF8
    }
}

function Get-PackageReferencesXml {
    param([Parameter(Mandatory = $true)][string]$PackageVersion)

    return @"
  <ItemGroup>
    <PackageReference Include="Humanizer" Version="$PackageVersion" />
  </ItemGroup>
"@
}

function Test-AnalyzerPackageEntries {
    param([Parameter(Mandatory = $true)][string]$PackagePath)

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    $zip = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        $entries = @($zip.Entries | Select-Object -ExpandProperty FullName)
    } finally {
        $zip.Dispose()
    }

    $expectedEntries = @(
        "analyzers/dotnet/roslyn3.8/cs/Humanizer.Analyzers.dll",
        "analyzers/dotnet/roslyn4.8/cs/Humanizer.Analyzers.dll",
        "analyzers/dotnet/roslyn4.14/cs/Humanizer.Analyzers.dll",
        "buildTransitive/Humanizer.targets"
    )

    $missingEntries = @($expectedEntries | Where-Object { $entries -notcontains $_ })
    if ($missingEntries.Count -gt 0) {
        return [PSCustomObject]@{
            Success = $false
            Details = "Missing analyzer package entries: $($missingEntries -join ', ')"
        }
    }

    return [PSCustomObject]@{
        Success = $true
        Details = $null
    }
}

function Invoke-AnalyzerSmokeTest {
    param(
        [Parameter(Mandatory = $true)][string]$DisplayName,
        [Parameter(Mandatory = $true)][string]$TargetFramework,
        [Parameter(Mandatory = $true)][string]$PackageVersion,
        [Parameter(Mandatory = $true)][string]$NuGetConfig,
        [Parameter(Mandatory = $true)][string]$TempDirectory
    )

    $fixtureDirectory = Join-Path $PSScriptRoot "fixtures\PackageSmoke\ConsoleConsumer"
    $projectDirectory = Join-Path $TempDirectory ($DisplayName -replace '[^A-Za-z0-9]', '')

    Expand-TemplateDirectory -TemplateDirectory $fixtureDirectory -DestinationDirectory $projectDirectory -Tokens @{
        "__TARGET_FRAMEWORK__" = $TargetFramework
        "__PACKAGE_REFERENCES__" = (Get-PackageReferencesXml -PackageVersion $PackageVersion)
    }

    Copy-Item -Path (Join-Path $PSScriptRoot "fixtures\PackageSmoke\AnalyzerProbe.cs") -Destination (Join-Path $projectDirectory "AnalyzerProbe.cs") -Force

    $projectPath = Join-Path $projectDirectory "Consumer.csproj"
    $restoreResult = Invoke-CommandText -FilePath "dotnet" -ArgumentList @("restore", $projectPath, "--configfile", $NuGetConfig, "-nologo") -WorkingDirectory $projectDirectory
    if ($restoreResult.ExitCode -ne 0) {
        return [PSCustomObject]@{
            DisplayName = $DisplayName
            Success = $false
            Details = "Restore failed.`n$($restoreResult.Output)"
        }
    }

    $buildResult = Invoke-CommandText -FilePath "dotnet" -ArgumentList @("build", $projectPath, "--no-restore", "-nologo") -WorkingDirectory $projectDirectory
    if ($buildResult.ExitCode -eq 0 -or ($buildResult.Output -notmatch 'HUMANIZER001')) {
        return [PSCustomObject]@{
            DisplayName = $DisplayName
            Success = $false
            Details = "Expected HUMANIZER001 analyzer diagnostic for $DisplayName.`n$($buildResult.Output)"
        }
    }

    return [PSCustomObject]@{
        DisplayName = $DisplayName
        Success = $true
        Details = $null
    }
}

Write-AzureDevOpsSection "Humanizer Analyzer Package Verification"
Write-Host "Package Version: $PackageVersion"
Write-Host "Packages Directory: $PackagesDirectory"
Write-Host ""

if (-not (Test-Path $PackagesDirectory)) {
    Write-AzureDevOpsError "Packages directory not found: $PackagesDirectory"
    throw "Packages directory not found: $PackagesDirectory"
}

$mainPackage = Get-ChildItem -Path $PackagesDirectory -Filter "Humanizer.$PackageVersion.nupkg" -File
if (-not $mainPackage) {
    Write-AzureDevOpsError "Humanizer package not found: Humanizer.$PackageVersion.nupkg"
    throw "Humanizer package not found"
}

Write-Host "##[command]Found package: $($mainPackage.Name)"
Write-Host ""

$tempPath = if ($env:TEMP) { $env:TEMP } elseif ($env:TMP) { $env:TMP } else { "/tmp" }
$tempDir = Join-Path $tempPath "HumanizerAnalyzerPackageTest_$(New-Guid)"
New-Item -ItemType Directory -Path $tempDir -Force | Out-Null

try {
    Write-AzureDevOpsSection "Inspecting analyzer package contents"
    $packageContentResult = Test-AnalyzerPackageEntries -PackagePath $mainPackage.FullName
    if (-not $packageContentResult.Success) {
        Write-AzureDevOpsError $packageContentResult.Details
        throw $packageContentResult.Details
    }

    Write-Host "##[command]Analyzer package entries found"
    Write-Host ""

    $nugetConfigPath = Join-Path $tempDir "NuGet.config"
    $absolutePackagesDir = (Resolve-Path $PackagesDirectory).Path
    @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="LocalPackages" value="$absolutePackagesDir" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
"@ | Set-Content -Path $nugetConfigPath -Encoding UTF8

    Write-AzureDevOpsSection "Running analyzer smoke tests"
    $smokeTests = @(
        @{ DisplayName = "Console analyzer net8.0"; TargetFramework = "net8.0" }
    )

    $runningOnWindows = $false
    try {
        $runningOnWindows = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)
    } catch {
        $runningOnWindows = $false
    }

    if ($runningOnWindows) {
        $smokeTests += @{ DisplayName = "Console analyzer net48"; TargetFramework = "net48" }
    }

    $smokeResults = foreach ($smokeTest in $smokeTests) {
        Invoke-AnalyzerSmokeTest -DisplayName $smokeTest.DisplayName -TargetFramework $smokeTest.TargetFramework -PackageVersion $PackageVersion -NuGetConfig $nugetConfigPath -TempDirectory $tempDir
    }

    $failedSmokeTest = $smokeResults | Where-Object { -not $_.Success } | Select-Object -First 1
    if ($null -ne $failedSmokeTest) {
        Write-AzureDevOpsError $failedSmokeTest.Details
        throw $failedSmokeTest.Details
    }

    Write-AzureDevOpsSection "Verification Summary"
    Write-Host "Summary:"
    Write-Host "  ✓ Analyzer package entries present"
    foreach ($smokeResult in $smokeResults) {
        Write-Host "  ✓ $($smokeResult.DisplayName)"
    }

    # The analyzer smoke tests intentionally invoke a failing build to prove the analyzer fires.
    # Clear the inherited native exit code so the script itself returns success when verification passes.
    $global:LASTEXITCODE = 0
} finally {
    if (Test-Path $tempDir) {
        Remove-Item -LiteralPath $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
