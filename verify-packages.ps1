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
$tempDir = Join-Path $env:TEMP "HumanizerPackageTest_$(New-Guid)"
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

    # Test 1: Verify main metapackage can be restored and pulls in satellites
    Write-AzureDevOpsSection "Test 1: Verifying Humanizer metapackage dependencies"
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
    
    # Test 2: Verify satellite packages depend on Humanizer.Core
    Write-AzureDevOpsSection "Test 2: Verifying satellite packages depend on Humanizer.Core"
    
    $satelliteCount = 0
    foreach ($satellite in $satellitePackages) {
        $pkgName = $satellite.Name -replace "\.$PackageVersion\.nupkg$", ""
        $satelliteCount++
        Write-Host "[$satelliteCount/$($satellitePackages.Count)] Testing $pkgName..."
        
        $testDir = Join-Path $tempDir "SatelliteTest_$satelliteCount"
        New-Item -ItemType Directory -Path $testDir -Force | Out-Null
        Push-Location $testDir
        
        $output = dotnet new console -n Test --force 2>&1
        if ($LASTEXITCODE -ne 0) {
            Pop-Location
            Write-AzureDevOpsError "Failed to create test project for $pkgName"
            throw "Failed to create test project for $pkgName"
        }
        
        Set-Location Test
        
        $output = dotnet add package $pkgName --version $PackageVersion --no-restore 2>&1
        if ($LASTEXITCODE -ne 0) {
            Pop-Location
            Write-AzureDevOpsError "Failed to add $pkgName package reference"
            throw "Failed to add $pkgName package reference"
        }
        
        $restoreOutput = dotnet restore --configfile $nugetConfig 2>&1
        if ($LASTEXITCODE -ne 0) {
            Pop-Location
            Write-AzureDevOpsError "Failed to restore $pkgName"
            Write-Host $restoreOutput
            throw "Failed to restore $pkgName"
        }
        
        # Check if Humanizer.Core was pulled in
        $objPath = "obj/project.assets.json"
        if (-not (Test-Path $objPath)) {
            Pop-Location
            Write-AzureDevOpsError "project.assets.json not found for $pkgName"
            throw "project.assets.json not found for $pkgName"
        }
        
        $assets = Get-Content $objPath | ConvertFrom-Json
        $hasCoreReference = $assets.libraries.PSObject.Properties.Name -contains "Humanizer.Core/$PackageVersion"
        
        if (-not $hasCoreReference) {
            Pop-Location
            Write-AzureDevOpsError "$pkgName does not depend on Humanizer.Core"
            throw "$pkgName does not depend on Humanizer.Core"
        }
        
        Write-Host "  ##[command]✓ $pkgName correctly depends on Humanizer.Core"
        Pop-Location
    }
    
    Write-Host ""
    Write-AzureDevOpsSection "✓ All Verification Checks Passed"
    Write-Host "##[command]Summary:"
    Write-Host "  - Main metapackage (Humanizer): ✓"
    Write-Host "  - Core package (Humanizer.Core): ✓"
    Write-Host "  - Satellite packages verified: $($satellitePackages.Count)"
    Write-Host "  - All satellites are dependencies of metapackage: ✓"
    Write-Host "  - All satellites depend on Humanizer.Core: ✓"
    
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