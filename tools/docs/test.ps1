param(
    [switch]$RequireNativeAot,
    [string]$ReleaseVersion
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
$manifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
. (Join-Path $PSScriptRoot "snapshot-state.ps1")
. (Join-Path $PSScriptRoot "api-approval.ps1")
. (Join-Path $PSScriptRoot "nuget-package.ps1")

function Assert-VersionedExampleDefaults {
    param(
        [Parameter(Mandatory = $true)]$Entry,
        [Parameter(Mandatory = $true)][string]$ExamplesRoot
    )

    [xml]$props = Get-Content -Raw (
        Join-Path $ExamplesRoot "Directory.Build.props"
    )
    $groups = @(
        $props.SelectNodes(
            "/Project/PropertyGroup[@Label='HumanizerDocumentationSnapshot']"
        )
    )
    if ($groups.Count -ne 1) {
        throw "Version $($Entry.version) must declare one example-default group."
    }

    $versionNodes = @($groups[0].SelectNodes("HumanizerPackageVersion"))
    $expectedVersionCondition =
        "'`$(HumanizerProject)' == '' and " +
        "'`$(HumanizerPackageVersion)' == ''"
    if ($versionNodes.Count -ne 1 -or
        $versionNodes[0].InnerText -ne $Entry.source.packageVersion -or
        $versionNodes[0].GetAttribute("Condition") -ne
            $expectedVersionCondition) {
        throw "Version $($Entry.version) does not default to its exact package."
    }

    $excludedAssetsProperty = $Entry.PSObject.Properties[
        "exampleExcludedAssets"
    ]
    $expectedExcludedAssets = if ($null -eq $excludedAssetsProperty) {
        ""
    } else {
        @($excludedAssetsProperty.Value) -join ";"
    }
    $excludedAssetNodes = @(
        $groups[0].SelectNodes("HumanizerExampleExcludeAssets")
    )
    if (($expectedExcludedAssets -eq "" -and
            $excludedAssetNodes.Count -ne 0) -or
        ($expectedExcludedAssets -ne "" -and
            ($excludedAssetNodes.Count -ne 1 -or
                $excludedAssetNodes[0].InnerText -ne
                    $expectedExcludedAssets -or
                $excludedAssetNodes[0].GetAttribute("Condition") -ne
                    ("'`$(HumanizerProject)' == '' and " +
                        "'`$(HumanizerPackageVersion)' == " +
                        "'$($Entry.source.packageVersion)' and " +
                        "'`$(HumanizerExampleExcludeAssets)' == ''")))) {
        throw "Version $($Entry.version) has incorrect example asset defaults."
    }
}

$fingerprintTempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-fingerprint-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $fingerprintTempRoot | Out-Null
try {
    $staleReferencePath = Join-Path $fingerprintTempRoot "locale-yaml-reference.mdx"
    $referenceText = Get-Content -Raw (
        Join-Path $repoRoot "website/docs/contributing/locale-yaml-reference.mdx"
    )
    $staleReferenceText = [regex]::Replace(
        $referenceText,
        "(?m)^(src/Humanizer\.SourceGenerators/Common/CanonicalLocaleAuthoring\.cs )[0-9a-f]{64}$",
        ('${1}' + ('0' * 64))
    )
    [System.IO.File]::WriteAllText(
        $staleReferencePath,
        $staleReferenceText,
        [System.Text.UTF8Encoding]::new($false)
    )
    $fingerprintOutput = & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "generate-language-coverage.ps1"
    ) `
        -Check `
        -ReferencePath $staleReferencePath `
        -ProjectPath (Join-Path $fingerprintTempRoot "must-not-build.csproj") 2>&1 |
        Out-String
    if ($LASTEXITCODE -eq 0) {
        throw "Stale locale reference fingerprint unexpectedly passed."
    }
    if ($fingerprintOutput -notmatch "CanonicalLocaleAuthoring\.cs" -or
        $fingerprintOutput -notmatch "Re-review") {
        throw "Stale locale reference fingerprint did not fail with the required review guidance."
    }
} finally {
    Remove-Item $fingerprintTempRoot -Recurse -Force
}

& (Join-Path $PSScriptRoot "verify-manifest.ps1")
$fixtureManifest = Get-Content -Raw $manifestPath |
    ConvertFrom-Json -Depth 20
$fixtureLatest = @($fixtureManifest.versions | Where-Object latestStable)
$candidateVersion = if ([string]::IsNullOrWhiteSpace($ReleaseVersion)) {
    if ($fixtureLatest.Count -ne 1) {
        throw "The release fixture requires exactly one latest stable version."
    }
    [string]$fixtureLatest[0].version
} else {
    $ReleaseVersion
}
$candidateEntries = @(
    $fixtureManifest.versions |
        Where-Object version -eq $candidateVersion
)
$priorPublishedVersions = @(
    $fixtureManifest.versions |
        Where-Object {
            $_.version -ne "current" -and
            $_.published -and
            $_.version -ne $candidateVersion
        } |
        Sort-Object {
            [System.Management.Automation.SemanticVersion]::Parse(
                $_.version
            )
        } -Descending |
        ForEach-Object version
)
if ($candidateEntries.Count -ne 1 -or
    -not $candidateEntries[0].published -or
    $priorPublishedVersions.Count -eq 0) {
    throw "The release fixture requires one published candidate and at least one prior published version."
}
$candidateEntry = $candidateEntries[0]
$candidateSemanticVersion =
    [System.Management.Automation.SemanticVersion]::Parse($candidateVersion)
$previousLatestEntry = if ($candidateEntry.latestStable) {
    @(
        $fixtureManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.published -and
                $_.version -ne $candidateVersion -and
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                ) -lt $candidateSemanticVersion
            } |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending
    )[0]
} else {
    $fixtureLatest[0]
}
if ($null -eq $previousLatestEntry) {
    throw "The release fixture requires a default predecessor."
}
$previousLatestVersion = [string]$previousLatestEntry.version
$canPromoteCandidate =
    $candidateSemanticVersion -gt
        [System.Management.Automation.SemanticVersion]::Parse(
            $previousLatestVersion
        )

foreach ($publishedEntry in @(
    $fixtureManifest.versions |
        Where-Object { $_.version -ne "current" -and $_.published }
)) {
    Assert-VersionedExampleDefaults `
        -Entry $publishedEntry `
        -ExamplesRoot (Join-Path $repoRoot (
            "website/versioned_docs/version-$($publishedEntry.version)/_examples"
        ))
}

& (Join-Path $PSScriptRoot "build.ps1") `
    -Version $candidateVersion `
    -ValidateOnly
$aotParameters = @{
    RequireNativeAot = $RequireNativeAot
}
if (-not [string]::IsNullOrWhiteSpace($ReleaseVersion)) {
    $aotParameters.Version = @($ReleaseVersion, "current")
}
& (Join-Path $PSScriptRoot "verify-aot.ps1") @aotParameters

$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $tempRoot | Out-Null
try {
    $currentRestoreOutput = & dotnet restore (
        Join-Path $repoRoot "website/docs/_examples/quick-start/QuickStart.csproj"
    ) `
        --artifacts-path (Join-Path $tempRoot "current-without-input") `
        --nologo 2>&1 | Out-String
    if ($LASTEXITCODE -eq 0 -or
        $currentRestoreOutput -notmatch
            "Set HumanizerProject or HumanizerPackageVersion") {
        throw "Canonical current examples did not require an explicit input."
    }

    $invalidManifestPath = Join-Path $tempRoot "invalid-manifest.json"
    '{"schemaVersion":1,"versions":[]}' | Set-Content $invalidManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "build.ps1") -ValidateOnly -ManifestPath $invalidManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Empty manifest validation unexpectedly passed."
    }

    $duplicateRouteManifestPath = Join-Path $tempRoot "duplicate-route-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    ($manifest.versions | Where-Object version -eq "2.10.1").route = "2.11.10"
    $manifest | ConvertTo-Json -Depth 20 | Set-Content $duplicateRouteManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $duplicateRouteManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Duplicate route validation unexpectedly passed."
    }

    $missingPreviewManifestPath = Join-Path $tempRoot "missing-preview-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    $manifest.versions = @(
        $manifest.versions |
            Where-Object version -ne "current"
    )
    $manifest | ConvertTo-Json -Depth 20 |
        Set-Content $missingPreviewManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-manifest.ps1") `
        -ManifestPath $missingPreviewManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Missing preview manifest validation unexpectedly passed."
    }

    $missingTfmManifestPath = Join-Path $tempRoot "missing-tfm-manifest.json"
    $manifest = Get-Content -Raw $manifestPath | ConvertFrom-Json -Depth 20
    ($manifest.versions | Where-Object version -eq "2.10.1").referenceTfm = "net-missing"
    $manifest | ConvertTo-Json -Depth 20 | Set-Content $missingTfmManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-api.ps1") `
        -Version "2.10.1" `
        -Smoke `
        -ManifestPath $missingTfmManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Missing DLL/XML input validation unexpectedly passed."
    }

    $badExampleRoot = Join-Path $tempRoot "bad-example"
    Copy-Item `
        (Join-Path $repoRoot (
            "website/versioned_docs/version-$previousLatestVersion/_examples/quick-start"
        )) `
        $badExampleRoot `
        -Recurse
    foreach ($buildFile in @(
        "Directory.Build.props",
        "Directory.Build.targets"
    )) {
        Copy-Item `
            (Join-Path $repoRoot (
                "website/versioned_docs/version-$previousLatestVersion/_examples/$buildFile"
            )) `
            $badExampleRoot
    }
    $badExampleTargets = Join-Path $badExampleRoot "Directory.Build.targets"
    $badExampleText = (Get-Content -Raw $badExampleTargets).Replace(
        'Version="$(HumanizerPackageVersion)"',
        "Version=`"$candidateVersion`""
    )
    [System.IO.File]::WriteAllText(
        $badExampleTargets,
        $badExampleText,
        [System.Text.UTF8Encoding]::new($false)
    )
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version $previousLatestVersion `
        -ExamplesRoot $badExampleRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "An example restored against the wrong package version."
    }

    $mixedPackageGraphFailed = $false
    try {
        Assert-DocsHumanizerPackageGraph `
            -Entry $candidateEntry `
            -Libraries ([PSCustomObject]@{
                "Humanizer/$candidateVersion" =
                    [PSCustomObject]@{ type = "package" }
                "Humanizer.Core/$previousLatestVersion" =
                    [PSCustomObject]@{ type = "package" }
            }) `
            -Context "Synthetic example"
    } catch {
        $mixedPackageGraphFailed = $true
    }
    if (-not $mixedPackageGraphFailed) {
        throw "A mixed Humanizer package graph unexpectedly passed."
    }

    $extractPaths = [System.Collections.Generic.List[string]]::new()
    $extractedDllHashes = [System.Collections.Generic.List[string]]::new()
    foreach ($iteration in 1..2) {
        Use-DocsNuGetPackage `
            -Entry $candidateEntry `
            -RepoRoot $repoRoot `
            -Action {
            param($package)

            [void]$extractPaths.Add($package.ExtractPath)
            $dllPath = Join-Path (
                $package.ExtractPath
            ) "lib/$($candidateEntry.referenceTfm)/Humanizer.dll"
            [void]$extractedDllHashes.Add(
                (Get-FileHash $dllPath -Algorithm SHA256).Hash
            )
            if ($iteration -eq 1) {
                Set-Content $dllPath "tampered extraction"
            }
        }
        if (Test-Path $extractPaths[$extractPaths.Count - 1]) {
            throw "A verified package extraction was not removed after use."
        }
    }
    if ($extractPaths[0] -eq $extractPaths[1] -or
        $extractedDllHashes[0] -ne $extractedDllHashes[1]) {
        throw "Verified package extraction was reused instead of recreated."
    }

    $expectedExtras = @(
        "Humanizer.ICulturedStringTransformer",
        "Humanizer.Localisation.DataUnit",
        "Humanizer.MetricNumeralFormats"
    )
    $invalidExtraSets = @(
        [PSCustomObject]@{
            Name = "missing declaration"
            Actual = $expectedExtras
            Expected = $expectedExtras[0..1]
        },
        [PSCustomObject]@{
            Name = "unexpected generated type"
            Actual = @($expectedExtras + "Humanizer.Unexpected")
            Expected = $expectedExtras
        },
        [PSCustomObject]@{
            Name = "duplicate exception"
            Actual = $expectedExtras
            Expected = @($expectedExtras + $expectedExtras[0])
        },
        [PSCustomObject]@{
            Name = "now-approved unused exception"
            Actual = $expectedExtras[0..1]
            Expected = $expectedExtras
        }
    )
    foreach ($case in $invalidExtraSets) {
        $failed = $false
        try {
            Assert-GeneratedExtraTypes `
                -VersionLabel "2.10.1" `
                -Actual $case.Actual `
                -Expected $case.Expected
        } catch {
            $failed = $true
        }
        if (-not $failed) {
            throw "PublicAPI extra-type $($case.Name) unexpectedly passed."
        }
    }

    $memberCoverageRoot = Join-Path $tempRoot "api-member-coverage"
    Copy-Item `
        (Join-Path $repoRoot "website/docs/api") `
        $memberCoverageRoot `
        -Recurse
    $defaultFormatterPath = Join-Path (
        $memberCoverageRoot
    ) "Humanizer.DefaultFormatter.md"
    $memberLinksPath = Join-Path $tempRoot "api-member-links.txt"
    $publicLinksPath = Join-Path $tempRoot "public-api-links.txt"
    $protectedApprovalPath = Join-Path $tempRoot "protected-api-approval.txt"
    [System.IO.File]::WriteAllText(
        $memberLinksPath,
        "./`nT:Humanizer.DefaultFormatter|Humanizer.DefaultFormatter.md|DefaultFormatter`nP:Humanizer.DefaultFormatter.Culture|Humanizer.DefaultFormatter.md#Humanizer.DefaultFormatter.Culture|Culture`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $publicLinksPath,
        "./`nT:Humanizer.DefaultFormatter|Humanizer.DefaultFormatter.md|DefaultFormatter`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $protectedApprovalPath,
        "        protected System.Globalization.CultureInfo Culture { get; }`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    Assert-GeneratedApiLinks `
        -OutputPath $memberCoverageRoot `
        -LinksPath $memberLinksPath `
        -VersionLabel "current baseline"
    Assert-ApiAccessCoverage `
        -ApiLinksPath $memberLinksPath `
        -PublicLinksPath $publicLinksPath `
        -ApprovalPath $protectedApprovalPath `
        -VersionLabel "current baseline"
    $publicOnlyFailed = $false
    try {
        Assert-ApiAccessCoverage `
            -ApiLinksPath $publicLinksPath `
            -PublicLinksPath $publicLinksPath `
            -ApprovalPath $protectedApprovalPath `
            -VersionLabel "public-only"
    } catch {
        $publicOnlyFailed = $true
    }
    if (-not $publicOnlyFailed) {
        throw "Public-only API generation unexpectedly passed access coverage."
    }
    $defaultFormatter = Get-Content -Raw $defaultFormatterPath
    $cultureAnchor = "<a name='Humanizer.DefaultFormatter.Culture'></a>`n"
    $withoutCulture = $defaultFormatter.Replace(
        $cultureAnchor,
        ""
    )
    if ($withoutCulture -eq $defaultFormatter) {
        throw "The API member-coverage fixture is missing the Culture anchor."
    }
    [System.IO.File]::WriteAllText(
        $defaultFormatterPath,
        $withoutCulture,
        [System.Text.UTF8Encoding]::new($false)
    )
    $memberCoverageFailed = $false
    try {
        Assert-GeneratedApiLinks `
            -OutputPath $memberCoverageRoot `
            -LinksPath $memberLinksPath `
            -VersionLabel "current"
    } catch {
        $memberCoverageFailed = $true
    }
    if (-not $memberCoverageFailed) {
        throw "An omitted non-representative API member unexpectedly passed."
    }

    $preJournalRoot = Join-Path $tempRoot "pre-journal-website"
    New-Item -ItemType Directory -Path $preJournalRoot | Out-Null
    $preJournalFailed = $false
    try {
        Invoke-SnapshotTransaction `
            -WebsiteRoot $preJournalRoot `
            -Changes @(
                [PSCustomObject]@{
                    Target = "not-an-allowed-target"
                    Source = (Join-Path $tempRoot "missing-source")
                }
            )
    } catch {
        $preJournalFailed = $true
    }
    if (-not $preJournalFailed -or
        (Test-Path (Join-Path $preJournalRoot ".snapshot-transaction"))) {
        throw "Pre-journal snapshot failure left stale transaction state."
    }

    $journalInvariantRoot = Join-Path $tempRoot "journal-invariant-website"
    New-Item -ItemType Directory -Path $journalInvariantRoot | Out-Null
    $journalInvariantVersions = Join-Path $journalInvariantRoot "versions.json"
    $journalInvariantManifest = Join-Path (
        $journalInvariantRoot
    ) "humanizer-versions.json"
    $journalSourceOne = Join-Path $tempRoot "journal-source-one.json"
    $journalSourceTwo = Join-Path $tempRoot "journal-source-two.json"
    [System.IO.File]::WriteAllText(
        $journalInvariantVersions,
        "[`"$candidateVersion`"]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalInvariantManifest,
        "{`"schemaVersion`":1}`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalSourceOne,
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $journalSourceTwo,
        "{} `n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $journalInvariantVersionsHash = (
        Get-FileHash $journalInvariantVersions -Algorithm SHA256
    ).Hash
    $journalInvariantManifestHash = (
        Get-FileHash $journalInvariantManifest -Algorithm SHA256
    ).Hash
    foreach ($case in @(
        [PSCustomObject]@{
            Name = "duplicate target"
            ExpectedError = "unique targets"
            Changes = @(
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceTwo
                }
            )
        },
        [PSCustomObject]@{
            Name = "manifest before another target"
            ExpectedError = "manifest target must be last"
            Changes = @(
                [PSCustomObject]@{
                    Target = "humanizer-versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceTwo
                }
            )
        }
    )) {
        $journalInvariantFailed = $false
        try {
            Invoke-SnapshotTransaction `
                -WebsiteRoot $journalInvariantRoot `
                -Changes $case.Changes
        } catch {
            $journalInvariantFailed =
                $_.Exception.Message -match $case.ExpectedError
        }
        if (-not $journalInvariantFailed -or
            (Get-FileHash $journalInvariantVersions -Algorithm SHA256).Hash -ne
                $journalInvariantVersionsHash -or
            (Get-FileHash $journalInvariantManifest -Algorithm SHA256).Hash -ne
                $journalInvariantManifestHash -or
            (Test-Path (
                Join-Path $journalInvariantRoot ".snapshot-transaction"
            ))) {
            throw "Pre-journal $($case.Name) rejection was not fail-closed."
        }
    }

    $validationRollbackFailed = $false
    try {
        Invoke-SnapshotTransaction `
            -WebsiteRoot $journalInvariantRoot `
            -Changes @(
                [PSCustomObject]@{
                    Target = "versions.json"
                    Source = $journalSourceOne
                },
                [PSCustomObject]@{
                    Target = "humanizer-versions.json"
                    Source = $journalSourceTwo
                }
            ) `
            -Validate {
                throw "Synthetic post-install validation failure."
            }
    } catch {
        $validationRollbackFailed =
            $_.Exception.Message -match
                "Synthetic post-install validation failure"
    }
    if (-not $validationRollbackFailed -or
        (Get-FileHash $journalInvariantVersions -Algorithm SHA256).Hash -ne
            $journalInvariantVersionsHash -or
        (Get-FileHash $journalInvariantManifest -Algorithm SHA256).Hash -ne
            $journalInvariantManifestHash -or
        (Test-Path (
            Join-Path $journalInvariantRoot ".snapshot-transaction"
        ))) {
        throw "Post-install validation failure was not rolled back atomically."
    }

    $snapshotPath = Join-Path $repoRoot (
        "website/versioned_docs/version-$candidateVersion"
    )
    $sidebarsPath = Join-Path $repoRoot (
        "website/versioned_sidebars/version-$candidateVersion-sidebars.json"
    )
    $versionsPath = Join-Path $repoRoot "website/versions.json"
    $snapshotDigest = Get-SnapshotDirectoryDigest $snapshotPath
    $sidebarsHash = (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash
    $versionsHash = (Get-FileHash $versionsPath -Algorithm SHA256).Hash

    foreach ($rejectedArguments in @(
        @("-Version", $candidateVersion),
        @("-Version", $candidateVersion, "-CorrectPage", "api/Humanizer.StringHumanizeExtensions.md"),
        @("-All")
    )) {
        & pwsh -NoProfile -File (
            Join-Path $PSScriptRoot "snapshot.ps1"
        ) @rejectedArguments *> $null
        if ($LASTEXITCODE -eq 0) {
            throw "Rejected snapshot operation unexpectedly passed: $($rejectedArguments -join ' ')"
        }
    }
    if ((Get-SnapshotDirectoryDigest $snapshotPath) -ne $snapshotDigest -or
        (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash -ne $sidebarsHash -or
        (Get-FileHash $versionsPath -Algorithm SHA256).Hash -ne $versionsHash) {
        throw "A rejected snapshot operation changed immutable output."
    }

    $isolatedWebsiteRoot = Join-Path $tempRoot "website"
    New-Item -ItemType Directory -Path $isolatedWebsiteRoot | Out-Null
    foreach ($path in @(
        "docs",
        "version-overrides",
        "versioned_docs",
        "versioned_sidebars"
    )) {
        Copy-Item `
            (Join-Path $repoRoot "website/$path") `
            $isolatedWebsiteRoot `
            -Recurse
    }
    foreach ($path in @(
        "humanizer-versions.json",
        "scenario-api-contract.json",
        "sidebars.json",
        "versions.json"
    )) {
        Copy-Item `
            (Join-Path $repoRoot "website/$path") `
            $isolatedWebsiteRoot
    }
    $isolatedManifestPath = Join-Path (
        $isolatedWebsiteRoot
    ) "humanizer-versions.json"

    $global:HumanizerSnapshotLockProbeRoot = $isolatedWebsiteRoot
    $global:HumanizerSnapshotLockProbeResult = $null
    $lockBreakpoint = Set-PSBreakpoint `
        -Command Assert-FrozenSnapshot `
        -Action {
            $lockPath = Join-Path (
                $global:HumanizerSnapshotLockProbeRoot
            ) ".snapshot-mutation.lock"
            try {
                $probe = [System.IO.File]::Open(
                    $lockPath,
                    [System.IO.FileMode]::OpenOrCreate,
                    [System.IO.FileAccess]::ReadWrite,
                    [System.IO.FileShare]::None
                )
                $probe.Dispose()
                $global:HumanizerSnapshotLockProbeResult = "released"
            } catch [System.IO.IOException] {
                $global:HumanizerSnapshotLockProbeResult = "held"
            } catch {
                $global:HumanizerSnapshotLockProbeResult =
                    "error:$($_.Exception.GetType().FullName)"
            }
        }
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
        $lockProbeResult = $global:HumanizerSnapshotLockProbeResult
    } finally {
        Remove-PSBreakpoint $lockBreakpoint
        Remove-Variable `
            HumanizerSnapshotLockProbeRoot `
            -Scope Global `
            -ErrorAction SilentlyContinue
        Remove-Variable `
            HumanizerSnapshotLockProbeResult `
            -Scope Global `
            -ErrorAction SilentlyContinue
    }
    if ($lockProbeResult -ne "held") {
        throw "Snapshot check did not retain the global lock: $lockProbeResult"
    }

    $isolatedFixtureManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    $identicalOverlayFixture = @(
        foreach ($entry in $isolatedFixtureManifest.versions) {
            if ([string]::IsNullOrWhiteSpace(
                $entry.compatibilityOverlay
            )) {
                continue
            }
            $overlayRoot = Join-Path (
                $isolatedWebsiteRoot
            ) $entry.compatibilityOverlay
            $overlay = Get-Content -Raw (
                Join-Path $overlayRoot "overlay.json"
            ) | ConvertFrom-Json -Depth 20
            foreach ($path in @($overlay.replacements)) {
                if (Test-Path (
                    Join-Path $isolatedWebsiteRoot "docs/$path"
                ) -PathType Leaf) {
                    [PSCustomObject]@{
                        Entry = $entry
                        Path = $path
                        Root = $overlayRoot
                    }
                }
            }
        }
    )[0]
    if ($null -eq $identicalOverlayFixture) {
        throw "The documentation fixture requires one authored overlay replacement."
    }
    $identicalOverlayRelativePath = $identicalOverlayFixture.Path
    $identicalOverlayPath = Join-Path (
        $identicalOverlayFixture.Root
    ) $identicalOverlayRelativePath
    Copy-Item (
        Join-Path $isolatedWebsiteRoot "docs/$identicalOverlayRelativePath"
    ) $identicalOverlayPath -Force
    & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "verify-manifest.ps1"
    ) `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "A byte-identical overlay unexpectedly passed."
    }
    Copy-Item (
        Join-Path (
            Join-Path $repoRoot (
                "website/$($identicalOverlayFixture.Entry.compatibilityOverlay)"
            )
        ) $identicalOverlayRelativePath
    ) $identicalOverlayPath -Force

    $oldestPublishedVersion = $priorPublishedVersions[-1]
    $deletedPublishedPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_docs/version-$oldestPublishedVersion/start/overview.mdx"
    Remove-Item $deletedPublishedPath -Force
    & pwsh -NoProfile -File (
        Join-Path $PSScriptRoot "snapshot.ps1"
    ) `
        -All `
        -Check `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "A deleted published snapshot file unexpectedly passed."
    }
    Copy-Item (
        Join-Path $repoRoot (
            "website/versioned_docs/version-$oldestPublishedVersion/start/overview.mdx"
        )
    ) $deletedPublishedPath

    $transactionRoot = Join-Path $isolatedWebsiteRoot ".snapshot-transaction"
    New-Item -ItemType Directory `
        -Path (Join-Path $transactionRoot "backups") `
        -Force | Out-Null
    $versionsBeforeDigest = (Get-FileHash (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) -Algorithm SHA256).Hash
    Copy-Item (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) (Join-Path $transactionRoot "backups/0")
    [System.IO.File]::WriteAllText(
        (Join-Path $isolatedWebsiteRoot "versions.json"),
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $versionsAfterDigest = (Get-FileHash (
        Join-Path $isolatedWebsiteRoot "versions.json"
    ) -Algorithm SHA256).Hash
    Write-SnapshotJson `
        -Value ([PSCustomObject]@{
            schemaVersion = 1
            state = "applying"
            targets = @(
                [PSCustomObject]@{
                    index = 0
                    target = "versions.json"
                    staged = "staged/0"
                    backup = "backups/0"
                    hadOriginal = $true
                    backupReady = $true
                    installing = $true
                    installed = $false
                    beforeDigest = $versionsBeforeDigest
                    afterDigest = $versionsAfterDigest
                }
            )
        }) `
        -Path (Join-Path $transactionRoot "journal.json")
    [System.IO.File]::WriteAllText(
        (Join-Path $isolatedWebsiteRoot ".snapshot-mutation.lock"),
        '{"pid":-1,"startedUtc":"stale"}',
        [System.Text.UTF8Encoding]::new($false)
    )
    $recoveryFailedForExpectedReason = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version "not-declared" `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $recoveryFailedForExpectedReason =
            $_.Exception.Message -match "not declared exactly once"
    }
    if (-not $recoveryFailedForExpectedReason -or
        (Get-Content -Raw (Join-Path $isolatedWebsiteRoot "versions.json")) -eq
            "[]`n" -or
        (Test-Path $transactionRoot)) {
        throw "A stale snapshot transaction was not recovered before validation."
    }

    New-Item -ItemType Directory `
        -Path (Join-Path $transactionRoot "backups") `
        -Force | Out-Null
    $restartableVersionsPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versions.json"
    $restartableBeforeDigest = (
        Get-FileHash $restartableVersionsPath -Algorithm SHA256
    ).Hash
    Copy-Item `
        $restartableVersionsPath `
        (Join-Path $transactionRoot "backups/0")
    $restartableAfterSource = Join-Path (
        $tempRoot
    ) "restartable-after-versions.json"
    $restartableAbsentSource = Join-Path (
        $tempRoot
    ) "restartable-after-sidebar.json"
    [System.IO.File]::WriteAllText(
        $restartableAfterSource,
        "[]`n",
        [System.Text.UTF8Encoding]::new($false)
    )
    [System.IO.File]::WriteAllText(
        $restartableAbsentSource,
        "{} `n",
        [System.Text.UTF8Encoding]::new($false)
    )
    $restartableAbsentTarget = (
        "versioned_sidebars/version-9.9.9-sidebars.json"
    )
    Write-SnapshotJson `
        -Value ([PSCustomObject]@{
            schemaVersion = 1
            state = "applying"
            targets = @(
                [PSCustomObject]@{
                    index = 0
                    target = "versions.json"
                    staged = "staged/0"
                    backup = "backups/0"
                    hadOriginal = $true
                    backupReady = $true
                    installing = $false
                    installed = $true
                    beforeDigest = $restartableBeforeDigest
                    afterDigest = (
                        Get-FileHash `
                            $restartableAfterSource `
                            -Algorithm SHA256
                    ).Hash
                },
                [PSCustomObject]@{
                    index = 1
                    target = $restartableAbsentTarget
                    staged = "staged/1"
                    backup = "backups/1"
                    hadOriginal = $false
                    backupReady = $false
                    installing = $false
                    installed = $true
                    beforeDigest = $null
                    afterDigest = (
                        Get-FileHash `
                            $restartableAbsentSource `
                            -Algorithm SHA256
                    ).Hash
                }
            )
        }) `
        -Path (Join-Path $transactionRoot "journal.json")
    $restartableLock = Enter-SnapshotMutation `
        -WebsiteRoot $isolatedWebsiteRoot
    try {
        if ((Get-FileHash (
                Join-Path $isolatedWebsiteRoot "versions.json"
            ) -Algorithm SHA256).Hash -ne $restartableBeforeDigest -or
            (Test-Path (
                Join-Path $isolatedWebsiteRoot $restartableAbsentTarget
            )) -or
            (Test-Path $transactionRoot)) {
            throw "Restarted rollback did not preserve the restored state."
        }
    } finally {
        Exit-SnapshotMutation -Lock $restartableLock
    }

    $isolatedCurrentApi = Join-Path $isolatedWebsiteRoot "docs/api"
    $currentApiDigest = Get-SnapshotDirectoryDigest $isolatedCurrentApi
    Add-Content (
        Join-Path $isolatedCurrentApi "index.md"
    ) "`n<!-- tampered current API -->"
    $currentCheckFailed = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version current `
            -Check `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $currentCheckFailed = $true
    }
    if (-not $currentCheckFailed) {
        throw "A tampered current API unexpectedly passed."
    }
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version current `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    if ((Get-SnapshotDirectoryDigest $isolatedCurrentApi) -ne
        $currentApiDigest) {
        throw "Current API refresh did not restore the deterministic tree."
    }
    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version current `
        -Check `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null

    $isolatedManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    $isolatedCandidateEntries = @(
        $isolatedManifest.versions |
            Where-Object version -eq $candidateVersion
    )
    $priorVersions = @(
        $isolatedManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.published -and
                $_.version -ne $candidateVersion
            } |
            Sort-Object {
                [System.Management.Automation.SemanticVersion]::Parse(
                    $_.version
                )
            } -Descending |
            ForEach-Object version
    )
    if ($isolatedCandidateEntries.Count -ne 1 -or
        -not $isolatedCandidateEntries[0].published -or
        $priorVersions.Count -eq 0 -or
        $previousLatestVersion -notin $priorVersions) {
        throw "The isolated release fixture has invalid publication state."
    }
    $priorState = @{}
    foreach ($priorVersion in $priorVersions) {
        $priorEntry = @(
            $isolatedManifest.versions |
                Where-Object version -eq $priorVersion
        )[0]
        $priorState[$priorVersion] = [PSCustomObject]@{
            ManifestSnapshot = $priorEntry.immutability.snapshotSha256
            ManifestSidebar = $priorEntry.immutability.sidebarSha256
            LiveSnapshot = Get-SnapshotDirectoryDigest (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_docs/version-$priorVersion"
                )
            )
            LiveSidebar = (Get-FileHash (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_sidebars/version-$priorVersion-sidebars.json"
                )
            ) -Algorithm SHA256).Hash
        }
    }
    $releaseSnapshotPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_docs/version-$candidateVersion"
    $releaseSidebarPath = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_sidebars/version-$candidateVersion-sidebars.json"
    $releaseSnapshotDigest = Get-SnapshotDirectoryDigest $releaseSnapshotPath
    $releaseSidebarHash = (
        Get-FileHash $releaseSidebarPath -Algorithm SHA256
    ).Hash
    $canonicalDocsRoot = Join-Path $tempRoot "canonical-docs"
    Copy-Item `
        (Join-Path $isolatedWebsiteRoot "docs") `
        $canonicalDocsRoot `
        -Recurse
    Remove-Item (
        Join-Path $isolatedWebsiteRoot "docs"
    ) -Recurse -Force
    Copy-Item $releaseSnapshotPath (
        Join-Path $isolatedWebsiteRoot "docs"
    ) -Recurse
    Copy-Item (
        Join-Path $canonicalDocsRoot "_examples/Directory.Build.props"
    ) (
        Join-Path $isolatedWebsiteRoot "docs/_examples/Directory.Build.props"
    ) -Force
    Remove-Item (
        Join-Path $isolatedWebsiteRoot "docs/api"
    ) -Recurse -Force
    Copy-Item $releaseSidebarPath (
        Join-Path $isolatedWebsiteRoot "sidebars.json"
    ) -Force
    $releaseOverlayRoot = Join-Path (
        $isolatedWebsiteRoot
    ) $isolatedCandidateEntries[0].compatibilityOverlay
    $releaseOverlay = Get-Content -Raw (
        Join-Path $releaseOverlayRoot "overlay.json"
    ) | ConvertFrom-Json -Depth 20
    foreach ($path in @(
        $releaseOverlay.replacements + $releaseOverlay.exclusions
    )) {
        $destination = Join-Path (
            Join-Path $isolatedWebsiteRoot "docs"
        ) $path
        New-Item -ItemType Directory `
            -Path (Split-Path $destination -Parent) `
            -Force | Out-Null
        Copy-Item (Join-Path $canonicalDocsRoot $path) $destination -Force
    }
    foreach ($priorEntry in (
        $isolatedManifest.versions |
            Where-Object {
                $_.version -ne "current" -and
                $_.version -ne $candidateVersion
            }
    )) {
        $priorEntry.latestStable = $false
        $priorEntry.route = $priorEntry.version
        $priorEntry.label = $priorEntry.version
    }
    $previousLatest = @(
        $isolatedManifest.versions |
            Where-Object version -eq $previousLatestVersion
    )[0]
    $previousLatest.latestStable = $true
    $previousLatest.route = ""
    $previousLatest.label = "$previousLatestVersion (latest)"
    $future = @(
        $isolatedManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0]
    $future.published = $false
    $future.latestStable = $false
    $future.route = $candidateVersion
    $future.label = $candidateVersion
    $future.PSObject.Properties.Remove("immutability")
    Write-SnapshotJson -Value $isolatedManifest -Path $isolatedManifestPath
    Write-SnapshotJson `
        -Value $priorVersions `
        -Path (Join-Path $isolatedWebsiteRoot "versions.json") `
        -AsArray
    Remove-Item (
        Join-Path $isolatedWebsiteRoot (
            "versioned_docs/version-$candidateVersion"
        )
    ) -Recurse -Force
    Remove-Item (
        Join-Path $isolatedWebsiteRoot (
            "versioned_sidebars/version-$candidateVersion-sidebars.json"
        )
    ) -Force

    $assertReleaseRoundTrip = {
        param(
            [Parameter(Mandatory = $true)]$ReleasedManifest,
            [Parameter(Mandatory = $true)][string]$Mode
        )

        $releasedCandidates = @(
            $ReleasedManifest.versions |
                Where-Object version -eq $candidateVersion
        )
        if ($releasedCandidates.Count -ne 1) {
            throw "$Mode release did not retain exactly one candidate."
        }
        $releasedCandidate = $releasedCandidates[0]
        Assert-VersionedExampleDefaults `
            -Entry $releasedCandidate `
            -ExamplesRoot (Join-Path $isolatedWebsiteRoot (
                "versioned_docs/version-$candidateVersion/_examples"
            ))
        if ($releasedCandidate.immutability.snapshotSha256 -ne
                $releaseSnapshotDigest -or
            $releasedCandidate.immutability.sidebarSha256 -ne
                $releaseSidebarHash -or
            (Get-SnapshotDirectoryDigest (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_docs/version-$candidateVersion"
                )
            )) -ne $releaseSnapshotDigest -or
            (Get-FileHash (
                Join-Path $isolatedWebsiteRoot (
                    "versioned_sidebars/version-$candidateVersion-sidebars.json"
                )
            ) -Algorithm SHA256).Hash -ne $releaseSidebarHash) {
            throw "$Mode release round trip changed immutable $candidateVersion state."
        }
        foreach ($priorVersion in $priorVersions) {
            $releasedPrior = @(
                $ReleasedManifest.versions |
                    Where-Object version -eq $priorVersion
            )[0]
            $before = $priorState[$priorVersion]
            if ($releasedPrior.immutability.snapshotSha256 -ne
                    $before.ManifestSnapshot -or
                $releasedPrior.immutability.sidebarSha256 -ne
                    $before.ManifestSidebar -or
                (Get-SnapshotDirectoryDigest (
                    Join-Path $isolatedWebsiteRoot (
                        "versioned_docs/version-$priorVersion"
                    )
                )) -ne $before.LiveSnapshot -or
                (Get-FileHash (
                    Join-Path $isolatedWebsiteRoot (
                        "versioned_sidebars/version-$priorVersion-sidebars.json"
                    )
                ) -Algorithm SHA256).Hash -ne $before.LiveSidebar) {
                throw "$Mode release changed prior immutable state: $priorVersion"
            }
        }
    }

    & (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version $candidateVersion `
        -ManifestPath $isolatedManifestPath `
        -WebsiteRoot $isolatedWebsiteRoot *> $null
    $nonDefaultManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    & $assertReleaseRoundTrip `
        -ReleasedManifest $nonDefaultManifest `
        -Mode "Non-default"
    $nonDefaultCandidate = @(
        $nonDefaultManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0]
    $nonDefaultLatest = @(
        $nonDefaultManifest.versions |
            Where-Object version -eq $previousLatestVersion
    )[0]
    if (-not $nonDefaultCandidate.published -or
        $nonDefaultCandidate.latestStable -or
        $nonDefaultCandidate.route -ne $candidateVersion -or
        $nonDefaultCandidate.label -ne $candidateVersion -or
        -not $nonDefaultLatest.latestStable -or
        $nonDefaultLatest.route -ne "" -or
        $nonDefaultLatest.label -ne "$previousLatestVersion (latest)") {
        throw "Non-default publication changed latest-version routing."
    }

    if ($canPromoteCandidate) {
        $nonDefaultCandidate.published = $false
        $nonDefaultCandidate.PSObject.Properties.Remove("immutability")
        Write-SnapshotJson `
            -Value $nonDefaultManifest `
            -Path $isolatedManifestPath
        Write-SnapshotJson `
            -Value $priorVersions `
            -Path (Join-Path $isolatedWebsiteRoot "versions.json") `
            -AsArray
        Remove-Item (
            Join-Path $isolatedWebsiteRoot (
                "versioned_docs/version-$candidateVersion"
            )
        ) -Recurse -Force
        Remove-Item (
            Join-Path $isolatedWebsiteRoot (
                "versioned_sidebars/version-$candidateVersion-sidebars.json"
            )
        ) -Force
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -PromoteLatest `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
        $releasedManifest = Get-Content -Raw $isolatedManifestPath |
            ConvertFrom-Json -Depth 20
        & $assertReleaseRoundTrip `
            -ReleasedManifest $releasedManifest `
            -Mode "Promoted"
        $promotedCandidate = @(
            $releasedManifest.versions |
                Where-Object version -eq $candidateVersion
        )[0]
        $demotedLatest = @(
            $releasedManifest.versions |
                Where-Object version -eq $previousLatestVersion
        )[0]
        if (-not $promotedCandidate.latestStable -or
            $promotedCandidate.route -ne "" -or
            $promotedCandidate.label -ne "$candidateVersion (latest)" -or
            $demotedLatest.latestStable -or
            $demotedLatest.route -ne $previousLatestVersion -or
            $demotedLatest.label -ne $previousLatestVersion) {
            throw "Promoted publication did not update latest-version routing."
        }
    } else {
        $releasedManifest = $nonDefaultManifest
    }

    $correctionPath = "start/overview.mdx"
    $correctionSourceRoot = if (
        $correctionPath -in @($releaseOverlay.replacements)
    ) {
        $releaseOverlayRoot
    } else {
        Join-Path $isolatedWebsiteRoot "docs"
    }
    $isolatedCorrectionPage = Join-Path $correctionSourceRoot $correctionPath
    $isolatedSnapshotPage = Join-Path $releaseSnapshotPath $correctionPath
    if (-not (Test-Path $isolatedCorrectionPage -PathType Leaf) -or
        -not (Test-Path $isolatedSnapshotPage -PathType Leaf)) {
        throw "The release fixture requires the authored overview page."
    }
    $snapshotPageHash = (
        Get-FileHash $isolatedSnapshotPage -Algorithm SHA256
    ).Hash
    $snapshotTreeDigest = Get-SnapshotDirectoryDigest $releaseSnapshotPath
    $manifestHash = (Get-FileHash $isolatedManifestPath -Algorithm SHA256).Hash
    $recordedDigest = @(
        $releasedManifest.versions |
            Where-Object version -eq $candidateVersion
    )[0].immutability.snapshotSha256
    Add-Content `
        $isolatedCorrectionPage `
        "`n[broken correction](./missing-correction.mdx)"
    $correctionFailed = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version $candidateVersion `
            -CorrectPage $correctionPath `
            -ManifestPath $isolatedManifestPath `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $correctionFailed = $true
    }
    $afterFailedManifest = Get-Content -Raw $isolatedManifestPath |
        ConvertFrom-Json -Depth 20
    if (-not $correctionFailed -or
        (Get-FileHash $isolatedSnapshotPage -Algorithm SHA256).Hash -ne
            $snapshotPageHash -or
        (Get-SnapshotDirectoryDigest $releaseSnapshotPath) -ne
            $snapshotTreeDigest -or
        (Get-FileHash $isolatedManifestPath -Algorithm SHA256).Hash -ne
            $manifestHash -or
        @(
            $afterFailedManifest.versions |
                Where-Object version -eq $candidateVersion
        )[0].immutability.snapshotSha256 -ne $recordedDigest) {
        throw "Failed staged correction changed content or immutable digest."
    }
} finally {
    Remove-Item $tempRoot -Recurse -Force
}

Write-Host "Documentation manifest, snapshot immutability, idempotence, and failure checks passed."
