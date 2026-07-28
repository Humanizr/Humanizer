param(
    [string]$Version,
    [switch]$All,
    [switch]$Check,
    [switch]$PromoteLatest,
    [string[]]$CorrectPage,
    [string]$ManifestPath,
    [string]$WebsiteRoot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "snapshot-state.ps1")

if (-not $WebsiteRoot) {
    $WebsiteRoot = Join-Path $repoRoot "website"
}
$WebsiteRoot = [System.IO.Path]::GetFullPath($WebsiteRoot)
$canonicalManifestPath = Join-Path $WebsiteRoot "humanizer-versions.json"
if (-not $ManifestPath) {
    $ManifestPath = $canonicalManifestPath
}
$ManifestPath = [System.IO.Path]::GetFullPath($ManifestPath)

if ($All) {
    if ($Version -or $PromoteLatest -or $CorrectPage) {
        throw "-All cannot be combined with version, promotion, or correction."
    }
    if (-not $Check) {
        throw "-All is validation-only and requires -Check."
    }
} elseif (-not $Version) {
    throw "Choose exactly one of -Version or -All."
}
if (-not $Check -and $ManifestPath -ne
    [System.IO.Path]::GetFullPath($canonicalManifestPath)) {
    throw "Snapshot mutation requires the canonical version manifest."
}
if ($CorrectPage -and ($Check -or $PromoteLatest -or $Version -eq "current")) {
    throw "Historical correction cannot check, promote, or target current."
}
if ($Version -eq "current" -and $PromoteLatest) {
    throw "The current preview cannot be promoted."
}

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

function Get-ExcludedDocIds {
    param(
        [Parameter(Mandatory = $true)][string]$DocsRoot,
        [Parameter(Mandatory = $true)][object[]]$Exclusions
    )

    $ids = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    foreach ($path in $Exclusions | Where-Object { $_ -match "\.mdx?$" }) {
        $canonicalPath = Join-Path $DocsRoot $path
        $content = Get-Content -Raw $canonicalPath
        if ($content -notmatch "(?m)^id:\s*(?<id>[^\s]+)\s*$") {
            throw "Excluded documentation page has no explicit ID: $path"
        }
        $directory = [System.IO.Path]::GetDirectoryName($path).Replace("\", "/")
        $id = if ([string]::IsNullOrWhiteSpace($directory)) {
            $Matches["id"]
        } else {
            "$directory/$($Matches["id"])"
        }
        $null = $ids.Add($id)
    }
    return $ids
}

function Convert-SidebarItem {
    param(
        [Parameter(Mandatory = $true)]$Item,
        [Parameter(Mandatory = $true)]
        [System.Collections.Generic.HashSet[string]]$ExcludedIds
    )

    if ($Item -is [string]) {
        if ($ExcludedIds.Contains($Item)) {
            return $null
        }
        return $Item
    }
    if ([string]$Item.type -ne "category") {
        return $Item
    }

    $result = [ordered]@{
        type = "category"
        label = [string]$Item.label
    }
    if ($null -ne $Item.PSObject.Properties["collapsed"]) {
        $result.collapsed = [bool]$Item.collapsed
    }
    if ($null -ne $Item.PSObject.Properties["link"]) {
        if ([string]$Item.link.type -ne "doc" -or
            -not $ExcludedIds.Contains([string]$Item.link.id)) {
            $result.link = $Item.link
        }
    }
    $items = [System.Collections.Generic.List[object]]::new()
    foreach ($child in @($Item.items)) {
        $converted = Convert-SidebarItem `
            -Item $child `
            -ExcludedIds $ExcludedIds
        if ($null -ne $converted) {
            $items.Add($converted)
        }
    }
    if ($items.Count -gt 0) {
        $result.items = @($items)
    }
    if ($items.Count -eq 0 -and -not $result.Contains("link")) {
        return $null
    }
    return [PSCustomObject]$result
}

function New-PrunedSidebar {
    param(
        [Parameter(Mandatory = $true)][string]$SourcePath,
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)]
        [System.Collections.Generic.HashSet[string]]$ExcludedIds
    )

    $source = Get-Content -Raw $SourcePath | ConvertFrom-Json -Depth 100
    $items = [System.Collections.Generic.List[object]]::new()
    foreach ($item in @($source.docs)) {
        $converted = Convert-SidebarItem `
            -Item $item `
            -ExcludedIds $ExcludedIds
        if ($null -ne $converted) {
            $items.Add($converted)
        }
    }
    $labels = @(
        $items |
            Where-Object { $_ -isnot [string] } |
            ForEach-Object label
    )
    foreach ($required in @(
        "Start here",
        "Scenarios",
        "Languages",
        "Upgrading",
        "API reference"
    )) {
        if ($required -notin $labels) {
            throw "Derived sidebar is missing required root '$required'."
        }
    }
    Write-SnapshotJson `
        -Value ([ordered]@{ docs = @($items) }) `
        -Path $OutputPath
}

function Assert-SnapshotLinks {
    param(
        [Parameter(Mandatory = $true)][string]$DocsRoot,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    foreach ($page in Get-ChildItem $DocsRoot -File -Recurse |
        Where-Object Extension -in @(".md", ".mdx")) {
        $resolvedDocsRoot = [System.IO.Path]::GetFullPath($DocsRoot).TrimEnd(
            [System.IO.Path]::DirectorySeparatorChar
        ) + [System.IO.Path]::DirectorySeparatorChar
        $relativePage = [System.IO.Path]::GetRelativePath(
            $DocsRoot,
            $page.FullName
        ).Replace("\", "/")
        $content = Get-Content -Raw $page.FullName
        if ($relativePage -notmatch "^api/" -and
            ($content -match "\]\(/docs/" -or
                $content -match "\b(?:href|to)=[{`"']*/docs/")) {
            throw "$VersionLabel contains a cross-version absolute docs link: $relativePage"
        }
        foreach ($match in [regex]::Matches(
            $content,
            "\]\((?<target>[^)#\s]+)(?:#[^)]*)?\)"
        )) {
            $target = $match.Groups["target"].Value
            if ($target -match "^(?:[a-z]+:|//)" -or
                $target -notmatch "\.mdx?$") {
                continue
            }
            $resolved = [System.IO.Path]::GetFullPath(
                (Join-Path $page.DirectoryName $target)
            )
            if (-not $resolved.StartsWith(
                    $resolvedDocsRoot,
                    [System.StringComparison]::Ordinal
                ) -or
                -not (Test-Path $resolved -PathType Leaf)) {
                throw "$VersionLabel has a broken relative documentation link in $relativePage`: $target"
            }
        }
        foreach ($match in [regex]::Matches(
            $content,
            "!!raw-loader!(?<target>[^'`;]+)"
        )) {
            $target = $match.Groups["target"].Value
            $resolved = [System.IO.Path]::GetFullPath(
                (Join-Path $page.DirectoryName $target)
            )
            if (-not $resolved.StartsWith(
                    $resolvedDocsRoot,
                    [System.StringComparison]::Ordinal
                ) -or
                -not (Test-Path $resolved -PathType Leaf)) {
                throw "$VersionLabel has a broken example import in $relativePage`: $target"
            }
        }
    }
}

function Assert-VersionNarrativeAccuracy {
    param(
        [Parameter(Mandatory = $true)][string]$DocsRoot,
        [Parameter(Mandatory = $true)][string]$Version
    )

    $isTwoX = $Version.StartsWith("2.", [System.StringComparison]::Ordinal)
    $rejectWordForm = $Version -in @("2.10.1", "2.11.10", "2.13.14")
    foreach ($page in Get-ChildItem $DocsRoot -File -Recurse |
        Where-Object Extension -in @(".md", ".mdx")) {
        $relativePage = [System.IO.Path]::GetRelativePath(
            $DocsRoot,
            $page.FullName
        ).Replace("\", "/")
        if ($relativePage -match "^(?:api|upgrading|analyzer)/") {
            continue
        }
        $content = Get-Content -Raw $page.FullName
        foreach ($match in [regex]::Matches(
            $content,
            "dotnet add package Humanizer --version\s+(?<version>\S+)"
        )) {
            if ($match.Groups["version"].Value -ne $Version) {
                throw "$Version contains a mismatched Humanizer install command in ${relativePage}: $($match.Groups["version"].Value)"
            }
        }
        if ($isTwoX) {
            foreach ($forbidden in @(
                "Configurator.UseEnumDescriptionPropertyLocator",
                "DynamicLengthAndPreserveWords",
                "DynamicNumberOfCharactersAndPreserveWords",
                "TryToNumber"
            )) {
                if ($content.Contains(
                    $forbidden,
                    [System.StringComparison]::Ordinal
                )) {
                    throw "$Version contains unavailable '$forbidden' guidance in $relativePage."
                }
            }
        }
        if ($rejectWordForm -and
            $relativePage -in @(
                "languages/grammar-and-platform-formatting.mdx",
                "scenarios/numbers-in-words-and-ordinals.mdx"
            ) -and
            $content.Contains(
            "WordForm",
            [System.StringComparison]::Ordinal
        )) {
            throw "$Version contains unavailable 'WordForm' guidance in $relativePage."
        }
        if ($relativePage -eq
                "languages/grammar-and-platform-formatting.mdx") {
            foreach ($previewOnlyClaim in @(
                '`ByteSize.TryParse` uses only the decimal-separator override',
                '`main/preview` locale data can provide narrowly scoped',
                'YAML-backed `calendar` and `number.formatting` override surfaces'
            )) {
                if ($content.Contains(
                        $previewOnlyClaim,
                        [System.StringComparison]::Ordinal
                    )) {
                    throw "$Version grammar prose contains a preview-only claim: $previewOnlyClaim"
                }
            }
        }
        if ($relativePage -eq "languages/index.mdx" -and
            $content -match "(?i)(?:generated\s+)?main/preview\s+inventory") {
            throw "$Version labels selected language coverage as main/preview inventory."
        }
    }

    if ($Version -ne "2.10.1") {
        $fluentPage = Join-Path $DocsRoot (
            "scenarios/fluent-dates-and-time-spans.mdx"
        )
        if ((Test-Path $fluentPage -PathType Leaf) -and
            -not (Get-Content -Raw $fluentPage).Contains(
                'DateOnly` fluent helpers begin in `2.11.10` on compatible',
                [System.StringComparison]::Ordinal
            )) {
            throw "$Version does not state the exact DateOnly fluent-helper boundary."
        }
    }

    $migrationPage = Join-Path $DocsRoot "upgrading/version-3-migration.mdx"
    if ((Test-Path (Join-Path $DocsRoot "api/Humanizer.Resources.md")) -or
        (Test-Path (Join-Path $DocsRoot "api/Humanizer.ResourceKeys.md"))) {
        $migrationContent = Get-Content -Raw $migrationPage
        if (-not $migrationContent.Contains(
            'remain in stable Humanizer 3 releases through `3.0.10`',
            [System.StringComparison]::Ordinal
        )) {
            throw "$Version migration prose incorrectly removes Resources or ResourceKeys."
        }
    }
}

function Assert-DocumentIdentity {
    param(
        [Parameter(Mandatory = $true)][string]$Source,
        [Parameter(Mandatory = $true)][string]$Target,
        [Parameter(Mandatory = $true)][string]$RelativePath
    )

    if ($RelativePath -notmatch "\.mdx?$") {
        return
    }
    $sourceContent = Get-Content -Raw $Source
    $targetContent = Get-Content -Raw $Target
    if ($sourceContent -notmatch "(?m)^id:\s*(?<id>[^\s]+)\s*$") {
        throw "Correction source has no document identity: $RelativePath"
    }
    $sourceId = $Matches["id"]
    if ($targetContent -notmatch "(?m)^id:\s*(?<id>[^\s]+)\s*$" -or
        $Matches["id"] -ne $sourceId) {
        throw "Correction changes document identity: $RelativePath"
    }
}

function Add-ApiLanding {
    param(
        [Parameter(Mandatory = $true)][string]$ApiRoot,
        [Parameter(Mandatory = $true)]$Entry
    )

    $provenance = if ($Entry.version -eq "current") {
        "Generated from the ``main/preview`` ``$($Entry.apiPackage)`` checkout using the ``$($Entry.referenceTfm)`` reference assembly."
    } else {
        "Generated from ``$($Entry.apiPackage)`` ``$($Entry.source.packageVersion)`` using the ``$($Entry.referenceTfm)`` reference assembly."
    }
    $landing = @"
---
id: api
title: API reference
sidebar_position: 2
---

# Humanizer API reference

$provenance
"@
    [System.IO.File]::WriteAllText(
        (Join-Path $ApiRoot "index.md"),
        "$landing`n",
        [System.Text.UTF8Encoding]::new($false)
    )
}

function Assert-FrozenSnapshot {
    param([Parameter(Mandatory = $true)]$Entry)

    if (-not $Entry.published) {
        throw "Stable snapshot is not published: $($Entry.version)"
    }
    $docsRoot = Join-Path $WebsiteRoot (
        "versioned_docs/version-$($Entry.version)"
    )
    & (Join-Path $PSScriptRoot "verify-api.ps1") `
        -Version $Entry.version `
        -ManifestPath $ManifestPath `
        -WebsiteRoot $WebsiteRoot
    & (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version $Entry.version `
        -ManifestPath $ManifestPath `
        -ExamplesRoot (Join-Path $docsRoot "_examples")
    & (Join-Path $PSScriptRoot "generate-language-coverage.ps1") `
        -Check `
        -Version $Entry.version `
        -ManifestPath $ManifestPath `
        -OutputPath (Join-Path $docsRoot "languages/language-coverage.json")
    Assert-SnapshotLinks `
        -DocsRoot $docsRoot `
        -VersionLabel $Entry.version
    Assert-VersionNarrativeAccuracy `
        -DocsRoot $docsRoot `
        -Version $Entry.version
    & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
        -Version $Entry.version `
        -DocsRoot $docsRoot `
        -ApiRoot (Join-Path $docsRoot "api") `
        -ManifestPath $ManifestPath `
        -WebsiteRoot $WebsiteRoot
}

$mutationLock = $null
try {
    $mutationLock = Enter-SnapshotMutation -WebsiteRoot $WebsiteRoot
    & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $ManifestPath `
        -WebsiteRoot $WebsiteRoot
    $manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
    $entries = @($manifest.versions)

    if ($All) {
        foreach ($published in @(
            $entries | Where-Object { $_.version -ne "current" -and $_.published }
        )) {
            Assert-FrozenSnapshot -Entry $published
        }
        Write-Host "All frozen snapshots passed direct validation."
        return
    }

    $matchingEntries = @($entries | Where-Object version -eq $Version)
    if ($matchingEntries.Count -ne 1) {
        throw "Version '$Version' is not declared exactly once."
    }
    $entry = $matchingEntries[0]

    if ($Version -eq "current") {
        $stagingRoot = Join-Path $WebsiteRoot (
            ".snapshot-current-$([guid]::NewGuid().ToString('N'))"
        )
        $stagingApi = Join-Path $stagingRoot "api"
        try {
            New-Item -ItemType Directory -Path $stagingRoot | Out-Null
            & (Join-Path $PSScriptRoot "verify-api.ps1") `
                -Version current `
                -ManifestPath $ManifestPath `
                -WebsiteRoot $WebsiteRoot `
                -OutputDirectory $stagingApi
            Add-ApiLanding -ApiRoot $stagingApi -Entry $entry
            & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
                -Version current `
                -DocsRoot (Join-Path $WebsiteRoot "docs") `
                -ApiRoot $stagingApi `
                -ManifestPath $ManifestPath `
                -WebsiteRoot $WebsiteRoot

            $liveApi = Join-Path $WebsiteRoot "docs/api"
            if ($Check) {
                if (-not (Test-Path $liveApi -PathType Container) -or
                    (Get-SnapshotDirectoryDigest $liveApi) -ne
                    (Get-SnapshotDirectoryDigest $stagingApi)) {
                    throw "The current API tree is stale or modified."
                }
                Write-Host "Current API passed deterministic validation."
                return
            }

            & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                -ManifestPath $ManifestPath `
                -WebsiteRoot $WebsiteRoot
            Invoke-SnapshotTransaction `
                -WebsiteRoot $WebsiteRoot `
                -Changes @(
                    [PSCustomObject]@{
                        Target = "docs/api"
                        Source = $stagingApi
                    }
                ) `
                -Validate {
                    & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                        -ManifestPath $ManifestPath `
                        -WebsiteRoot $WebsiteRoot
                    & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
                        -Version current `
                        -DocsRoot (Join-Path $WebsiteRoot "docs") `
                        -ApiRoot (Join-Path $WebsiteRoot "docs/api") `
                        -ManifestPath $ManifestPath `
                        -WebsiteRoot $WebsiteRoot
                }
            Write-Host "Refreshed the current API atomically."
            return
        } finally {
            Remove-Item $stagingRoot -Recurse -Force -ErrorAction SilentlyContinue
        }
    }

    $snapshotPath = Join-Path $WebsiteRoot "versioned_docs/version-$Version"
    $sidebarsPath = Join-Path $WebsiteRoot (
        "versioned_sidebars/version-$Version-sidebars.json"
    )
    $overlayRoot = Join-Path $WebsiteRoot $entry.compatibilityOverlay
    $overlay = Get-Content -Raw (Join-Path $overlayRoot "overlay.json") |
        ConvertFrom-Json -Depth 20

    if ($Check) {
        Assert-FrozenSnapshot -Entry $entry
        Write-Host "Frozen snapshot passed direct validation: $Version"
        return
    }

    if ($CorrectPage.Count -gt 0) {
        if (-not $entry.published -or
            -not (Test-Path $snapshotPath -PathType Container)) {
            throw "Historical correction requires a published snapshot: $Version"
        }
        $corrections = @($CorrectPage | Sort-Object -Unique)
        if ($corrections.Count -ne $CorrectPage.Count) {
            throw "Historical correction paths must be unique."
        }
        $stagingRoot = Join-Path $WebsiteRoot (
            ".snapshot-correction-$Version-$([guid]::NewGuid().ToString('N'))"
        )
        $stagingDocs = Join-Path $stagingRoot "docs"
        $stagingManifest = Join-Path $stagingRoot "humanizer-versions.json"
        try {
            New-Item -ItemType Directory -Path $stagingRoot | Out-Null
            Copy-Item $snapshotPath $stagingDocs -Recurse
            $changed = [System.Collections.Generic.List[string]]::new()
            foreach ($path in $corrections) {
                Assert-CorrectionPath -Path $path
                if ($path -in @($overlay.exclusions)) {
                    throw "Excluded page cannot be corrected in ${Version}: $path"
                }
                $sourceRoot = if ($path -in @($overlay.replacements)) {
                    $overlayRoot
                } else {
                    Join-Path $WebsiteRoot "docs"
                }
                $sourcePath = Join-Path $sourceRoot $path
                $targetPath = Join-Path $snapshotPath $path
                if (-not (Test-Path $sourcePath -PathType Leaf) -or
                    -not (Test-Path $targetPath -PathType Leaf)) {
                    throw "Historical correction page is missing: $path"
                }
                Assert-DocumentIdentity `
                    -Source $sourcePath `
                    -Target $targetPath `
                    -RelativePath $path
                $stagedPath = Join-Path $stagingDocs $path
                Copy-Item $sourcePath $stagedPath -Force
                if ((Get-FileHash $sourcePath -Algorithm SHA256).Hash -ne
                    (Get-FileHash $targetPath -Algorithm SHA256).Hash) {
                    $changed.Add($path)
                }
            }

            Assert-SnapshotLinks `
                -DocsRoot $stagingDocs `
                -VersionLabel $Version
            Assert-VersionNarrativeAccuracy `
                -DocsRoot $stagingDocs `
                -Version $Version
            & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
                -Version $Version `
                -DocsRoot $stagingDocs `
                -ApiRoot (Join-Path $stagingDocs "api") `
                -ManifestPath $ManifestPath `
                -WebsiteRoot $WebsiteRoot
            if (@($corrections | Where-Object { $_ -match "^_examples/" }).Count -gt 0) {
                & (Join-Path $PSScriptRoot "verify-examples.ps1") `
                    -Version $Version `
                    -ManifestPath $ManifestPath `
                    -ExamplesRoot (Join-Path $stagingDocs "_examples")
            }
            if ($changed.Count -eq 0) {
                Write-Host "Historical correction is already applied: $Version"
                return
            }

            $entry.immutability.snapshotSha256 = (
                Get-SnapshotDirectoryDigest -Path $stagingDocs
            )
            Write-SnapshotJson -Value $manifest -Path $stagingManifest
            & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                -ManifestPath $stagingManifest `
                -WebsiteRoot $WebsiteRoot `
                -SkipNativeState
            & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                -ManifestPath $ManifestPath `
                -WebsiteRoot $WebsiteRoot

            $changes = [System.Collections.Generic.List[object]]::new()
            foreach ($path in $changed) {
                $changes.Add([PSCustomObject]@{
                    Target = "versioned_docs/version-$Version/$path"
                    Source = (Join-Path $stagingDocs $path)
                })
            }
            $changes.Add([PSCustomObject]@{
                Target = "humanizer-versions.json"
                Source = $stagingManifest
            })
            Invoke-SnapshotTransaction `
                -WebsiteRoot $WebsiteRoot `
                -Changes @($changes) `
                -Validate {
                    & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                        -ManifestPath $ManifestPath `
                        -WebsiteRoot $WebsiteRoot
                    Assert-SnapshotLinks `
                        -DocsRoot $snapshotPath `
                        -VersionLabel $Version
                    Assert-VersionNarrativeAccuracy `
                        -DocsRoot $snapshotPath `
                        -Version $Version
                    & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
                        -Version $Version `
                        -DocsRoot $snapshotPath `
                        -ApiRoot (Join-Path $snapshotPath "api") `
                        -ManifestPath $ManifestPath `
                        -WebsiteRoot $WebsiteRoot
                }
            Write-Host "Applied historical correction: $Version ($($changed -join ', '))"
            return
        } finally {
            Remove-Item $stagingRoot -Recurse -Force -ErrorAction SilentlyContinue
        }
    }

    if ($entry.published -or
        (Test-Path $snapshotPath) -or
        (Test-Path $sidebarsPath)) {
        throw "New snapshot creation requires an unpublished version with absent outputs."
    }

    $stagingRoot = Join-Path $WebsiteRoot (
        ".snapshot-release-$Version-$([guid]::NewGuid().ToString('N'))"
    )
    $stagingDocs = Join-Path $stagingRoot "docs"
    $stagingSidebar = Join-Path $stagingRoot "sidebars.json"
    $stagingVersions = Join-Path $stagingRoot "versions.json"
    $stagingManifest = Join-Path $stagingRoot "humanizer-versions.json"
    try {
        New-Item -ItemType Directory -Path $stagingDocs | Out-Null
        Get-ChildItem (Join-Path $WebsiteRoot "docs") -Force |
            Where-Object Name -ne "api" |
            Copy-Item -Destination $stagingDocs -Recurse
        foreach ($path in @($overlay.exclusions)) {
            Remove-Item (Join-Path $stagingDocs $path) -Force
        }
        foreach ($path in @($overlay.replacements)) {
            $destination = Join-Path $stagingDocs $path
            New-Item -ItemType Directory `
                -Path (Split-Path $destination -Parent) `
                -Force | Out-Null
            Copy-Item (Join-Path $overlayRoot $path) $destination -Force
        }

        $stagingApi = Join-Path $stagingDocs "api"
        & (Join-Path $PSScriptRoot "verify-api.ps1") `
            -Version $Version `
            -ManifestPath $ManifestPath `
            -WebsiteRoot $WebsiteRoot `
            -OutputDirectory $stagingApi
        Add-ApiLanding -ApiRoot $stagingApi -Entry $entry
        & (Join-Path $PSScriptRoot "verify-examples.ps1") `
            -Version $Version `
            -ManifestPath $ManifestPath `
            -ExamplesRoot (Join-Path $stagingDocs "_examples")
        & (Join-Path $PSScriptRoot "generate-language-coverage.ps1") `
            -Version $Version `
            -ManifestPath $ManifestPath `
            -OutputPath (
                Join-Path $stagingDocs "languages/language-coverage.json"
            )
        $excludedIds = Get-ExcludedDocIds `
            -DocsRoot (Join-Path $WebsiteRoot "docs") `
            -Exclusions @($overlay.exclusions)
        New-PrunedSidebar `
            -SourcePath (Join-Path $WebsiteRoot "sidebars.json") `
            -OutputPath $stagingSidebar `
            -ExcludedIds $excludedIds
        Assert-SnapshotLinks `
            -DocsRoot $stagingDocs `
            -VersionLabel $Version
        Assert-VersionNarrativeAccuracy `
            -DocsRoot $stagingDocs `
            -Version $Version
        & (Join-Path $PSScriptRoot "verify-scenario-api.ps1") `
            -Version $Version `
            -DocsRoot $stagingDocs `
            -ApiRoot $stagingApi `
            -ManifestPath $ManifestPath `
            -WebsiteRoot $WebsiteRoot

        $entry.published = $true
        if ($PromoteLatest) {
            $previousLatest = @($entries | Where-Object latestStable)
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
        $entry | Add-Member -NotePropertyName immutability -NotePropertyValue (
            [PSCustomObject]@{
                snapshotSha256 = Get-SnapshotDirectoryDigest -Path $stagingDocs
                sidebarSha256 = (
                    Get-FileHash $stagingSidebar -Algorithm SHA256
                ).Hash
            }
        )
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
        Write-SnapshotJson `
            -Value $nativeVersions `
            -Path $stagingVersions `
            -AsArray
        Write-SnapshotJson -Value $manifest -Path $stagingManifest
        & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
            -ManifestPath $stagingManifest `
            -WebsiteRoot $WebsiteRoot `
            -SkipNativeState
        & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
            -ManifestPath $ManifestPath `
            -WebsiteRoot $WebsiteRoot

        Invoke-SnapshotTransaction `
            -WebsiteRoot $WebsiteRoot `
            -Changes @(
                [PSCustomObject]@{
                    Target = "versioned_docs/version-$Version"
                    Source = $stagingDocs
                },
                [PSCustomObject]@{
                    Target = "versioned_sidebars/version-$Version-sidebars.json"
                    Source = $stagingSidebar
                },
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $stagingVersions
                },
                [PSCustomObject]@{
                    Target = "humanizer-versions.json"
                    Source = $stagingManifest
                }
            ) `
            -Validate {
                & (Join-Path $PSScriptRoot "verify-manifest.ps1") `
                    -ManifestPath $ManifestPath `
                    -WebsiteRoot $WebsiteRoot
            }
        Write-Host "Created immutable documentation snapshot: $Version"
    } finally {
        Remove-Item $stagingRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
} finally {
    if ($null -ne $mutationLock) {
        Exit-SnapshotMutation -Lock $mutationLock
    }
}
