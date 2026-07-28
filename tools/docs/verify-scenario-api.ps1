param(
    [Parameter(Mandatory = $true)][string]$Version,
    [Parameter(Mandatory = $true)][string]$DocsRoot,
    [Parameter(Mandatory = $true)][string]$ApiRoot,
    [string]$ManifestPath,
    [string]$ContractPath,
    [string]$WebsiteRoot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}
if (-not $WebsiteRoot) {
    $WebsiteRoot = Split-Path $ManifestPath -Parent
}
if (-not $ContractPath) {
    $ContractPath = Join-Path $WebsiteRoot "scenario-api-contract.json"
}

$contract = Get-Content -Raw $ContractPath | ConvertFrom-Json -Depth 20
if ($contract.schemaVersion -ne 1 -or
    $null -eq $contract.PSObject.Properties["pages"] -or
    $null -eq $contract.PSObject.Properties["substitutions"] -or
    $null -eq $contract.PSObject.Properties["unavailable"]) {
    throw "The scenario API contract has an unsupported schema."
}

$contractPages = @($contract.pages.PSObject.Properties.Name | Sort-Object)
$canonicalPages = @(
    Get-ChildItem (Join-Path $WebsiteRoot "docs/scenarios") `
        -File |
        Where-Object {
            $_.Name -ne "index.mdx" -and $_.Extension -in @(".md", ".mdx")
        } |
        ForEach-Object { "scenarios/$($_.Name)" } |
        Sort-Object
)
if (($contractPages -join "`n") -ne ($canonicalPages -join "`n")) {
    throw "The scenario API contract must exactly cover every non-index scenario page."
}

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$entries = @($manifest.versions | Where-Object version -eq $Version)
if ($entries.Count -ne 1) {
    throw "Version '$Version' is not declared exactly once."
}
$entry = $entries[0]
$knownVersions = @($manifest.versions.version)
$logicalTargets = [System.Collections.Generic.HashSet[string]]::new(
    [System.StringComparer]::Ordinal
)

function Get-ContractTargets {
    param(
        [Parameter(Mandatory = $true)][string]$Page,
        [string[]]$Resolving = @()
    )

    if ($Page -notin $contractPages -or $Page -in $Resolving) {
        throw "The scenario API contract contains an invalid union: $Page"
    }
    $pageContract = $contract.pages.PSObject.Properties[$Page].Value
    $unionProperty = $pageContract.PSObject.Properties["unionOf"]
    if ($null -eq $unionProperty) {
        $targets = @($pageContract)
        if ($targets.Count -eq 0 -or
            $targets.Count -ne @($targets | Sort-Object -Unique).Count -or
            @($targets | Where-Object {
                $_ -notmatch "^Humanizer(?:\.[A-Za-z0-9_]+)+\.md$"
            }).Count -gt 0) {
            throw "The scenario API contract has invalid targets for $Page."
        }
        foreach ($target in $targets) {
            $null = $logicalTargets.Add($target)
        }
        return $targets
    }
    $members = @($unionProperty.Value)
    if ($members.Count -lt 2 -or
        $members.Count -ne @($members | Sort-Object -Unique).Count) {
        throw "The scenario API contract has an invalid union for $Page."
    }
    return @(
        foreach ($member in $members) {
            Get-ContractTargets `
                -Page $member `
                -Resolving @($Resolving + $Page)
        }
    ) | Sort-Object -Unique
}

$resolvedPageTargets = @{}
foreach ($pagePath in $contractPages) {
    $resolvedPageTargets[$pagePath] = @(
        Get-ContractTargets -Page $pagePath
    )
}

$ruleKeys = [System.Collections.Generic.HashSet[string]]::new(
    [System.StringComparer]::Ordinal
)
foreach ($ruleKind in @("substitutions", "unavailable")) {
    foreach ($rule in @($contract.$ruleKind)) {
        $ruleVersions = @($rule.versions)
        if ($ruleVersions.Count -eq 0 -or
            $ruleVersions.Count -ne
                @($ruleVersions | Sort-Object -Unique).Count -or
            @($ruleVersions | Where-Object { $_ -notin $knownVersions }).Count -gt 0 -or
            -not $logicalTargets.Contains([string]$rule.target)) {
            throw "The scenario API contract has invalid $ruleKind metadata."
        }
        if ($ruleKind -eq "substitutions" -and
            ($rule.replacement -notmatch "^Humanizer(?:\.[A-Za-z0-9_]+)+\.md$" -or
                $rule.replacement -eq $rule.target)) {
            throw "The scenario API contract has an invalid substitution."
        }
        foreach ($ruleVersion in $ruleVersions) {
            $key = "$ruleVersion`n$($rule.target)"
            if (-not $ruleKeys.Add($key)) {
                throw "The scenario API contract has overlapping rules for $($rule.target) in $ruleVersion."
            }
        }
    }
}

$excluded = @()
if ($Version -ne "current") {
    $overlayPath = Join-Path (
        Split-Path $ManifestPath -Parent
    ) "$($entry.compatibilityOverlay)/overlay.json"
    $overlay = Get-Content -Raw $overlayPath | ConvertFrom-Json -Depth 20
    $excluded = @($overlay.exclusions)
}

function Resolve-Target {
    param([Parameter(Mandatory = $true)][string]$Target)

    foreach ($unavailable in @($contract.unavailable)) {
        if ($unavailable.target -eq $Target -and
            $Version -in @($unavailable.versions)) {
            return $null
        }
    }
    foreach ($substitution in @($contract.substitutions)) {
        if ($substitution.target -eq $Target -and
            $Version -in @($substitution.versions)) {
            return [string]$substitution.replacement
        }
    }
    return $Target
}

foreach ($pagePath in $contractPages) {
    $page = Join-Path $DocsRoot $pagePath
    if (-not (Test-Path $page -PathType Leaf)) {
        if ($pagePath -notin $excluded) {
            throw "$Version is missing contracted scenario page $pagePath."
        }
        continue
    }
    if ($pagePath -in $excluded) {
        throw "$Version materialized excluded scenario page $pagePath."
    }

    $content = Get-Content -Raw $page
    $related = [regex]::Match(
        $content,
        "(?ms)^## Related guides and API\s*(?<body>.*?)(?=^## |\z)"
    )
    if (-not $related.Success) {
        throw "$Version scenario has no related API section: $pagePath"
    }
    $links = @(
        [regex]::Matches(
            $related.Groups["body"].Value,
            "\]\((?<target>[^)#\s]+)(?:#[^)]*)?\)"
        ) |
            ForEach-Object { $_.Groups["target"].Value }
    )
    if (@($links | Where-Object { $_ -eq "../api/index.md" }).Count -gt 0) {
        throw "$Version scenario links only to the API root: $pagePath"
    }

    $expected = @(
        foreach ($target in @($resolvedPageTargets[$pagePath])) {
            $resolved = Resolve-Target -Target $target
            if ($null -ne $resolved) {
                $resolved
            }
        }
    )
    $actual = @(
        $links |
            Where-Object { $_ -match "^\.\./api/(?<target>[^/]+\.md)$" } |
            ForEach-Object { $Matches["target"] } |
            Sort-Object -Unique
    )
    if (($actual -join "`n") -ne (@($expected | Sort-Object -Unique) -join "`n")) {
        throw "$Version scenario API targets differ for $pagePath. Actual: $($actual -join ', '); expected: $($expected -join ', ')."
    }
    foreach ($target in $expected) {
        if (-not (Test-Path (Join-Path $ApiRoot $target) -PathType Leaf)) {
            throw "$Version scenario API target does not exist for $pagePath`: $target"
        }
    }
}

Write-Host "Scenario API contract passed: $Version"
