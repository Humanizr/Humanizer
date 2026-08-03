function Assert-GeneratedApiLinks {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    if (-not (Test-Path $LinksPath -PathType Leaf)) {
        throw "$VersionLabel did not generate an API links catalog."
    }
    $lines = @(Get-Content $LinksPath)
    if ($lines.Count -lt 2 -or $lines[0] -ne "./") {
        throw "$VersionLabel generated a malformed API links catalog."
    }

    $ids = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    $pageContent = @{}
    foreach ($line in $lines | Select-Object -Skip 1) {
        $parts = @($line -split "\|", 3)
        if ($parts.Count -ne 3 -or
            [string]::IsNullOrWhiteSpace($parts[0]) -or
            -not $ids.Add($parts[0])) {
            throw "$VersionLabel generated a malformed or duplicate API link: $line"
        }

        $target = @($parts[1] -split "#", 2)
        if ($target.Count -gt 2 -or
            [string]::IsNullOrWhiteSpace($target[0]) -or
            $target[0].Contains("/") -or
            $target[0].Contains("\") -or
            -not $target[0].EndsWith(
                ".md",
                [System.StringComparison]::Ordinal
            )) {
            throw "$VersionLabel generated an unsafe API link: $line"
        }
        $pagePath = Join-Path $OutputPath $target[0]
        if (-not (Test-Path $pagePath -PathType Leaf)) {
            throw "$VersionLabel API link targets a missing page: $line"
        }
        if ($target.Count -eq 2) {
            if (-not $pageContent.ContainsKey($pagePath)) {
                $pageContent[$pagePath] = Get-Content -Raw $pagePath
            }
            $anchor = "<a name='$($target[1])'></a>"
            if (-not $pageContent[$pagePath].Contains(
                    $anchor,
                    [System.StringComparison]::Ordinal
                )) {
                throw "$VersionLabel API link targets a missing member anchor: $line"
            }
        }
    }
}

function Assert-ApiAccessCoverage {
    param(
        [Parameter(Mandatory = $true)][string]$ApiLinksPath,
        [Parameter(Mandatory = $true)][string]$PublicLinksPath,
        [Parameter(Mandatory = $true)]$AssemblyInventory,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $apiIds = @(
        Get-Content $ApiLinksPath |
            Select-Object -Skip 1 |
            ForEach-Object { ($_ -split "\|", 3)[0] }
    )
    $publicIds = @(
        Get-Content $PublicLinksPath |
            Select-Object -Skip 1 |
            ForEach-Object { ($_ -split "\|", 3)[0] }
    )
    $apiSet = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    foreach ($id in $apiIds) {
        [void]$apiSet.Add($id)
    }
    $missingPublic = @($publicIds | Where-Object { -not $apiSet.Contains($_) })
    $publicSet = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    foreach ($id in $publicIds) {
        [void]$publicSet.Add($id)
    }
    $apiOnly = @($apiIds | Where-Object { -not $publicSet.Contains($_) })
    $expectedProtected = @(
        $AssemblyInventory.Records |
            Where-Object Access -eq "protected" |
            ForEach-Object Id |
            Sort-Object -Unique
    )
    $actualProtected = @($apiOnly | Sort-Object -Unique)
    if ($missingPublic.Count -gt 0 -or
        ($actualProtected -join "`n") -ne ($expectedProtected -join "`n")) {
        throw "$VersionLabel API access coverage differs from its assembly-derived exact inventory. Missing public: $($missingPublic -join ', '); generated protected: $($actualProtected -join ', '); expected protected: $($expectedProtected -join ', ')."
    }
}
