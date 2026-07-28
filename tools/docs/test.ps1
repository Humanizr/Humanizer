param(
    [switch]$RequireNativeAot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
$manifestPath = Join-Path $repoRoot "website/humanizer-versions.json"

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
& (Join-Path $PSScriptRoot "build.ps1") -Version "3.0.10" -ValidateOnly
& (Join-Path $PSScriptRoot "verify-aot.ps1") `
    -RequireNativeAot:$RequireNativeAot

$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $tempRoot | Out-Null
try {
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
        (Join-Path $repoRoot "website/docs/_examples/quick-start") `
        $badExampleRoot `
        -Recurse
    foreach ($buildFile in @(
        "Directory.Build.props",
        "Directory.Build.targets"
    )) {
        Copy-Item `
            (Join-Path $repoRoot "website/docs/_examples/$buildFile") `
            $badExampleRoot
    }
    $badExampleTargets = Join-Path $badExampleRoot "Directory.Build.targets"
    $badExampleText = (Get-Content -Raw $badExampleTargets).Replace(
        'Version="$(HumanizerPackageVersion)"',
        'Version="3.0.10"'
    )
    [System.IO.File]::WriteAllText(
        $badExampleTargets,
        $badExampleText,
        [System.Text.UTF8Encoding]::new($false)
    )
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "verify-examples.ps1") `
        -Version "2.10.1" `
        -ExamplesRoot $badExampleRoot *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "An example restored against the wrong package version."
    }

    $snapshotPath = Join-Path $repoRoot "website/versioned_docs/version-3.0.10"
    $sidebarsPath = Join-Path $repoRoot "website/versioned_sidebars/version-3.0.10-sidebars.json"
    $versionsPath = Join-Path $repoRoot "website/versions.json"
    $snapshotDigest = Get-DirectoryDigest $snapshotPath
    $sidebarsHash = (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash
    $versionsHash = (Get-FileHash $versionsPath -Algorithm SHA256).Hash

    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version "3.0.10" *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Existing snapshot mutation unexpectedly passed."
    }

    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version "3.0.10" `
        -CorrectPage "api/Humanizer.StringHumanizeExtensions.md" *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Generated API correction unexpectedly passed."
    }

    if ((Get-DirectoryDigest $snapshotPath) -ne $snapshotDigest -or
        (Get-FileHash $sidebarsPath -Algorithm SHA256).Hash -ne $sidebarsHash -or
        (Get-FileHash $versionsPath -Algorithm SHA256).Hash -ne $versionsHash) {
        throw "A rejected snapshot operation changed immutable output."
    }

    $isolatedWebsiteRoot = Join-Path $tempRoot "website"
    New-Item -ItemType Directory `
        -Path (Join-Path $isolatedWebsiteRoot "docs") `
        -Force | Out-Null
    New-Item -ItemType Directory `
        -Path (Join-Path $isolatedWebsiteRoot "versioned_docs/version-3.0.10") `
        -Force | Out-Null
    New-Item -ItemType Directory `
        -Path (Join-Path $isolatedWebsiteRoot "versioned_sidebars") `
        -Force | Out-Null
    Copy-Item `
        (Join-Path $repoRoot "website/humanizer-versions.json") `
        $isolatedWebsiteRoot
    Copy-Item `
        (Join-Path $repoRoot "website/versions.json") `
        $isolatedWebsiteRoot
    Copy-Item `
        (Join-Path $repoRoot "website/version-overrides") `
        $isolatedWebsiteRoot `
        -Recurse
    foreach ($path in @("index.md", "proof.mdx")) {
        Copy-Item `
            (Join-Path $repoRoot "website/docs/$path") `
            (Join-Path $isolatedWebsiteRoot "docs/$path")
        Copy-Item `
            (Join-Path $snapshotPath $path) `
            (Join-Path $isolatedWebsiteRoot "versioned_docs/version-3.0.10/$path")
    }
    Copy-Item `
        $sidebarsPath `
        (Join-Path $isolatedWebsiteRoot "versioned_sidebars")

    $isolatedOverlayProof = Join-Path (
        $isolatedWebsiteRoot
    ) "version-overrides/3.0.10/proof.mdx"
    $isolatedSnapshotProof = Join-Path (
        $isolatedWebsiteRoot
    ) "versioned_docs/version-3.0.10/proof.mdx"
    $snapshotProofHash = (
        Get-FileHash $isolatedSnapshotProof -Algorithm SHA256
    ).Hash
    Add-Content `
        $isolatedOverlayProof `
        "`n<!-- correction transaction test -->"
    $correctionFailed = $false
    try {
        & (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version "3.0.10" `
            -CorrectPage @("proof.mdx", "missing.md") `
            -ManifestPath (Join-Path $isolatedWebsiteRoot "humanizer-versions.json") `
            -WebsiteRoot $isolatedWebsiteRoot *> $null
    } catch {
        $correctionFailed = $true
    }
    if (-not $correctionFailed) {
        throw "Invalid multi-page historical correction unexpectedly passed."
    }
    if ((Get-FileHash $isolatedSnapshotProof -Algorithm SHA256).Hash -ne
        $snapshotProofHash) {
        throw "Failed multi-page correction partially changed a snapshot."
    }

    foreach ($iteration in 1..2) {
        & pwsh -NoProfile -File (Join-Path $PSScriptRoot "snapshot.ps1") `
            -Version "3.0.10" `
            -Check *> $null
        if ($LASTEXITCODE -ne 0) {
            throw "Snapshot idempotence check $iteration failed."
        }
    }

    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version "3.0.10" `
        -CorrectPage "proof.mdx" `
        -Check *> $null
    if ($LASTEXITCODE -ne 0) {
        throw "Exact historical page correction check failed."
    }

    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "snapshot.ps1") `
        -Version "3.0.10" `
        -CorrectPage "_examples/quick-start/Program.cs" `
        -Check *> $null
    if ($LASTEXITCODE -ne 0) {
        throw "Exact historical example correction check failed."
    }
} finally {
    Remove-Item $tempRoot -Recurse -Force
}

Write-Host "Documentation manifest, snapshot immutability, idempotence, and failure checks passed."
