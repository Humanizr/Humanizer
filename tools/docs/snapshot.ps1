param(
    [Parameter(Mandatory = $true)][string]$Version,
    [switch]$Check,
    [switch]$PromoteLatest,
    [string[]]$CorrectPage,
    [string]$ManifestPath,
    [string]$WebsiteRoot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if (-not $WebsiteRoot) {
    $WebsiteRoot = Join-Path $repoRoot "website"
}
$websiteRoot = [System.IO.Path]::GetFullPath($WebsiteRoot)
$canonicalManifestPath = Join-Path $websiteRoot "humanizer-versions.json"
if (-not $ManifestPath) {
    $ManifestPath = $canonicalManifestPath
}
$ManifestPath = [System.IO.Path]::GetFullPath($ManifestPath)
if (-not $Check -and
    $ManifestPath -ne [System.IO.Path]::GetFullPath($canonicalManifestPath)) {
    throw "Snapshot mutation requires the canonical version manifest."
}

& (Join-Path $PSScriptRoot "verify-manifest.ps1") `
    -ManifestPath $ManifestPath `
    -WebsiteRoot $websiteRoot

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$entries = @($manifest.versions)
$entry = @($entries | Where-Object version -eq $Version)
if ($entry.Count -ne 1) {
    throw "Version '$Version' is not declared exactly once."
}
$entry = $entry[0]
if ($entry.version -eq "current") {
    throw "The current preview is the canonical corpus, not a snapshot."
}

$snapshotPath = Join-Path $websiteRoot "versioned_docs/version-$Version"
$sidebarsPath = Join-Path $websiteRoot "versioned_sidebars/version-$Version-sidebars.json"
$overlayRoot = Join-Path $websiteRoot $entry.compatibilityOverlay
$overlay = Get-Content -Raw (Join-Path $overlayRoot "overlay.json") |
    ConvertFrom-Json -Depth 20

function Assert-CorrectionPath {
    param([Parameter(Mandatory = $true)][string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path) -or
        [System.IO.Path]::IsPathRooted($Path) -or
        $Path.Contains("\") -or
        $Path -match "(^|/)\.\.(/|$)" -or
        $Path -match "[*?\[\]]" -or
        $Path -match "^api(/|$)" -or
        ($Path -notmatch "\.mdx?$" -and
            $Path -notmatch "^_examples/.+\.(?:cs|csproj)$")) {
        throw "Historical corrections require exact authored page or runnable example paths: $Path"
    }
}

function Get-DirectoryDigest {
    param([Parameter(Mandatory = $true)][string]$Path)

    $lines = Get-ChildItem $Path -File -Recurse -Force |
        Sort-Object FullName |
        ForEach-Object {
            $relativePath = [System.IO.Path]::GetRelativePath(
                $Path,
                $_.FullName
            ).Replace("\", "/")
            "$relativePath $((Get-FileHash $_.FullName -Algorithm SHA256).Hash)"
        }
    $bytes = [System.Text.Encoding]::UTF8.GetBytes(($lines -join "`n"))
    return [Convert]::ToHexString(
        [System.Security.Cryptography.SHA256]::HashData($bytes)
    )
}

function Write-Json {
    param(
        [Parameter(Mandatory = $true)]$Value,
        [Parameter(Mandatory = $true)][string]$Path,
        [switch]$AsArray
    )

    $json = if ($AsArray) {
        $Value | ConvertTo-Json -Depth 20 -AsArray
    } else {
        $Value | ConvertTo-Json -Depth 20
    }
    [System.IO.File]::WriteAllText(
        $Path,
        "$json`n",
        [System.Text.UTF8Encoding]::new($false)
    )
}

if ($CorrectPage.Count -gt 0) {
    if ($PromoteLatest) {
        throw "Historical correction and latest promotion are mutually exclusive."
    }
    if (-not (Test-Path $snapshotPath -PathType Container)) {
        throw "Historical snapshot does not exist: $Version"
    }

    $corrections = @($CorrectPage | Sort-Object -Unique)
    if ($corrections.Count -ne $CorrectPage.Count) {
        throw "Historical correction paths must be unique."
    }
    $correctionRoot = Join-Path (
        [System.IO.Path]::GetTempPath()
    ) "humanizer-docs-correction-$Version-$([guid]::NewGuid().ToString('N'))"
    $stagedRoot = Join-Path $correctionRoot "staged"
    $backupRoot = Join-Path $correctionRoot "backup"
    New-Item -ItemType Directory -Path $stagedRoot | Out-Null
    New-Item -ItemType Directory -Path $backupRoot | Out-Null
    try {
        $correctionEntries = @(
            foreach ($path in $corrections) {
                Assert-CorrectionPath -Path $path
                if ($path -in @($overlay.exclusions)) {
                    throw "Excluded page cannot be corrected in ${Version}: $path"
                }

                $sourceRoot = if ($path -in @($overlay.replacements)) {
                    $overlayRoot
                } else {
                    Join-Path $websiteRoot "docs"
                }
                $sourcePath = Join-Path $sourceRoot $path
                $targetPath = Join-Path $snapshotPath $path
                if (-not (Test-Path $sourcePath -PathType Leaf) -or
                    -not (Test-Path $targetPath -PathType Leaf)) {
                    throw "Historical correction page is missing: $path"
                }

                $sourceHash = (Get-FileHash $sourcePath -Algorithm SHA256).Hash
                $targetHash = (Get-FileHash $targetPath -Algorithm SHA256).Hash
                if ($Check -and $sourceHash -ne $targetHash) {
                    throw "Historical correction is not applied: $path"
                }

                $stagedPath = Join-Path $stagedRoot $path
                $backupPath = Join-Path $backupRoot $path
                New-Item -ItemType Directory `
                    -Path (Split-Path $stagedPath -Parent) `
                    -Force | Out-Null
                New-Item -ItemType Directory `
                    -Path (Split-Path $backupPath -Parent) `
                    -Force | Out-Null
                Copy-Item $sourcePath $stagedPath
                Copy-Item $targetPath $backupPath

                [PSCustomObject]@{
                    Path = $path
                    SourceHash = $sourceHash
                    TargetHash = $targetHash
                    StagedPath = $stagedPath
                    BackupPath = $backupPath
                    TargetPath = $targetPath
                }
            }
        )

        if (-not $Check) {
            try {
                foreach ($correction in $correctionEntries |
                    Where-Object SourceHash -ne TargetHash) {
                    Copy-Item `
                        $correction.StagedPath `
                        $correction.TargetPath `
                        -Force
                }
            } catch {
                foreach ($correction in $correctionEntries) {
                    Copy-Item `
                        $correction.BackupPath `
                        $correction.TargetPath `
                        -Force
                }
                throw
            }
        }
    } finally {
        Remove-Item $correctionRoot -Recurse -Force -ErrorAction SilentlyContinue
    }

    Write-Host "Historical correction passed: $Version ($($corrections -join ', '))"
    return
}

if ($PromoteLatest -and $entry.published) {
    throw "A published version cannot be promoted through snapshot creation."
}
if (-not $Check -and (Test-Path $snapshotPath)) {
    throw "Snapshot $Version already exists; use -Check or exact -CorrectPage paths."
}
if ($Check -and -not (Test-Path $snapshotPath -PathType Container)) {
    throw "Snapshot $Version does not exist."
}

$stagingRoot = Join-Path (
    [System.IO.Path]::GetTempPath()
) "humanizer-docs-snapshot-$Version-$([guid]::NewGuid().ToString('N'))"
$stagingDocs = Join-Path $stagingRoot "docs"
$stagingSidebar = Join-Path $stagingRoot "sidebars.json"
$stagingVersions = Join-Path $stagingRoot "versions.json"
$stagingManifest = Join-Path $stagingRoot "humanizer-versions.json"
New-Item -ItemType Directory -Path $stagingDocs | Out-Null

try {
    Get-ChildItem (Join-Path $websiteRoot "docs") -Force |
        Where-Object Name -ne "api" |
        Copy-Item -Destination $stagingDocs -Recurse

    foreach ($path in @($overlay.exclusions)) {
        Remove-Item (Join-Path $stagingDocs $path) -Force
    }
    foreach ($path in @($overlay.replacements)) {
        $sourcePath = Join-Path $overlayRoot $path
        $destinationPath = Join-Path $stagingDocs $path
        New-Item -ItemType Directory `
            -Path (Split-Path $destinationPath -Parent) `
            -Force | Out-Null
        Copy-Item $sourcePath $destinationPath -Force
    }

    & (Join-Path $PSScriptRoot "verify-api.ps1") `
        -Version $Version `
        -ManifestPath $ManifestPath `
        -OutputDirectory (Join-Path $stagingDocs "api")

    $apiLanding = @"
---
id: api
title: API reference
sidebar_position: 2
---

# Humanizer API reference

Generated from ``$($entry.apiPackage)`` ``$($entry.source.packageVersion)`` using the ``$($entry.referenceTfm)`` reference assembly.
"@
    Set-Content (Join-Path $stagingDocs "api/index.md") $apiLanding

    & (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version $Version `
        -ManifestPath $ManifestPath `
        -ExamplesRoot (Join-Path $stagingDocs "_examples")

    Copy-Item (Join-Path $websiteRoot "sidebars.json") $stagingSidebar

    if (-not $Check) {
        $entry.published = $true
        if ($PromoteLatest) {
            $previousLatest = @(
                $entries |
                    Where-Object latestStable
            )
            if ($previousLatest.Count -ne 1 -or
                [System.Management.Automation.SemanticVersion]::Parse(
                    $entry.version
                ) -le
                [System.Management.Automation.SemanticVersion]::Parse(
                    $previousLatest[0].version
                )) {
                throw "Latest promotion requires a newer stable version."
            }

            $previousLatest[0].latestStable = $false
            $previousLatest[0].route = $previousLatest[0].version
            $previousLatest[0].label = $previousLatest[0].version
            $entry.latestStable = $true
            $entry.route = ""
            $entry.label = "$Version (latest)"
        }
    }

    $nativeVersions = @(
        $entries |
            Where-Object { $_.version -ne "current" -and $_.published } |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending |
            ForEach-Object version
    )
    Write-Json -Value $nativeVersions -Path $stagingVersions -AsArray
    Write-Json -Value $manifest -Path $stagingManifest

    if ($Check) {
        $mismatches = @()
        if ((Get-DirectoryDigest $stagingDocs) -ne
            (Get-DirectoryDigest $snapshotPath)) {
            $mismatches += "versioned_docs/version-$Version"
        }
        if (-not (Test-Path $sidebarsPath -PathType Leaf) -or
            (Get-FileHash $stagingSidebar -Algorithm SHA256).Hash -ne
            (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash) {
            $mismatches += "versioned_sidebars/version-$Version-sidebars.json"
        }
        $versionsPath = Join-Path $websiteRoot "versions.json"
        if ((Get-FileHash $stagingVersions -Algorithm SHA256).Hash -ne
            (Get-FileHash $versionsPath -Algorithm SHA256).Hash) {
            $mismatches += "versions.json"
        }
        if ($mismatches.Count -gt 0) {
            throw "Snapshot $Version is stale: $($mismatches -join ', ')"
        }

        Write-Host "Snapshot is deterministic and current: $Version"
        return
    }

    & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $stagingManifest `
        -WebsiteRoot $websiteRoot `
        -SkipNativeState

    $versionsPath = Join-Path $websiteRoot "versions.json"
    $originalManifest = Get-Content -Raw $ManifestPath
    $originalVersions = Get-Content -Raw $versionsPath
    $createdSnapshot = $false
    $createdSidebar = $false
    try {
        Move-Item $stagingDocs $snapshotPath
        $createdSnapshot = $true
        Copy-Item $stagingSidebar $sidebarsPath
        $createdSidebar = $true
        Copy-Item $stagingVersions $versionsPath -Force
        Copy-Item $stagingManifest $ManifestPath -Force

        & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
            -ManifestPath $ManifestPath `
            -WebsiteRoot $websiteRoot
    } catch {
        if ($createdSnapshot) {
            Remove-Item $snapshotPath -Recurse -Force
        }
        if ($createdSidebar) {
            Remove-Item $sidebarsPath -Force
        }
        [System.IO.File]::WriteAllText($ManifestPath, $originalManifest)
        [System.IO.File]::WriteAllText($versionsPath, $originalVersions)
        throw
    }

    Write-Host "Created immutable documentation snapshot: $Version"
} finally {
    Remove-Item $stagingRoot -Recurse -Force -ErrorAction SilentlyContinue
}
