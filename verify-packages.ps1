<#
.SYNOPSIS
    Verifies Humanizer NuGet packages structure, dependencies, and SDK compatibility.

.DESCRIPTION
    This script validates that Humanizer NuGet packages are correctly built and can be restored
    across multiple .NET SDK versions. It performs the following checks:
    
    1. Verifies all expected packages exist (main metapackage, core package, and satellite packages)
    2. Tests package restoration on multiple .NET SDK versions (8, 9, and 10)
       - Creates isolated test environments with global.json for each SDK version
       - Validates that packages can be restored successfully
    3. Verifies that the main Humanizer metapackage includes all satellite packages as dependencies
    
    The script is designed to run in CI/CD pipelines (Azure DevOps) and provides detailed
    logging with Azure DevOps-specific formatting.

.PARAMETER PackageVersion
    The version of the Humanizer packages to verify (e.g., "3.0.0-rc.14").

.PARAMETER PackagesDirectory
    The directory containing the built NuGet packages (.nupkg files).

.EXAMPLE
    .\verify-packages.ps1 -PackageVersion "3.0.0" -PackagesDirectory ".\artifacts\packages"

.NOTES
    - The script requires .NET SDK 8, 9, and/or 10 to be installed
    - SDKs that are not installed will be skipped with a warning
    - MSBuild on .NET Framework testing could be added for Windows environments
#>

param(
    [Parameter(Mandatory=$true)]
    [string]$PackageVersion,
    
    [Parameter(Mandatory=$true)]
    [string]$PackagesDirectory
)

$ErrorActionPreference = "Stop"

function Write-AzureDevOpsSection {
    param([string]$Message)
    Write-Host "##[section]$Message"
}

function Write-AzureDevOpsError {
    param([string]$Message)
    Write-Host "##[error]$Message"
}

function Write-AzureDevOpsWarning {
    param([string]$Message)
    Write-Host "##[warning]$Message"
}

Write-AzureDevOpsSection "Humanizer Package Verification"
Write-Host "Package Version: $PackageVersion"
Write-Host "Packages Directory: $PackagesDirectory"
Write-Host ""

# Verify the packages directory exists
if (-not (Test-Path $PackagesDirectory)) {
    Write-AzureDevOpsError "Packages directory not found: $PackagesDirectory"
    throw "Packages directory not found: $PackagesDirectory"
}

# Find all packages
$mainPackage = Get-ChildItem -Path $PackagesDirectory -Filter "Humanizer.$PackageVersion.nupkg" -File
$corePackage = Get-ChildItem -Path $PackagesDirectory -Filter "Humanizer.Core.$PackageVersion.nupkg" -File
$satellitePackages = Get-ChildItem -Path $PackagesDirectory -Filter "Humanizer.Core.*.$PackageVersion.nupkg" -File | 
    Where-Object { $_.Name -ne "Humanizer.Core.$PackageVersion.nupkg" }

if (-not $mainPackage) {
    Write-AzureDevOpsError "Main Humanizer metapackage not found: Humanizer.$PackageVersion.nupkg"
    throw "Main Humanizer metapackage not found"
}
Write-Host "##[command]Found main metapackage: $($mainPackage.Name)"

if (-not $corePackage) {
    Write-AzureDevOpsError "Humanizer.Core package not found: Humanizer.Core.$PackageVersion.nupkg"
    throw "Humanizer.Core package not found"
}
Write-Host "##[command]Found core package: $($corePackage.Name)"

Write-Host "##[command]Found $($satellitePackages.Count) satellite (localized) packages"
$satellitePackages | ForEach-Object { Write-Host "  - $($_.Name)" }
Write-Host ""

# Create a temporary test directory
$tempPath = if ($env:TEMP) { $env:TEMP } elseif ($env:TMP) { $env:TMP } else { "/tmp" }
$tempDir = Join-Path $tempPath "HumanizerPackageTest_$(New-Guid)"
New-Item -ItemType Directory -Path $tempDir -Force | Out-Null

try {
    # Create NuGet.config pointing to local packages
    $nugetConfig = Join-Path $tempDir "NuGet.config"
    $absolutePackagesDir = (Resolve-Path $PackagesDirectory).Path
    $nugetConfigContent = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="LocalPackages" value="$absolutePackagesDir" />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  </packageSources>
</configuration>
"@
    Set-Content -Path $nugetConfig -Value $nugetConfigContent

    # Get list of installed SDKs
    Write-Host "Detecting installed .NET SDKs..."
    $installedSdks = dotnet --list-sdks 2>&1 | Out-String
    Write-Host $installedSdks
    
    # Define SDK versions to test
    # These versions will use rollForward to match any installed SDK in that major version
    # Note: MSBuild on .NET Framework could be added here for Windows environments to test net48 compatibility
    $sdkVersionsToTest = @(
        @{ Version = "8.0.100"; RollForward = "latestFeature"; Name = "SDK 8"; MajorVersion = 8 },
        @{ Version = "9.0.100"; RollForward = "latestFeature"; Name = "SDK 9"; MajorVersion = 9 },
        @{ Version = "10.0.100-rc.2"; RollForward = "latestFeature"; Name = "SDK 10"; MajorVersion = 10 }
    )

    $sdkTestResults = @()
    
    # Filter to only test SDKs that are actually installed
    $sdksToTest = $sdkVersionsToTest | Where-Object {
        $majorVersion = $_.MajorVersion
        $pattern = "(?m)^$majorVersion\."
        if ($installedSdks -match $pattern) {
            $true
        } else {
            Write-Host "##[warning]$($_.Name) not installed, skipping"
            $false
        }
    }
    
    if ($sdksToTest.Count -eq 0) {
        $sdkMajorVersions = ($sdkVersionsToTest | ForEach-Object { $_.MajorVersion }) -join ", "
        Write-AzureDevOpsWarning "No target SDK versions ($sdkMajorVersions) are installed"
    }

    # Test package restoration with each SDK version
    foreach ($sdkConfig in $sdksToTest) {
        Write-AzureDevOpsSection "Testing package restoration with $($sdkConfig.Name) (v$($sdkConfig.Version))"
        
        # Create a test folder for this SDK version
        $sdkTestDir = Join-Path $tempDir "$($sdkConfig.Name -replace ' ', '')"
        New-Item -ItemType Directory -Path $sdkTestDir -Force | Out-Null
        
        $currentLocation = Get-Location
        try {
            Set-Location $sdkTestDir
            
            # Create global.json for this SDK version
            $globalJsonContent = @"
{
  "sdk": {
    "version": "$($sdkConfig.Version)",
    "rollForward": "$($sdkConfig.RollForward)"
  }
}
"@
            Set-Content -Path "global.json" -Value $globalJsonContent
            Write-Host "Created global.json with SDK version $($sdkConfig.Version) and rollForward: $($sdkConfig.RollForward)"
            
            # Create test project
            Write-Host "Creating test project..."
            $output = dotnet new console -n MetaTest --force 2>&1
            if ($LASTEXITCODE -ne 0) {
                Write-AzureDevOpsWarning "Failed to create test project with $($sdkConfig.Name): $output"
                $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $false; Error = "Failed to create test project" }
                continue
            }
            
            Set-Location MetaTest
            
            # Add Humanizer package reference
            Write-Host "Adding Humanizer package reference..."
            $output = dotnet add package Humanizer --version $PackageVersion --no-restore 2>&1
            if ($LASTEXITCODE -ne 0) {
                Write-AzureDevOpsWarning "Failed to add Humanizer package reference with $($sdkConfig.Name): $output"
                $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $false; Error = "Failed to add package reference" }
                continue
            }
            
            # Restore packages
            Write-Host "Restoring packages..."
            $restoreOutput = dotnet restore --configfile $nugetConfig 2>&1
            if ($LASTEXITCODE -ne 0) {
                Write-AzureDevOpsWarning "Failed to restore Humanizer metapackage with $($sdkConfig.Name)"
                Write-Host $restoreOutput
                $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $false; Error = "Failed to restore packages" }
                continue
            }
            
            # Verify restore succeeded
            $objPath = "obj/project.assets.json"
            if (-not (Test-Path $objPath)) {
                Write-AzureDevOpsWarning "project.assets.json not found after restore with $($sdkConfig.Name)"
                $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $false; Error = "project.assets.json not found" }
                continue
            }
            
            Write-Host "##[command]✓ Package restoration succeeded with $($sdkConfig.Name)"
            $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $true }
            
        } catch {
            Write-AzureDevOpsWarning "Exception testing $($sdkConfig.Name): $($_.Exception.Message)"
            $sdkTestResults += @{ SDK = $sdkConfig.Name; Success = $false; Error = $_.Exception.Message }
        } finally {
            Set-Location $currentLocation
        }
    }

    # Report SDK test results
    Write-Host ""
    Write-AzureDevOpsSection "SDK Version Test Results"
    $allSdksPassed = $true
    foreach ($result in $sdkTestResults) {
        if ($result.Success) {
            Write-Host "  ✓ $($result.SDK): Passed"
        } else {
            Write-Host "  ✗ $($result.SDK): Failed - $($result.Error)"
            $allSdksPassed = $false
        }
    }
    
    if ($sdkTestResults.Count -eq 0) {
        Write-AzureDevOpsWarning "No SDK versions were tested"
    } elseif (-not $allSdksPassed) {
        Write-AzureDevOpsError "Not all SDK versions passed package restoration tests"
        throw "SDK version testing failed"
    }

    # Verify main metapackage can be restored and pulls in satellites
    Write-Host ""
    Write-AzureDevOpsSection "Verifying Humanizer metapackage dependencies"
    Push-Location $tempDir
    
    Write-Host "Creating test project..."
    $output = dotnet new console -n MetaTest --force 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-AzureDevOpsError "Failed to create test project"
        throw "Failed to create test project: $output"
    }
    
    Set-Location MetaTest
    
    Write-Host "Adding Humanizer package reference..."
    $output = dotnet add package Humanizer --version $PackageVersion --no-restore 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-AzureDevOpsError "Failed to add Humanizer package reference"
        throw "Failed to add package reference: $output"
    }
    
    Write-Host "Restoring packages..."
    $restoreOutput = dotnet restore --configfile $nugetConfig 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-AzureDevOpsError "Failed to restore Humanizer metapackage"
        Write-Host $restoreOutput
        throw "Failed to restore Humanizer metapackage"
    }
    
    # Check what packages were restored
    $objPath = "obj/project.assets.json"
    if (-not (Test-Path $objPath)) {
        Write-AzureDevOpsError "project.assets.json not found after restore"
        throw "project.assets.json not found after restore"
    }
    
    $assets = Get-Content $objPath | ConvertFrom-Json
    $restoredPackages = $assets.libraries.PSObject.Properties.Name | Where-Object { $_ -like "Humanizer*" }
    
    Write-Host "Restored packages:"
    $restoredPackages | ForEach-Object { Write-Host "  - $_" }
    Write-Host ""
    
    # Verify all satellite packages are included as dependencies
    $missingSatellites = @()
    foreach ($satellite in $satellitePackages) {
        $pkgName = $satellite.Name -replace "\.$PackageVersion\.nupkg$", ""
        $pkgEntry = "$pkgName/$PackageVersion"
        if ($restoredPackages -notcontains $pkgEntry) {
            $missingSatellites += $pkgName
        }
    }
    
    if ($missingSatellites.Count -gt 0) {
        Write-AzureDevOpsError "The following satellite packages are NOT dependencies of the Humanizer metapackage: $($missingSatellites -join ', ')"
        throw "Missing satellite package dependencies in Humanizer metapackage"
    }
    
    Write-Host "##[command]✓ All $($satellitePackages.Count) satellite packages are dependencies of the Humanizer metapackage"
    
    Pop-Location
    Write-Host ""
    
    Write-AzureDevOpsSection "✓ All Verification Checks Passed"
    Write-Host "##[command]Summary:"
    Write-Host "  - Main metapackage (Humanizer): ✓"
    Write-Host "  - Core package (Humanizer.Core): ✓"
    Write-Host "  - Satellite packages verified: $($satellitePackages.Count)"
    Write-Host "  - All satellites are dependencies of metapackage: ✓"
    Write-Host "  - SDK version tests passed: $($sdkTestResults | Where-Object Success | Measure-Object | Select-Object -ExpandProperty Count)/$($sdkTestResults.Count)"
    
} catch {
    Write-AzureDevOpsError $_.Exception.Message
    throw
} finally {
    # Cleanup
    if (Test-Path $tempDir) {
        Write-Host ""
        Write-Host "Cleaning up temporary directory..."
        Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
