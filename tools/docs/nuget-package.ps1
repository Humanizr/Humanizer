function Invoke-DocsCheckedCommand {
    param(
        [Parameter(Mandatory = $true)][string]$FilePath,
        [Parameter(Mandatory = $true)][string[]]$ArgumentList,
        [Parameter(Mandatory = $true)][string]$WorkingDirectory
    )

    $logPath = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-command-$([guid]::NewGuid().ToString('N')).log"
    Push-Location $WorkingDirectory
    try {
        & $FilePath @ArgumentList *> $logPath
        if ($LASTEXITCODE -ne 0) {
            $details = Get-Content -Raw $logPath
            throw "$FilePath failed with exit code $LASTEXITCODE.`n$details"
        }
    } finally {
        Pop-Location
        Remove-Item $logPath -Force -ErrorAction SilentlyContinue
    }
}

function Assert-DocsNuGetPackage {
    param(
        [Parameter(Mandatory = $true)][string]$PackageId,
        [Parameter(Mandatory = $true)][string]$PackageVersion,
        [Parameter(Mandatory = $true)][string]$PackagePath,
        [Parameter(Mandatory = $true)][string]$RepoRoot
    )

    $registrationUri = "https://api.nuget.org/v3/registration5-semver1/$PackageId/$PackageVersion.json"
    $registration = Invoke-RestMethod `
        -Uri $registrationUri `
        -MaximumRetryCount 6 `
        -RetryIntervalSec 10 `
        -TimeoutSec 30
    $catalog = Invoke-RestMethod `
        -Uri $registration.catalogEntry `
        -MaximumRetryCount 6 `
        -RetryIntervalSec 10 `
        -TimeoutSec 30
    if ($catalog.packageHashAlgorithm -ne "SHA512" -or -not $catalog.packageHash) {
        throw "NuGet.org did not publish a SHA-512 package hash for $PackageId $PackageVersion."
    }

    $actualHash = [Convert]::ToBase64String(
        [System.Security.Cryptography.SHA512]::HashData(
            [System.IO.File]::ReadAllBytes($PackagePath)
        )
    )
    if ($actualHash -ne $catalog.packageHash) {
        throw "NuGet.org package hash validation failed for $PackageId $PackageVersion."
    }

    $archive = [System.IO.Compression.ZipFile]::OpenRead($PackagePath)
    try {
        if (-not ($archive.Entries.FullName -contains ".signature.p7s")) {
            throw "NuGet package is unsigned: $PackageId $PackageVersion."
        }
    } finally {
        $archive.Dispose()
    }

    try {
        Invoke-DocsCheckedCommand `
            -FilePath "dotnet" `
            -ArgumentList @(
                "nuget", "verify", $PackagePath,
                "--all",
                "--configfile", (Join-Path $RepoRoot "nuget.config")
            ) `
            -WorkingDirectory $RepoRoot
        Write-Host "NuGet signature passed: $PackageId $PackageVersion"
    } catch {
        $allowedHistoricalCodes = @("NU3028", "NU3037")
        $reportedCodes = @(
            [regex]::Matches($_.Exception.Message, "NU\d{4}") |
                ForEach-Object Value |
                Sort-Object -Unique
        )
        $unexpectedCodes = @(
            $reportedCodes |
                Where-Object { $_ -notin $allowedHistoricalCodes }
        )
        if ($reportedCodes.Count -eq 0 -or $unexpectedCodes.Count -gt 0) {
            throw
        }

        Write-Host "NuGet signature is historical; signed package and NuGet.org SHA-512 passed: $PackageId $PackageVersion"
    }
}

function Get-DocsNuGetArchive {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [string]$PackageId
    )

    if ($Entry.source.kind -ne "nuget") {
        throw "Version $($Entry.version) is not backed by a NuGet package."
    }

    $packageVersion = $Entry.source.packageVersion
    if (-not $packageVersion -or (-not $PackageId -and -not $Entry.apiPackage)) {
        throw "Version $($Entry.version) is missing NuGet package metadata."
    }

    $packageId = if ($PackageId) {
        $PackageId.ToLowerInvariant()
    } else {
        $Entry.apiPackage.ToLowerInvariant()
    }
    $cacheRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-nuget"
    $packageRoot = Join-Path $cacheRoot "$packageId/$packageVersion"
    $packagePath = Join-Path $packageRoot "$packageId.$packageVersion.nupkg"

    New-Item -ItemType Directory -Path $packageRoot -Force | Out-Null
    $downloadPackage = {
        $uri = "https://api.nuget.org/v3-flatcontainer/$packageId/$packageVersion/$packageId.$packageVersion.nupkg"
        $downloadPath = "$packagePath.$([guid]::NewGuid().ToString('N')).tmp"
        try {
            Invoke-WebRequest `
                -Uri $uri `
                -OutFile $downloadPath `
                -MaximumRetryCount 6 `
                -RetryIntervalSec 10 `
                -TimeoutSec 30
            Assert-DocsNuGetPackage `
                -PackageId $packageId `
                -PackageVersion $packageVersion `
                -PackagePath $downloadPath `
                -RepoRoot $RepoRoot
            Move-Item $downloadPath $packagePath -Force
        } finally {
            Remove-Item $downloadPath -Force -ErrorAction SilentlyContinue
        }
    }

    if (Test-Path $packagePath -PathType Leaf) {
        try {
            Assert-DocsNuGetPackage `
                -PackageId $packageId `
                -PackageVersion $packageVersion `
                -PackagePath $packagePath `
                -RepoRoot $RepoRoot
        } catch {
            Remove-Item $packagePath -Force
            & $downloadPackage
        }
    } else {
        & $downloadPackage
    }

    return [PSCustomObject]@{
        PackageId = $packageId
        PackageVersion = $packageVersion
        PackagePath = $packagePath
    }
}

function Use-DocsNuGetPackage {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][scriptblock]$Action,
        [string]$PackageId
    )

    $archive = Get-DocsNuGetArchive `
        -Entry $Entry `
        -RepoRoot $RepoRoot `
        -PackageId $PackageId
    $extractPath = Join-Path (
        [System.IO.Path]::GetTempPath()
    ) "humanizer-docs-extract-$([guid]::NewGuid().ToString('N'))"
    New-Item -ItemType Directory -Path $extractPath | Out-Null
    try {
        [System.IO.Compression.ZipFile]::ExtractToDirectory(
            $archive.PackagePath,
            $extractPath
        )
        & $Action ([PSCustomObject]@{
            PackageId = $archive.PackageId
            PackageVersion = $archive.PackageVersion
            PackagePath = $archive.PackagePath
            ExtractPath = $extractPath
        })
    } finally {
        Remove-Item $extractPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}

function Assert-DocsHumanizerPackageGraph {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)]$Libraries,
        [Parameter(Mandatory = $true)][string]$Context
    )

    $expectedVersion = [string]$Entry.source.packageVersion
    $resolvedIds = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::OrdinalIgnoreCase
    )
    foreach ($library in $Libraries.PSObject.Properties) {
        $separator = $library.Name.LastIndexOf("/")
        if ($separator -le 0) {
            continue
        }

        $packageId = $library.Name.Substring(0, $separator)
        if (-not $packageId.Equals(
                "Humanizer",
                [System.StringComparison]::OrdinalIgnoreCase
            ) -and
            -not $packageId.StartsWith(
                "Humanizer.",
                [System.StringComparison]::OrdinalIgnoreCase
            )) {
            continue
        }

        $packageVersion = $library.Name.Substring($separator + 1)
        if ($library.Value.type -ne "package" -or
            $packageVersion -ne $expectedVersion) {
            throw "$Context resolved $packageId $packageVersion instead of package version $expectedVersion."
        }
        [void]$resolvedIds.Add($packageId)
    }

    foreach ($requiredId in @(
        [string]$Entry.installPackage,
        [string]$Entry.apiPackage
    ) | Sort-Object -Unique) {
        if (-not $resolvedIds.Contains($requiredId)) {
            throw "$Context did not resolve required package $requiredId $expectedVersion."
        }
    }
}
