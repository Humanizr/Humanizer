$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
$manifestPath = Join-Path $repoRoot "website/humanizer-versions.json"

& (Join-Path $PSScriptRoot "build.ps1") -Version "3.0.10" -ValidateOnly

$tempRoot = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-docs-tests-$([guid]::NewGuid().ToString('N'))"
New-Item -ItemType Directory -Path $tempRoot | Out-Null
try {
    $invalidManifestPath = Join-Path $tempRoot "invalid-manifest.json"
    '{"schemaVersion":1,"versions":[]}' | Set-Content $invalidManifestPath
    & pwsh -NoProfile -File (Join-Path $PSScriptRoot "build.ps1") -ValidateOnly -ManifestPath $invalidManifestPath *> $null
    if ($LASTEXITCODE -eq 0) {
        throw "Empty manifest validation unexpectedly passed."
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
} finally {
    Remove-Item $tempRoot -Recurse -Force
}

Write-Host "Documentation missing-input and invalid-manifest checks passed."
