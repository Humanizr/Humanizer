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

function Get-DocsNuGetPackage {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$RepoRoot
    )

    if ($Entry.source.kind -ne "nuget") {
        throw "Version $($Entry.version) is not backed by a NuGet package."
    }

    $packageVersion = $Entry.source.packageVersion
    if (-not $packageVersion -or -not $Entry.apiPackage) {
        throw "Version $($Entry.version) is missing NuGet package metadata."
    }

    $packageId = $Entry.apiPackage.ToLowerInvariant()
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

    $packageDigest = (Get-FileHash $packagePath -Algorithm SHA256).Hash.ToLowerInvariant()
    $extractPath = Join-Path $packageRoot "package-$packageDigest"
    if (-not (Test-Path $extractPath -PathType Container)) {
        $temporaryExtractPath = "$extractPath-$([guid]::NewGuid().ToString('N')).tmp"
        try {
            [System.IO.Compression.ZipFile]::ExtractToDirectory(
                $packagePath,
                $temporaryExtractPath
            )
            try {
                [System.IO.Directory]::Move($temporaryExtractPath, $extractPath)
            } catch [System.IO.IOException] {
                if (-not (Test-Path $extractPath -PathType Container)) {
                    throw
                }
            }
        } finally {
            Remove-Item `
                $temporaryExtractPath `
                -Recurse `
                -Force `
                -ErrorAction SilentlyContinue
        }
    }

    return [PSCustomObject]@{
        PackageId = $packageId
        PackageVersion = $packageVersion
        PackagePath = $packagePath
        ExtractPath = $extractPath
    }
}
