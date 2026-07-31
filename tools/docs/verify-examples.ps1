param(
    [switch]$All,
    [string[]]$Area,
    [string]$Version,
    [string]$ManifestPath,
    [string]$ExamplesRoot
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "nuget-package.ps1")
if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}
$explicitExamplesRoot = -not [string]::IsNullOrWhiteSpace($ExamplesRoot)
$requestedAreas = @(
    $Area |
        ForEach-Object { $_ -split "," } |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
        ForEach-Object { $_.Trim() } |
        Sort-Object -Unique
)

& (Join-Path $PSScriptRoot "verify-manifest.ps1") `
    -ManifestPath $ManifestPath `
    -WebsiteRoot (Split-Path $ManifestPath -Parent) `
    -SkipNativeState:$explicitExamplesRoot

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$versions = @($manifest.versions)
if ($Version) {
    $versions = @($versions | Where-Object version -eq $Version)
} elseif (-not $All) {
    $versions = @(
        $versions |
            Where-Object { $_.latestStable -or $_.version -eq "current" }
    )
}
if ($versions.Count -eq 0) {
    throw "No example versions matched the requested verification."
}

$artifactsRoot = Join-Path (
    [System.IO.Path]::GetTempPath()
) "humanizer-docs-examples-$([guid]::NewGuid().ToString('N'))"
$canonicalExamplesRoot = [System.IO.Path]::GetFullPath(
    (Join-Path $repoRoot "website/docs/_examples")
)
try {
    foreach ($entry in $versions) {
        $entryExamplesRoot = if ($explicitExamplesRoot) {
            $ExamplesRoot
        } elseif ($entry.published) {
            Join-Path $repoRoot "website/versioned_docs/version-$($entry.version)/_examples"
        } else {
            Join-Path $repoRoot "website/docs/_examples"
        }
        $allProjects = @(
            Get-ChildItem $entryExamplesRoot `
                -Filter "*.csproj" `
                -File `
                -Recurse
        )
        $projectAreas = @{}
        foreach ($project in $allProjects) {
            $projectAreas[$project.FullName] = @(
                [regex]::Matches(
                    (Get-Content -Raw $project.FullName),
                    "<DocumentationArea>\s*(?<area>[^<]+?)\s*</DocumentationArea>"
                ) |
                    ForEach-Object { $_.Groups["area"].Value.Trim() }
            )
        }
        foreach ($requestedArea in $requestedAreas) {
            $matchingProjects = @(
                $allProjects |
                Where-Object {
                    $projectAreas[$_.FullName] -contains $requestedArea
                }
            )
            if ($matchingProjects.Count -eq 0) {
                throw "No runnable documentation examples were found in area $requestedArea for $($entry.version)."
            }
        }
        $projects = @(
            $allProjects |
                Where-Object {
                    if ($requestedAreas.Count -eq 0) {
                        return $true
                    }

                    $projectArea = @($projectAreas[$_.FullName])
                    return @(
                        $requestedAreas |
                            Where-Object {
                                $projectArea -contains $_
                            }
                    ).Count -gt 0
                }
        )
        if ($projects.Count -eq 0) {
            $areaDescription = if ($requestedAreas.Count -gt 0) {
                " in area(s) $($requestedAreas -join ', ')"
            } else {
                ""
            }
            throw "No runnable documentation examples were found$areaDescription for $($entry.version)."
        }

        for ($projectIndex = 0; $projectIndex -lt $projects.Count; $projectIndex++) {
            $project = $projects[$projectIndex]
            $projectArtifacts = Join-Path (
                $artifactsRoot
            ) "$($entry.version)/$($project.BaseName)-$projectIndex"
            $usesVersionedDefaults =
                $entry.source.kind -eq "nuget" -and
                [System.IO.Path]::GetFullPath($entryExamplesRoot) -ne
                    $canonicalExamplesRoot
            $inputArguments = @(
                if ($entry.source.kind -eq "checkout") {
                    "-p:HumanizerProject=$(Join-Path $repoRoot 'src/Humanizer/Humanizer.csproj')"
                } elseif (-not $usesVersionedDefaults) {
                    "-p:HumanizerPackageVersion=$($entry.source.packageVersion)"
                }
            )
            $exampleExcludedAssetsProperty = $entry.PSObject.Properties[
                "exampleExcludedAssets"
            ]
            $exampleExcludedAssets = if (
                $null -eq $exampleExcludedAssetsProperty
            ) {
                @()
            } else {
                @($exampleExcludedAssetsProperty.Value)
            }
            if ($exampleExcludedAssets.Count -gt 0 -and
                -not $usesVersionedDefaults) {
                $encodedExcludedAssets = (
                    $exampleExcludedAssets -join "%3B"
                )
                $inputArguments += (
                    "-p:HumanizerExampleExcludeAssets=" +
                    $encodedExcludedAssets
                )
            }
            $restoreArguments = @(
                "restore",
                $project.FullName,
                "--artifacts-path", $projectArtifacts,
                "--nologo"
            ) + $inputArguments
            & dotnet @restoreArguments
            if ($LASTEXITCODE -ne 0) {
                throw "Example $($project.Name) failed to restore for $($entry.version)."
            }

            $assetFiles = @(
                Get-ChildItem $projectArtifacts `
                    -Filter "project.assets.json" `
                    -File `
                    -Recurse |
                    Where-Object {
                        $_.Directory.Name -eq $project.BaseName
                    }
            )
            if ($assetFiles.Count -ne 1) {
                throw "Example $($project.Name) did not produce one restore graph."
            }
            $assets = Get-Content -Raw $assetFiles[0].FullName |
                ConvertFrom-Json -Depth 100
            if ($entry.source.kind -eq "checkout") {
                $projectPath = [System.IO.Path]::GetFullPath(
                    (Join-Path $repoRoot "src/Humanizer/Humanizer.csproj")
                )
                $projectReferences = @(
                    foreach ($framework in
                        $assets.project.restore.frameworks.PSObject.Properties.Value) {
                        @($framework.projectReferences.PSObject.Properties.Name)
                    }
                )
                if ($projectPath -notin $projectReferences) {
                    throw "Example $($project.Name) did not resolve the current Humanizer project."
                }
            } else {
                Assert-DocsHumanizerPackageGraph `
                    -Entry $entry `
                    -Libraries $assets.libraries `
                    -Context "Example $($project.Name)"
            }

            $runArguments = @(
                "run",
                "--project", $project.FullName,
                "--configuration", "Release",
                "--artifacts-path", $projectArtifacts,
                "--no-restore"
            ) + $inputArguments
            $standardErrorPath = Join-Path $projectArtifacts "stderr.txt"
            $actualOutput = @(& dotnet @runArguments 2> $standardErrorPath)
            $runExitCode = $LASTEXITCODE
            if ($runExitCode -ne 0) {
                $standardError = @(
                    Get-Content $standardErrorPath -ErrorAction SilentlyContinue
                ) -join "`n"
                throw @"
Example $($project.Name) failed for $($entry.version).
Standard output:
$($actualOutput -join "`n")
Standard error:
$standardError
"@
            }
            $actualText = $actualOutput -join "`n"
            $expectedOutputPath = Join-Path (
                $project.DirectoryName
            ) "expected-output.txt"
            $hasExpectedOutput = Test-Path $expectedOutputPath -PathType Leaf
            if ($entry.version -eq "current" -and -not $hasExpectedOutput) {
                throw "Example $($project.Name) has no expected-output.txt."
            }
            if ($hasExpectedOutput) {
                $expectedText = @(Get-Content $expectedOutputPath) -join "`n"
                if (-not [string]::Equals(
                    $expectedText,
                    $actualText,
                    [System.StringComparison]::Ordinal
                )) {
                    throw @"
Example $($project.Name) output differed for $($entry.version).
Expected:
$expectedText
Actual:
$actualText
"@
                }
            }
            Write-Host $actualText
        }

        Write-Host "Runnable examples passed: $($entry.version)"
    }
} finally {
    Remove-Item $artifactsRoot -Recurse -Force -ErrorAction SilentlyContinue
}
