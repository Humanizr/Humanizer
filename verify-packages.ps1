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

$script:AzureDevOpsErrorLogged = $false
$script:AzureDevOpsWarningLogged = $false

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
    if (-not [string]::IsNullOrEmpty($Message)) {
        $escaped = ConvertTo-AzureDevOpsCommandValue $Message
        Write-Host "##vso[task.logissue type=error;]$escaped"
        $script:AzureDevOpsErrorLogged = $true
    }
}

function Write-AzureDevOpsWarning {
    param([string]$Message)
    if (-not [string]::IsNullOrEmpty($Message)) {
        $escaped = ConvertTo-AzureDevOpsCommandValue $Message
        Write-Host "##vso[task.logissue type=warning;]$escaped"
        $script:AzureDevOpsWarningLogged = $true
    }
}

function Write-AzureDevOpsInformation {
    param([string]$Message)
    if (-not [string]::IsNullOrEmpty($Message)) {
        $escaped = ConvertTo-AzureDevOpsCommandValue $Message
        Write-Host "##vso[task.logissue type=info;]$escaped"
    }
}

function Invoke-CapturedProcess {
    param(
        [Parameter(Mandatory = $true)][string]$FilePath,
        [string[]]$ArgumentList = @(),
        [string]$WorkingDirectory
    )

    $startInfo = New-Object System.Diagnostics.ProcessStartInfo
    $startInfo.FileName = $FilePath
    $startInfo.UseShellExecute = $false
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.CreateNoWindow = $true
    if ($PSVersionTable.PSVersion.Major -ge 7 -and $null -ne $startInfo) {
        if ($startInfo.PSObject.Properties.Match('StandardOutputEncoding').Count -gt 0) {
            $startInfo.StandardOutputEncoding = [System.Text.Encoding]::UTF8
        }
        if ($startInfo.PSObject.Properties.Match('StandardErrorEncoding').Count -gt 0) {
            $startInfo.StandardErrorEncoding = [System.Text.Encoding]::UTF8
        }
    }

    if ($ArgumentList) {
        if ($startInfo.PSObject.Properties.Match('ArgumentList').Count -gt 0) {
            foreach ($argument in $ArgumentList) {
                [void]$startInfo.ArgumentList.Add($argument)
            }
        } else {
            $escapedArguments = $ArgumentList | ForEach-Object {
                if ($_ -match '\s' -or $_ -match '"') {
                    '"' + ($_ -replace '"', '\"') + '"'
                } else {
                    $_
                }
            }
            $startInfo.Arguments = [string]::Join(' ', $escapedArguments)
        }
    }

    if ($WorkingDirectory) {
        $startInfo.WorkingDirectory = $WorkingDirectory
    }

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $startInfo

    $null = $process.Start()

    $standardOutput = $process.StandardOutput.ReadToEnd()
    $standardError = $process.StandardError.ReadToEnd()

    $process.WaitForExit()

    $exitCode = $process.ExitCode

    $process.Dispose()

    $combinedParts = @()
    if ($standardOutput) { $combinedParts += $standardOutput }
    if ($standardError) { $combinedParts += $standardError }

    return [PSCustomObject]@{
        ExitCode        = $exitCode
        StandardOutput  = $standardOutput
        StandardError   = $standardError
        CombinedOutput  = ($combinedParts -join "`n")
    }
}

function Get-NormalizedVersionString {
    param([string]$Version)

    if ([string]::IsNullOrWhiteSpace($Version)) {
        return $Version
    }

    $prefix = $Version.Split('-')[0].Split('+')[0]
    $segments = $prefix.Split('.')
    if ($segments.Length -le 3) {
        return $prefix
    }

    return ($segments[0..2] -join '.')
}

function Get-MSBuildProductName {
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path)) {
        return $null
    }

    if ($Path -match "Microsoft Visual Studio\\(\\d{4})\\BuildTools") {
        return "VS $($Matches[1]) Build Tools"
    }

    if ($Path -match "Microsoft Visual Studio\\(\\d{4})\\") {
        $year = $Matches[1]
        return "VS $year"
    }

    if ($Path -match "Microsoft.NET\\Framework64") {
        return ".NETFX"
    }

    if ($Path -match "Microsoft.NET\\Framework") {
        return ".NETFX"
    }

    return $null
}

function Get-MSBuildInfos {
    param([bool]$RunningOnWindows)

    if (-not $RunningOnWindows) {
        return @()
    }

    $msbuildPaths = @()

    $msbuildCommands = @(Get-Command msbuild.exe -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source -Unique)
    if ($msbuildCommands) {
        $msbuildPaths += $msbuildCommands
    }

    $programFilesX86 = [Environment]::GetEnvironmentVariable("ProgramFiles(x86)")
    if (-not [string]::IsNullOrWhiteSpace($programFilesX86)) {
        $vswherePath = Join-Path $programFilesX86 "Microsoft Visual Studio/Installer/vswhere.exe"
        if (Test-Path $vswherePath) {
            $vswhereArguments = @(
                "-prerelease",
                "-products", "*",
                "-requires", "Microsoft.Component.MSBuild",
                "-find", "MSBuild/**/Bin/MSBuild.exe",
                "-all"
            )
            $vswhereOutput = & $vswherePath @vswhereArguments 2>$null
            if ($vswhereOutput) {
                $msbuildPaths += $vswhereOutput
            }
        }
    }

    $windowsDir = [Environment]::GetEnvironmentVariable("WINDIR")
    if (-not [string]::IsNullOrWhiteSpace($windowsDir)) {
        $framework64 = Join-Path $windowsDir "Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe"
        $framework32 = Join-Path $windowsDir "Microsoft.NET/Framework/v4.0.30319/MSBuild.exe"

        if (Test-Path $framework64) {
            $msbuildPaths += $framework64
        } elseif (Test-Path $framework32) {
            $msbuildPaths += $framework32
        }
    }

    $msbuildPaths = $msbuildPaths | Where-Object { $_ -and (Test-Path $_) } | Sort-Object -Unique
    $msbuildInfos = @()

    foreach ($msbuildPath in $msbuildPaths) {
        $msbuildItem = Get-Item $msbuildPath
        $msbuildVersion = $null
        try {
            $msbuildVersion = $msbuildItem.VersionInfo.ProductVersion
            if (-not $msbuildVersion -and $msbuildItem.VersionInfo.FileVersion) {
                $msbuildVersion = $msbuildItem.VersionInfo.FileVersion
            }
        } catch {
            $msbuildVersion = $null
        }

        $normalizedMsbuildVersion = Get-NormalizedVersionString $msbuildVersion
        $productName = Get-MSBuildProductName $msbuildPath
        if (-not $productName -and $msbuildItem.VersionInfo -and $msbuildItem.VersionInfo.ProductName) {
            $productName = $msbuildItem.VersionInfo.ProductName
        }

        $baseName = if ($normalizedMsbuildVersion) { "MSBuild $normalizedMsbuildVersion" } else { "MSBuild" }
        $displayName = if ($productName) { "$baseName ($productName)" } else { $baseName }

        $msbuildInfos += [PSCustomObject]@{
            Path        = $msbuildPath
            RawVersion  = $msbuildVersion
            Version     = $normalizedMsbuildVersion
            ProductName = $productName
            DisplayName = $displayName
        }
    }

    return $msbuildInfos
}

function Set-ProjectTargetFramework {
    param(
        [Parameter(Mandatory = $true)][string]$ProjectPath,
        [Parameter(Mandatory = $true)][string]$TargetFramework
    )

    if (-not (Test-Path $ProjectPath)) {
        return
    }

    $xmlDocument = New-Object System.Xml.XmlDocument
    $xmlDocument.PreserveWhitespace = $true

    try {
        $xmlDocument.Load($ProjectPath)
    } catch {
        return
    }

    $namespaceUri = $xmlDocument.DocumentElement.NamespaceURI
    $namespaceManager = $null

    if (-not [string]::IsNullOrEmpty($namespaceUri)) {
        $namespaceManager = New-Object System.Xml.XmlNamespaceManager($xmlDocument.NameTable)
        $namespaceManager.AddNamespace('msb', $namespaceUri)
    }

    if ($namespaceManager) {
        $targetNode = $xmlDocument.SelectSingleNode('//msb:TargetFramework', $namespaceManager)
        if (-not $targetNode) {
            $targetNode = $xmlDocument.SelectSingleNode('//msb:TargetFrameworks', $namespaceManager)
        }
        $propertyGroup = $xmlDocument.SelectSingleNode('//msb:PropertyGroup', $namespaceManager)
    } else {
        $targetNode = $xmlDocument.SelectSingleNode('//TargetFramework')
        if (-not $targetNode) {
            $targetNode = $xmlDocument.SelectSingleNode('//TargetFrameworks')
        }
        $propertyGroup = $xmlDocument.SelectSingleNode('//PropertyGroup')
    }

    if (-not $propertyGroup) {
        if ($namespaceManager) {
            $propertyGroup = $xmlDocument.CreateElement('PropertyGroup', $namespaceUri)
        } else {
            $propertyGroup = $xmlDocument.CreateElement('PropertyGroup')
        }
        [void]$xmlDocument.DocumentElement.AppendChild($propertyGroup)
    }

    if ($targetNode) {
        $targetNode.InnerText = $TargetFramework
    } else {
        if ($namespaceManager) {
            $targetNode = $xmlDocument.CreateElement('TargetFramework', $namespaceUri)
        } else {
            $targetNode = $xmlDocument.CreateElement('TargetFramework')
        }
        $targetNode.InnerText = $TargetFramework
        [void]$propertyGroup.AppendChild($targetNode)
    }

    $xmlDocument.Save($ProjectPath)
}

function Write-AzureDevOpsErrorDetail {
    param(
        [string]$Summary,
        [string]$Details
    )

    if ([string]::IsNullOrWhiteSpace($Summary) -and [string]::IsNullOrWhiteSpace($Details)) {
        return
    }

    $message = $null

    if (-not [string]::IsNullOrWhiteSpace($Summary)) {
        $message = $Summary.Trim()
    }

    if (-not [string]::IsNullOrWhiteSpace($Details)) {
        $normalizedDetails = ($Details -replace "(`r`n|`r)", "`n").Trim("`n")
        if ($message) {
            $message = "$message`n$normalizedDetails"
        } else {
            $message = $normalizedDetails
        }
    }

    Write-AzureDevOpsError $message
}

function Get-RestoreDiagnostics {
    param([string]$Output)

    if ([string]::IsNullOrEmpty($Output)) {
        return [PSCustomObject]@{ Errors = @(); Warnings = @() }
    }

    $lines = $Output -split "(`r`n|`r|`n)"
    $errorLines = @()
    $warningLines = @()
    foreach ($line in $lines) {
        $trimmed = $line.TrimEnd()
        if ([string]::IsNullOrWhiteSpace($trimmed)) {
            continue
        }

        if ($trimmed -match "(?i)\berror\s*:?\s*[A-Z0-9]+:") {
            $match = [regex]::Match($trimmed, "(?i)\berror\s*:?\s*[A-Z0-9]+:.*")
            if ($match.Success) {
                $errorLines += $match.Value.Trim()
            } else {
                $errorLines += $trimmed
            }
            continue
        }

        if ($trimmed -match "(?i)\bwarning\s*:?\s*[A-Z0-9]+:") {
            $match = [regex]::Match($trimmed, "(?i)\bwarning\s*:?\s*[A-Z0-9]+:.*")
            if ($match.Success) {
                $warningLines += $match.Value.Trim()
            } else {
                $warningLines += $trimmed
            }
        }
    }

    return [PSCustomObject]@{
        Errors   = $errorLines | Select-Object -Unique
        Warnings = $warningLines | Select-Object -Unique
    }
}

function Get-RestoreTargets {
    param([bool]$RunningOnWindows)

    Write-AzureDevOpsSection "Detecting installed .NET SDKs and MSBuild tools"

    $restoreTargets = @()

    $listSdksResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList @("--list-sdks")
    if ($listSdksResult.ExitCode -ne 0) {
        Write-AzureDevOpsErrorDetail "Failed to enumerate installed SDKs" $listSdksResult.CombinedOutput
        if ($listSdksResult.StandardOutput) { Write-Host $listSdksResult.StandardOutput }
        if ($listSdksResult.StandardError) { Write-Host $listSdksResult.StandardError }
        throw "Unable to determine installed SDKs"
    }

    if ($listSdksResult.StandardOutput) { Write-Host $listSdksResult.StandardOutput }
    if ($listSdksResult.StandardError) { Write-Host $listSdksResult.StandardError }

    $installedSdkLines = @()
    if ($listSdksResult.StandardOutput) {
        $installedSdkLines = ($listSdksResult.StandardOutput -split "(`r`n|`r|`n)") | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    }

    $sdksByMajor = @{}
    foreach ($line in $installedSdkLines) {
        if ($line -match '^(?<version>[^\s]+)\s+\[(?<path>.+)\]$') {
            $version = $Matches['version']
            $major = $version.Split('.')[0]
            if (-not $sdksByMajor.ContainsKey($major)) {
                $sdksByMajor[$major] = @()
            }
            $sdksByMajor[$major] += $version
        }
    }

    $sdkVersionsToTest = @(
        @{ Version = "8.0.100"; RollForward = "latestFeature"; Name = "SDK 8"; MajorVersion = 8; TargetFramework = "net8.0" },
        @{ Version = "9.0.100"; RollForward = "latestFeature"; Name = "SDK 9"; MajorVersion = 9; TargetFramework = "net9.0" },
        @{ Version = "10.0.100-rc.2"; RollForward = "latestFeature"; Name = "SDK 10"; MajorVersion = 10; TargetFramework = "net10.0" }
    )

    foreach ($sdkConfig in $sdkVersionsToTest) {
        $majorKey = [string]$sdkConfig.MajorVersion
        if ($sdksByMajor.ContainsKey($majorKey)) {
            $availableVersions = $sdksByMajor[$majorKey] | Sort-Object -Descending
            $selectedVersion = $availableVersions | Select-Object -First 1
            $normalizedVersion = Get-NormalizedVersionString $selectedVersion
            if (-not $normalizedVersion) {
                $normalizedVersion = Get-NormalizedVersionString $sdkConfig.Version
            }

            $displayName = if ($normalizedVersion) { ".NET SDK $normalizedVersion" } else { ".NET SDK $($sdkConfig.MajorVersion)" }

            $restoreTargets += [PSCustomObject]@{
                Kind              = 'dotnet'
                Id                = "sdk$($sdkConfig.MajorVersion)"
                DisplayName       = $displayName
                Version           = $normalizedVersion
                TargetFramework   = $sdkConfig.TargetFramework
                GlobalJsonVersion = $sdkConfig.Version
                RollForward       = $sdkConfig.RollForward
            }
        } else {
            Write-AzureDevOpsWarning "$($sdkConfig.Name) not installed, skipping"
        }
    }

    $dotnetTargets = $restoreTargets | Where-Object { $_.Kind -eq 'dotnet' }
    if ($dotnetTargets.Count -eq 0) {
        $sdkMajorVersions = ($sdkVersionsToTest | ForEach-Object { $_.MajorVersion }) -join ", "
        Write-AzureDevOpsWarning "No target SDK versions ($sdkMajorVersions) are installed"
    }

    if ($RunningOnWindows) {
        $msbuildInfos = Get-MSBuildInfos -RunningOnWindows $RunningOnWindows
        if ($msbuildInfos.Count -eq 0) {
            Write-AzureDevOpsWarning "No MSBuild installations detected"
        } else {
            $index = 0
            foreach ($info in $msbuildInfos) {
                $index++
                $restoreTargets += [PSCustomObject]@{
                    Kind        = 'msbuild'
                    Id          = "msbuild$index"
                    DisplayName = $info.DisplayName
                    Version     = $info.Version
                    Path        = $info.Path
                }
            }
        }
    } else {
        Write-AzureDevOpsWarning "MSBuild restore tests skipped (non-Windows environment)"
    }

    if ($restoreTargets.Count -gt 0) {
        Write-Host "Discovered restore targets:"
        foreach ($target in $restoreTargets) {
            Write-Host "  - $($target.DisplayName)"
        }
    } else {
        Write-AzureDevOpsWarning "No restore targets were discovered"
    }

    return $restoreTargets
}

function Invoke-DotnetRestoreTarget {
    param(
        [PSCustomObject]$Target,
        [string]$TempDir,
        [string]$NuGetConfig,
        [string]$PackageVersion
    )

    $resultRecord = [PSCustomObject]@{
        Kind        = 'dotnet'
        DisplayName = $Target.DisplayName
        Version     = $Target.Version
        Success     = $false
        Details     = $null
    }

    $sdkTestDir = Join-Path $TempDir $Target.Id
    New-Item -ItemType Directory -Path $sdkTestDir -Force | Out-Null

    $currentLocation = Get-Location
    try {
        Set-Location $sdkTestDir

        $globalJsonContent = @"
{
  "sdk": {
    "version": "$($Target.GlobalJsonVersion)",
    "rollForward": "$($Target.RollForward)"
  }
}
"@
        Set-Content -Path "global.json" -Value $globalJsonContent
        Write-Host "Created global.json targeting $($Target.GlobalJsonVersion) (rollForward $($Target.RollForward))"

        Write-Host "Creating test project..."
        $createProjectResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList @("new", "console", "-n", "MetaTest", "--force", "--framework", $Target.TargetFramework) -WorkingDirectory (Get-Location).Path
        if ($createProjectResult.ExitCode -ne 0) {
            $diagnostics = Publish-RestoreFailure "Restore failed for $($Target.DisplayName) while creating test project" $Target.DisplayName $createProjectResult
            $resultRecord.Details = Get-FailureDetailText -Diagnostics $diagnostics -ProcessResult $createProjectResult
            return $resultRecord
        }

        Set-Location MetaTest

        $projectPath = Join-Path (Get-Location).Path "MetaTest.csproj"
        Set-ProjectTargetFramework -ProjectPath $projectPath -TargetFramework $Target.TargetFramework

        Write-Host "Adding Humanizer package reference..."
        $addPackageArguments = @("add", "package", "Humanizer", "--version", $PackageVersion, "--framework", $Target.TargetFramework)
        $addPackageResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList $addPackageArguments -WorkingDirectory (Get-Location).Path
        if ($addPackageResult.ExitCode -ne 0) {
            $diagnostics = Publish-RestoreFailure "Restore failed for $($Target.DisplayName) while adding Humanizer package reference" $Target.DisplayName $addPackageResult
            $resultRecord.Details = Get-FailureDetailText -Diagnostics $diagnostics -ProcessResult $addPackageResult
            return $resultRecord
        }

        Write-Host "Restoring packages..."
        $restoreResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList @("restore", "--configfile", $NuGetConfig) -WorkingDirectory (Get-Location).Path
        if ($restoreResult.ExitCode -ne 0) {
            $diagnostics = Publish-RestoreFailure "Restore failed for $($Target.DisplayName) during dotnet restore" $Target.DisplayName $restoreResult
            $resultRecord.Details = Get-FailureDetailText -Diagnostics $diagnostics -ProcessResult $restoreResult
            return $resultRecord
        }

        $objPath = "obj/project.assets.json"
        if (-not (Test-Path $objPath)) {
            Write-AzureDevOpsWarning "project.assets.json not found after restore with $($Target.DisplayName)"
            $resultRecord.Details = "project.assets.json missing"
            return $resultRecord
        }

        Publish-RestoreSuccess "Restore succeeded: $($Target.DisplayName)" $Target.DisplayName $restoreResult
        $resultRecord.Success = $true
        return $resultRecord
    } catch {
        $exceptionText = $_ | Out-String
        $exceptionTrimmed = $exceptionText.Trim()
        Write-AzureDevOpsErrorDetail "Exception testing $($Target.DisplayName): $($_.Exception.Message)" $exceptionTrimmed
        if ($exceptionText) { Write-Host $exceptionText }
        $resultRecord.Details = $exceptionTrimmed
        return $resultRecord
    } finally {
        Set-Location $currentLocation
    }
}

function Invoke-MSBuildRestoreTarget {
    param(
        [PSCustomObject]$Target,
        [string]$TempDir,
        [string]$NuGetConfig,
        [string]$PackageVersion
    )

    $resultRecord = [PSCustomObject]@{
        Kind        = 'msbuild'
        DisplayName = $Target.DisplayName
        Version     = $Target.Version
        Success     = $false
        Details     = $null
    }

    $msbuildTestDir = Join-Path $TempDir $Target.Id
    New-Item -ItemType Directory -Path $msbuildTestDir -Force | Out-Null

    $currentLocation = Get-Location
    try {
        Set-Location $msbuildTestDir

        $projectRoot = Join-Path (Get-Location).Path "MetaTest"
        New-Item -ItemType Directory -Path $projectRoot -Force | Out-Null
        Set-Location $projectRoot

        $projectContent = @"
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="`$(MSBuildExtensionsPath)\`$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('`$(MSBuildExtensionsPath)\`$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '`$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '`$(Platform)' == '' ">AnyCPU</Platform>
    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
    <OutputType>Library</OutputType>
    <RootNamespace>MetaTest</RootNamespace>
    <AssemblyName>MetaTest</AssemblyName>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Humanizer" Version="$PackageVersion" />
  </ItemGroup>
  <Import Project="`$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
</Project>
"@
        Set-Content -Path "MetaTest.csproj" -Value $projectContent -Encoding UTF8

        $projectFile = "MetaTest.csproj"

        $msbuildRestoreResult = Invoke-CapturedProcess -FilePath $Target.Path -ArgumentList @($projectFile, "/t:Restore", "/p:RestoreConfigFile=$NuGetConfig", "/nologo") -WorkingDirectory (Get-Location).Path
        if ($msbuildRestoreResult.ExitCode -ne 0) {
            $diagnostics = Publish-RestoreFailure "Restore failed for $($Target.DisplayName) during MSBuild restore" $Target.DisplayName $msbuildRestoreResult
            $resultRecord.Details = Get-FailureDetailText -Diagnostics $diagnostics -ProcessResult $msbuildRestoreResult
            return $resultRecord
        }

        $msbuildAssetsPath = "obj/project.assets.json"
        if (-not (Test-Path $msbuildAssetsPath)) {
            Write-AzureDevOpsWarning "project.assets.json not found after restore with $($Target.DisplayName)"
            $resultRecord.Details = "project.assets.json missing"
            return $resultRecord
        }

        Publish-RestoreSuccess "Restore succeeded: $($Target.DisplayName)" $Target.DisplayName $msbuildRestoreResult
        $resultRecord.Success = $true
        return $resultRecord
    } catch {
        $exceptionText = $_ | Out-String
        $exceptionTrimmed = $exceptionText.Trim()
        Write-AzureDevOpsErrorDetail "Exception testing $($Target.DisplayName): $($_.Exception.Message)" $exceptionTrimmed
        if ($exceptionText) { Write-Host $exceptionText }
        $resultRecord.Details = $exceptionTrimmed
        return $resultRecord
    } finally {
        Set-Location $currentLocation
    }
}

function Write-RestoreDiagnosticLines {
    param(
        [string]$DisplayName,
        [PSCustomObject]$Diagnostics
    )

    if ($null -eq $Diagnostics) {
        return
    }

    if ($Diagnostics.Warnings -and $Diagnostics.Warnings.Count -gt 0) {
        foreach ($warningLine in $Diagnostics.Warnings | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) {
            Write-AzureDevOpsWarning "${DisplayName}: $($warningLine.Trim())"
        }
    }

    if ($Diagnostics.Errors -and $Diagnostics.Errors.Count -gt 0) {
        foreach ($errorLine in $Diagnostics.Errors | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }) {
            Write-AzureDevOpsError "${DisplayName}: $($errorLine.Trim())"
        }
    }
}

function Publish-RestoreFailure {
    param(
        [string]$Context,
        [string]$DisplayName,
        [PSCustomObject]$ProcessResult
    )

    if (-not [string]::IsNullOrWhiteSpace($Context)) {
        Write-Host "##[command]✗ $Context"
    }

    if ($null -eq $ProcessResult) {
        if (-not [string]::IsNullOrWhiteSpace($Context)) {
            Write-AzureDevOpsError $Context
        }
        return $null
    }

    if ($ProcessResult.StandardOutput) {
        Write-Host $ProcessResult.StandardOutput
    }

    if ($ProcessResult.StandardError) {
        Write-Host $ProcessResult.StandardError
    }

    $diagnostics = Get-RestoreDiagnostics -Output $ProcessResult.CombinedOutput

    Write-RestoreDiagnosticLines -DisplayName $DisplayName -Diagnostics $diagnostics

    if (($diagnostics.Errors.Count -eq 0) -and ($diagnostics.Warnings.Count -eq 0)) {
        $fallbackMessage = $ProcessResult.StandardError
        if ([string]::IsNullOrWhiteSpace($fallbackMessage)) {
            $fallbackMessage = $ProcessResult.StandardOutput
        }

        if (-not [string]::IsNullOrWhiteSpace($fallbackMessage)) {
            $fallbackLines = ($fallbackMessage -split "(`r`n|`r|`n)") | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
            foreach ($line in $fallbackLines) {
                Write-AzureDevOpsError "${DisplayName}: $($line.Trim())"
            }
        }
    }

    return $diagnostics
}

function Publish-RestoreSuccess {
    param(
        [string]$Context,
        [string]$DisplayName,
        [PSCustomObject]$ProcessResult
    )

    if (-not [string]::IsNullOrWhiteSpace($Context)) {
        Write-Host "##[command]✓ $Context"
    }

    if ($null -eq $ProcessResult) {
        return $null
    }

    $diagnostics = Get-RestoreDiagnostics -Output $ProcessResult.CombinedOutput

    Write-RestoreDiagnosticLines -DisplayName $DisplayName -Diagnostics $diagnostics

    return $diagnostics
}

function Get-FailureDetailText {
    param(
        [PSCustomObject]$Diagnostics,
        [PSCustomObject]$ProcessResult
    )

    if ($Diagnostics) {
        if ($Diagnostics.Errors -and $Diagnostics.Errors.Count -gt 0) {
            return ($Diagnostics.Errors -join "`n").Trim()
        }

        if ($Diagnostics.Warnings -and $Diagnostics.Warnings.Count -gt 0) {
            return ($Diagnostics.Warnings -join "`n").Trim()
        }
    }

    if ($ProcessResult) {
        if (-not [string]::IsNullOrWhiteSpace($ProcessResult.StandardError)) {
            return $ProcessResult.StandardError.Trim()
        }

        if (-not [string]::IsNullOrWhiteSpace($ProcessResult.StandardOutput)) {
            return $ProcessResult.StandardOutput.Trim()
        }

        if (-not [string]::IsNullOrWhiteSpace($ProcessResult.CombinedOutput)) {
            return $ProcessResult.CombinedOutput.Trim()
        }
    }

    return $null
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

    # Discover restore targets
    $restoreTestResults = @()
    $verificationFailures = @()

    $runningOnWindows = $false
    try {
        $runningOnWindows = [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)
    } catch {
        $runningOnWindows = $false
    }

    $restoreTargets = Get-RestoreTargets -RunningOnWindows $runningOnWindows

    foreach ($target in $restoreTargets) {
        Write-AzureDevOpsSection "Testing package restoration with $($target.DisplayName)"

        if ($target.Kind -eq 'dotnet') {
            $testResult = Invoke-DotnetRestoreTarget -Target $target -TempDir $tempDir -NuGetConfig $nugetConfig -PackageVersion $PackageVersion
        } elseif ($target.Kind -eq 'msbuild') {
            $testResult = Invoke-MSBuildRestoreTarget -Target $target -TempDir $tempDir -NuGetConfig $nugetConfig -PackageVersion $PackageVersion
        } else {
            continue
        }

        if ($null -ne $testResult) {
            $restoreTestResults += $testResult
        }
    }

    Write-Host ""
    Write-AzureDevOpsSection "Restore Test Results"

    $validRestoreResults = @($restoreTestResults | Where-Object { $_ -and -not [string]::IsNullOrWhiteSpace($_.DisplayName) })

    if ($validRestoreResults.Count -eq 0) {
        Write-AzureDevOpsWarning "No restore tests were executed"
    } else {
        foreach ($result in $validRestoreResults) {
            if ($result.Success) {
                Write-Host "  ✓ $($result.DisplayName)"
            } else {
                Write-Host "  ✗ $($result.DisplayName)"
            }
        }
    }

    # Verify main metapackage can be restored and pulls in satellites
    Write-Host ""
    Write-AzureDevOpsSection "Verifying Humanizer metapackage dependencies"
    Push-Location $tempDir

    $missingSatellites = @()
    $metapackageCheckCompleted = $false

    Write-Host "Creating test project..."
    $metaVerificationSucceeded = $true
    $metaProjectCreated = $false
    $globalCreateResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList @("new", "console", "-n", "MetaTest", "--force", "--framework", "net8.0") -WorkingDirectory (Get-Location).Path
    if ($globalCreateResult.ExitCode -ne 0) {
        Publish-RestoreFailure "Failed to create metapackage verification project" "Humanizer metapackage" $globalCreateResult
        $verificationFailures += "Metapackage verification project creation failed"
        $metaVerificationSucceeded = $false
    } else {
        $metaProjectCreated = $true
    }

    if ($metaVerificationSucceeded -and $metaProjectCreated) {
        Set-Location MetaTest

        $metaProjectPath = Join-Path (Get-Location).Path "MetaTest.csproj"
        Set-ProjectTargetFramework -ProjectPath $metaProjectPath -TargetFramework "net8.0"

        Write-Host "Adding Humanizer package reference..."
        $globalAddPackageArguments = @("add", "package", "Humanizer", "--version", $PackageVersion, "--framework", "net8.0")

        $globalAddPackageResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList $globalAddPackageArguments -WorkingDirectory (Get-Location).Path
        if ($globalAddPackageResult.ExitCode -ne 0) {
            Publish-RestoreFailure "Failed to add Humanizer package reference to metapackage verification project" "Humanizer metapackage" $globalAddPackageResult
            $verificationFailures += "Metapackage verification project setup failed"
            $metaVerificationSucceeded = $false
        }
    }

    if ($metaVerificationSucceeded -and $metaProjectCreated) {
        Write-Host "Restoring packages..."
        $globalRestoreResult = Invoke-CapturedProcess -FilePath "dotnet" -ArgumentList @("restore", "--configfile", $nugetConfig) -WorkingDirectory (Get-Location).Path
        if ($globalRestoreResult.ExitCode -ne 0) {
            Publish-RestoreFailure "Failed to restore Humanizer metapackage" "Humanizer metapackage" $globalRestoreResult
            $verificationFailures += "Metapackage restore failed"
            $metaVerificationSucceeded = $false
        } else {
            Publish-RestoreSuccess "Humanizer metapackage restore succeeded" "Humanizer metapackage" $globalRestoreResult
        }

        if ($metaVerificationSucceeded) {
            $objPath = "obj/project.assets.json"
            if (-not (Test-Path $objPath)) {
                Write-AzureDevOpsError "project.assets.json not found after metapackage restore"
                $verificationFailures += "Metapackage restore assets missing"
                $metaVerificationSucceeded = $false
            } else {
                $assets = Get-Content $objPath | ConvertFrom-Json
                $restoredPackages = $assets.libraries.PSObject.Properties.Name | Where-Object { $_ -like "Humanizer*" }

                Write-Host "Restored packages:"
                $restoredPackages | ForEach-Object { Write-Host "  - $_" }
                Write-Host ""

                $missingSatellites = @()
                foreach ($satellite in $satellitePackages) {
                    $pkgName = $satellite.Name -replace "\.$PackageVersion\.nupkg$", ""
                    $pkgEntry = "$pkgName/$PackageVersion"
                    if ($restoredPackages -notcontains $pkgEntry) {
                        $missingSatellites += $pkgName
                    }
                }

                $metapackageCheckCompleted = $true

                if ($missingSatellites.Count -gt 0) {
                    $missingMessage = "The following satellite packages are NOT dependencies of the Humanizer metapackage: $($missingSatellites -join ', ')"
                    Write-AzureDevOpsError $missingMessage
                    $verificationFailures += "Humanizer metapackage dependencies are incomplete"
                } else {
                    Write-Host "##[command]✓ All $($satellitePackages.Count) satellite packages are dependencies of the Humanizer metapackage"
                }
            }
        }
    }

    if ($metaProjectCreated -and (Get-Location).Path -like "*MetaTest") {
        Set-Location ..
    }

    Pop-Location
    Write-Host ""

    Write-AzureDevOpsSection "Verification Summary"

    $restoreSuccesses = @($validRestoreResults | Where-Object { $_.Success })
    $restoreFailures = @($validRestoreResults | Where-Object { -not $_.Success })
    $restorePassedCount = $restoreSuccesses.Count
    $restoreTotalCount = $validRestoreResults.Count

    $summaryLines = @()
    $summaryLines += "Summary:"
    $summaryLines += "  ✓ Humanizer"
    $summaryLines += "  ✓ Humanizer.Core"

    $satelliteLine = "  ✓ Satellite packages verified: $($satellitePackages.Count)"
    if ($satellitePackages.Count -eq 0) {
        $satelliteLine = "  ✗ Satellite packages verified: 0"
    }
    $summaryLines += $satelliteLine

    if ($metapackageCheckCompleted -and $missingSatellites.Count -eq 0) {
        $summaryLines += "  ✓ All satellites are dependencies of Humanizer"
    } elseif ($metapackageCheckCompleted) {
        $summaryLines += "  ✗ All satellites are dependencies of Humanizer"
    } else {
        $summaryLines += "  ✗ All satellites are dependencies of Humanizer (not verified)"
    }

    $summaryLines += ""
    $summaryLines += "Restore tests passed: $restorePassedCount/$restoreTotalCount"

    if ($restoreTotalCount -eq 0) {
        $summaryLines += "  (no restore tests executed)"
    } else {
        foreach ($result in $restoreSuccesses) {
            $summaryLines += "  ✓ $($result.DisplayName)"
        }
    }

    if ($restoreFailures.Count -gt 0) {
        $summaryLines += "Restore tests failed:"
        foreach ($failure in $restoreFailures) {
            $summaryLines += "  ✗ $($failure.DisplayName)"
        }
    }

    if ($verificationFailures.Count -gt 0) {
        $summaryLines += "Verification checks failed:"
        foreach ($failure in $verificationFailures) {
            $summaryLines += "  ✗ $failure"
        }
    }

    foreach ($line in $summaryLines) {
        Write-Host $line
    }

    $summaryText = $summaryLines -join "`n"

    $hasFailures = ($restoreFailures.Count -gt 0) -or ($verificationFailures.Count -gt 0)

    if ($hasFailures) {
        Write-AzureDevOpsError $summaryText
        $completionMessage = ConvertTo-AzureDevOpsCommandValue $summaryText
        Write-Host "##vso[task.complete result=Failed;]$completionMessage"
    } elseif ($script:AzureDevOpsWarningLogged) {
        Write-AzureDevOpsWarning $summaryText
    } else {
        Write-AzureDevOpsInformation $summaryText
    }

} catch {
    $catchText = ($_ | Out-String).Trim()
    Write-AzureDevOpsErrorDetail $_.Exception.Message $catchText
    throw
} finally {
    # Cleanup
    if (Test-Path $tempDir) {
        Write-Host ""
        Write-Host "Cleaning up temporary directory..."
        Remove-Item -Path $tempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}
