param(
    [switch]$Check,
    [switch]$UpdateReferenceFingerprint,
    [string]$Version = "current",
    [string]$LocalesPath,
    [string]$OutputPath,
    [string]$ProjectPath,
    [string]$ManifestPath,
    [string]$ReferencePath
)

$ErrorActionPreference = "Stop"
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "../..")).Path
. (Join-Path $PSScriptRoot "nuget-package.ps1")

if (-not $ManifestPath) {
    $ManifestPath = Join-Path $repoRoot "website/humanizer-versions.json"
}
if (-not $ProjectPath) {
    $ProjectPath = Join-Path $repoRoot "src/Humanizer/Humanizer.csproj"
}
if (-not $LocalesPath) {
    $LocalesPath = Join-Path $repoRoot "src/Humanizer/Locales"
}
if (-not $ReferencePath) {
    $ReferencePath = Join-Path $repoRoot "website/docs/contributing/locale-yaml-reference.mdx"
}
if (-not $OutputPath) {
    if ($Version -ne "current") {
        throw "Historical language coverage requires -OutputPath so U7 can stage the version-specific sibling JSON."
    }
    $OutputPath = Join-Path $repoRoot "website/docs/languages/language-coverage.json"
}

function Get-NormalizedSha256 {
    param([Parameter(Mandatory = $true)][string]$Path)

    $content = [System.IO.File]::ReadAllText($Path).
        Replace("`r`n", "`n").
        Replace("`r", "`n")
    $bytes = [System.Text.UTF8Encoding]::new($false).GetBytes($content)
    return [Convert]::ToHexString(
        [System.Security.Cryptography.SHA256]::HashData($bytes)
    ).ToLowerInvariant()
}

function Get-ReferenceFingerprintEntries {
    $paths = @(
        "src/Humanizer/Bytes/ByteSize.cs",
        "src/Humanizer/Configuration/LocaliserRegistry.cs",
        "src/Humanizer/GrammaticalGender.cs",
        "src/Humanizer/Localisation/CollectionFormatters/CliticCollectionFormatter.cs",
        "src/Humanizer/Localisation/CollectionFormatters/DefaultCollectionFormatter.cs",
        "src/Humanizer/Localisation/CollectionFormatters/DelimitedCollectionFormatter.cs",
        "src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDateCalendarMode.cs",
        "src/Humanizer/Localisation/DateToOrdinalWords/OrdinalDatePattern.cs",
        "src/Humanizer/Localisation/EifelerRule.cs",
        "src/Humanizer/Localisation/Formatters/ProfiledFormatter.cs",
        "src/Humanizer/Localisation/NumberToWords/BillionStrategyNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/ConjunctionalScaleNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/HarmonyOrdinalNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/InvertedTensNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/JoinedScaleNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/PluralizedScaleNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/ScaleStrategyNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/SouthSlavicCardinalNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/UnitLeadingCompoundNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/NumberToWords/VariantDecadeNumberToWordsConverter.cs",
        "src/Humanizer/Localisation/Ordinalizers/TemplateOrdinalizer.cs",
        "src/Humanizer/Localisation/Ordinalizers/WordFormTemplateOrdinalizer.cs",
        "src/Humanizer/Localisation/TimeToClockNotation/PhraseClockNotationConverter.cs",
        "src/Humanizer/Localisation/WordsToNumber/TokenMapWordsToNumberConverter.cs",
        "src/Humanizer/Localisation/WordsToNumber/TokenMapWordsToNumberOrdinalMapBuilder.cs",
        "src/Humanizer/MetricNumeralExtensions.cs",
        "src/Humanizer/OrdinalizeExtensions.cs",
        "src/Humanizer.SourceGenerators/Common/CanonicalLocaleAuthoring.cs",
        "src/Humanizer.SourceGenerators/Common/LocaleYamlCatalog.cs",
        "src/Humanizer.SourceGenerators/Common/EngineContractCatalog.cs",
        "src/Humanizer.SourceGenerators/Common/EngineContractModels.cs",
        "src/Humanizer.SourceGenerators/Common/EngineContractUtilities.cs",
        "src/Humanizer.SourceGenerators/Common/ProfileDefinitions.cs",
        "src/Humanizer.SourceGenerators/Common/GenerationHelpers.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/NumberToWordsEngineContractFactory.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/WordsToNumberEngineContractFactory.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/TimeOnlyToClockNotationEngineContractFactory.cs",
        "src/Humanizer.SourceGenerators/Generators/TokenMapWordsToNumberInput.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/FormatterProfileCatalogInput.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/InflectionCatalogInput.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalizerProfileCatalogInput.cs",
        "src/Humanizer.SourceGenerators/Generators/ProfileCatalogs/OrdinalDateProfileCatalogInput.cs",
        "src/Humanizer.SourceGenerators/Generators/LocaleRegistryInput.cs",
        "src/Humanizer.SourceGenerators/Common/LocalePhraseNormalization.cs"
    )

    $entries = [ordered]@{}
    foreach ($relativePath in $paths | Sort-Object) {
        $sourcePath = Join-Path $repoRoot $relativePath
        if (-not (Test-Path $sourcePath -PathType Leaf)) {
            throw "Locale reference fingerprint source is missing: $relativePath"
        }
        $entries[$relativePath] = Get-NormalizedSha256 -Path $sourcePath
    }
    return $entries
}

function Get-ReferenceFingerprintMatch {
    param([Parameter(Mandatory = $true)][string]$Content)

    $markerStart = "<!-- humanizer-locale-reference-fingerprint:v1"
    $markerCount = [regex]::Matches(
        $Content,
        [regex]::Escape($markerStart)
    ).Count
    if ($markerCount -ne 1) {
        throw "Locale YAML reference must contain exactly one '$markerStart' marker; found $markerCount."
    }

    $pattern = "(?ms)^<!-- humanizer-locale-reference-fingerprint:v1`n(?<body>.*?)`n-->$"
    $matches = [regex]::Matches($Content, $pattern)
    if ($matches.Count -ne 1) {
        throw "Locale YAML reference fingerprint marker is malformed."
    }
    return $matches[0]
}

function Format-ReferenceFingerprint {
    param([Parameter(Mandatory = $true)]$Entries)

    $lines = @("<!-- humanizer-locale-reference-fingerprint:v1")
    foreach ($relativePath in $Entries.Keys) {
        $lines += "$relativePath $($Entries[$relativePath])"
    }
    $lines += "-->"
    return $lines -join "`n"
}

function Assert-ReferenceFingerprint {
    $referenceContent = [System.IO.File]::ReadAllText($ReferencePath).
        Replace("`r`n", "`n").
        Replace("`r", "`n")
    $match = Get-ReferenceFingerprintMatch -Content $referenceContent
    $expected = Get-ReferenceFingerprintEntries

    if ($UpdateReferenceFingerprint) {
        if ($Check) {
            throw "-UpdateReferenceFingerprint cannot be combined with -Check."
        }
        $replacement = Format-ReferenceFingerprint -Entries $expected
        $updated = $referenceContent.Substring(0, $match.Index) +
            $replacement +
            $referenceContent.Substring($match.Index + $match.Length)
        [System.IO.File]::WriteAllText(
            $ReferencePath,
            $updated,
            [System.Text.UTF8Encoding]::new($false)
        )
        Write-Host "Updated locale YAML reference fingerprint after explicit human review."
        return $false
    }

    $actual = [ordered]@{}
    $invalidLines = [System.Collections.Generic.List[string]]::new()
    foreach ($line in $match.Groups["body"].Value -split "`n") {
        if ($line -notmatch "^(?<path>\S+) (?<hash>[0-9a-f]{64})$") {
            $invalidLines.Add($line)
            continue
        }
        if ($actual.Contains($Matches["path"])) {
            throw "Locale YAML reference fingerprint contains duplicate entry '$($Matches["path"])'."
        }
        $actual[$Matches["path"]] = $Matches["hash"]
    }
    if ($invalidLines.Count -gt 0) {
        throw "Locale YAML reference fingerprint contains malformed entries: $($invalidLines -join ', ')."
    }

    $missing = @($expected.Keys | Where-Object { -not $actual.Contains($_) })
    $extra = @($actual.Keys | Where-Object { -not $expected.Contains($_) })
    if ($missing.Count -gt 0 -or $extra.Count -gt 0) {
        throw "Locale YAML reference fingerprint entry set changed. Missing: $($missing -join ', '); extra: $($extra -join ', '). Re-review the reference before updating the fingerprint."
    }

    $stale = @(
        $expected.Keys |
            Where-Object { $actual[$_] -cne $expected[$_] }
    )
    if ($stale.Count -gt 0) {
        throw "Locale YAML reference fingerprint is stale for: $($stale -join ', '). Re-review locale-yaml-reference.mdx against those generator sources, then run tools/docs/generate-language-coverage.ps1 -UpdateReferenceFingerprint."
    }
    return $true
}

if (-not (Test-Path $ReferencePath -PathType Leaf)) {
    throw "Locale YAML reference not found: $ReferencePath"
}
if (-not (Assert-ReferenceFingerprint)) {
    return
}

if (-not (Test-Path $ManifestPath -PathType Leaf)) {
    throw "Version manifest not found: $ManifestPath"
}

$manifest = Get-Content -Raw $ManifestPath | ConvertFrom-Json -Depth 20
$entry = @($manifest.versions | Where-Object version -eq $Version)
if ($entry.Count -ne 1) {
    throw "Version '$Version' is not declared exactly once."
}
$entry = $entry[0]

function Get-MsBuildProperties {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string[]]$Names,
        [hashtable]$Properties = @{}
    )

    $arguments = @(
        "msbuild",
        $Path,
        "-getProperty:$($Names -join ',')",
        "-nologo"
    )
    foreach ($property in $Properties.GetEnumerator()) {
        $arguments += "-p:$($property.Key)=$($property.Value)"
    }

    $result = & dotnet @arguments | ConvertFrom-Json -Depth 10
    if ($LASTEXITCODE -ne 0) {
        throw "Could not evaluate $($Names -join ', ') for $Path."
    }
    return $result.Properties
}

function Get-ApiAvailability {
    param(
        [Parameter(Mandatory = $true)][hashtable]$XmlByFramework
    )

    $dateOnlyOrdinals = [System.Collections.Generic.List[string]]::new()
    $clockNotation = [System.Collections.Generic.List[string]]::new()
    foreach ($framework in @(Sort-TargetFrameworks $XmlByFramework.Keys)) {
        $xmlPath = $XmlByFramework[$framework]
        if (-not (Test-Path $xmlPath -PathType Leaf)) {
            throw "API XML evidence is missing for $framework`: $xmlPath"
        }

        $documentation = [xml][System.IO.File]::ReadAllText($xmlPath)
        $memberNames = @($documentation.doc.members.member | ForEach-Object name)
        if ($memberNames -contains "M:Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(System.DateOnly)") {
            $dateOnlyOrdinals.Add($framework)
        }
        if ($memberNames -contains "M:Humanizer.TimeOnlyToClockNotationExtensions.ToClockNotation(System.TimeOnly,Humanizer.ClockNotationRounding)") {
            $clockNotation.Add($framework)
        }
    }

    return [ordered]@{
        dateOnlyOrdinals = @($dateOnlyOrdinals)
        clockNotation = @($clockNotation)
    }
}

function Sort-TargetFrameworks {
    param([Parameter(Mandatory = $true)][string[]]$Frameworks)

    return @(
        $Frameworks |
            Sort-Object {
                if ($_ -match "^netstandard(\d+)\.(\d+)$") {
                    return "00:{0:D4}:{1:D4}" -f [int]$Matches[1], [int]$Matches[2]
                }
                if ($_ -match "^net(\d)(\d)$") {
                    return "01:{0:D4}:{1:D4}" -f [int]$Matches[1], [int]$Matches[2]
                }
                if ($_ -match "^net(\d+)\.(\d+)$") {
                    return "02:{0:D4}:{1:D4}" -f [int]$Matches[1], [int]$Matches[2]
                }
                return "99:$_"
            }
    )
}

function Get-CurrentEvidence {
    if (-not (Test-Path $LocalesPath -PathType Container)) {
        throw "Locale YAML directory not found: $LocalesPath"
    }

    $properties = Get-MsBuildProperties `
        -Path $ProjectPath `
        -Names @("PackageId", "TargetFrameworks")
    $targetFrameworks = @(
        Sort-TargetFrameworks @($properties.TargetFrameworks -split ";" | Where-Object { $_ })
    )
    if (-not $properties.PackageId -or $targetFrameworks.Count -eq 0) {
        throw "Humanizer package facts no longer declare PackageId and TargetFrameworks."
    }

    Invoke-DocsCheckedCommand `
        -FilePath "dotnet" `
        -ArgumentList @(
            "build",
            $ProjectPath,
            "--configuration", "Release",
            "--nologo"
        ) `
        -WorkingDirectory $repoRoot

    $xmlByFramework = @{}
    foreach ($framework in $targetFrameworks) {
        $targetProperties = Get-MsBuildProperties `
            -Path $ProjectPath `
            -Names @("TargetPath", "DocumentationFile") `
            -Properties @{
                Configuration = "Release"
                TargetFramework = $framework
            }
        $xmlByFramework[$framework] = [string]$targetProperties.DocumentationFile
    }

    return [ordered]@{
        coverageKind = "yaml-capabilities"
        versionKey = "current"
        generatedFor = "main/preview"
        source = "src/Humanizer/Locales/*.yml"
        localesPath = [System.IO.Path]::GetFullPath($LocalesPath)
        package = [ordered]@{
            id = [string]$properties.PackageId
            targetFrameworks = $targetFrameworks
            localeData = "generated into the main package"
            apiAvailability = Get-ApiAvailability -XmlByFramework $xmlByFramework
        }
    }
}

function Get-PublishedEvidenceFromPackages {
    param(
        [Parameter(Mandatory = $true)]$MetaPackage,
        [Parameter(Mandatory = $true)]$ApiPackage
    )

    $nuspecPath = Get-ChildItem $MetaPackage.ExtractPath -Filter "*.nuspec" -File |
        Select-Object -ExpandProperty FullName -First 1
    if (-not $nuspecPath) {
        throw "Verified metapackage is missing its nuspec: $($MetaPackage.PackagePath)"
    }
    $apiNuspecPath = Get-ChildItem $ApiPackage.ExtractPath -Filter "*.nuspec" -File |
        Select-Object -ExpandProperty FullName -First 1
    if (-not $apiNuspecPath) {
        throw "Verified API package is missing its nuspec: $($ApiPackage.PackagePath)"
    }

    $nuspec = [xml][System.IO.File]::ReadAllText($nuspecPath)
    $apiNuspec = [xml][System.IO.File]::ReadAllText($apiNuspecPath)
    if ([string]$nuspec.package.metadata.version -ne [string]$entry.source.packageVersion) {
        throw "Verified metapackage nuspec version does not match manifest version $($entry.source.packageVersion)."
    }
    if ([string]$apiNuspec.package.metadata.version -ne [string]$entry.source.packageVersion) {
        throw "Verified API package nuspec version does not match manifest version $($entry.source.packageVersion)."
    }

    $dependencies = @(
        $nuspec.package.metadata.dependencies.group.dependency |
            Where-Object { $_ }
    ) + @(
        $nuspec.package.metadata.dependencies.dependency |
            Where-Object { $_ }
    )
    $localePrefix = "$($entry.apiPackage)."
    $localeDependencies = @(
        $dependencies |
            Where-Object {
                ([string]$_.id).StartsWith($localePrefix, [StringComparison]::OrdinalIgnoreCase)
            }
    )
    $duplicates = @(
        $localeDependencies |
            Group-Object { ([string]$_.id).ToLowerInvariant() } |
            Where-Object Count -gt 1
    )
    if ($duplicates.Count -gt 0) {
        throw "Verified metapackage declares duplicate locale dependencies: $(($duplicates.Name | Sort-Object) -join ', ')."
    }

    $allowedDependencyVersions = @(
        [string]$entry.source.packageVersion,
        "[$($entry.source.packageVersion)]"
    )
    foreach ($dependency in $localeDependencies) {
        $dependencyId = [string]$dependency.id
        $dependencyVersion = [string]$dependency.version
        $culture = $dependencyId.Substring($localePrefix.Length)
        if ($culture -notmatch "^[a-z]{2,3}(?:-[A-Za-z0-9]{2,8})*$") {
            throw "Verified metapackage declares malformed locale dependency '$dependencyId'."
        }
        if ($dependencyVersion -notin $allowedDependencyVersions) {
            throw "Locale dependency '$dependencyId' does not exactly match manifest version $($entry.source.packageVersion)."
        }
    }

    $cultures = @(
        "en"
        $localeDependencies |
            ForEach-Object { ([string]$_.id).Substring($localePrefix.Length) }
    ) | Sort-Object -Unique
    if ($cultures.Count -le 1) {
        throw "Verified metapackage did not declare locale dependencies for $Version."
    }

    $xmlByFramework = @{}
    $libRoot = Join-Path $ApiPackage.ExtractPath "lib"
    foreach ($frameworkDirectory in Get-ChildItem $libRoot -Directory | Sort-Object Name) {
        $xmlPath = Join-Path $frameworkDirectory.FullName "Humanizer.xml"
        $dllPath = Join-Path $frameworkDirectory.FullName "Humanizer.dll"
        if (-not (Test-Path $dllPath -PathType Leaf) -or
            -not (Test-Path $xmlPath -PathType Leaf)) {
            throw "Verified API package has an incomplete Humanizer DLL/XML pair for $($frameworkDirectory.Name)."
        }
        $xmlByFramework[$frameworkDirectory.Name] = $xmlPath
    }
    if ($xmlByFramework.Count -eq 0) {
        throw "Verified API package has no Humanizer DLL/XML assets: $($ApiPackage.PackagePath)"
    }

    return [ordered]@{
        coverageKind = "published-package-cultures"
        versionKey = $Version
        generatedFor = $Version
        source = "verified NuGet packages $($entry.installPackage) and $($entry.apiPackage)"
        cultures = $cultures
        package = [ordered]@{
            id = [string]$entry.installPackage
            version = [string]$entry.source.packageVersion
            apiPackage = [string]$entry.apiPackage
            targetFrameworks = @(Sort-TargetFrameworks $xmlByFramework.Keys)
            localeData = "culture packages declared by the verified metapackage; neutral en supplied by the verified API package"
            apiAvailability = Get-ApiAvailability -XmlByFramework $xmlByFramework
        }
    }
}

function Get-PublishedEvidence {
    if ($entry.source.kind -ne "nuget" -or -not $entry.source.packageVersion) {
        throw "Version '$Version' is not backed by a published NuGet package."
    }

    return Use-DocsNuGetPackage `
        -Entry $entry `
        -RepoRoot $repoRoot `
        -PackageId $entry.installPackage `
        -Action {
        param($metaPackage)

        Use-DocsNuGetPackage `
            -Entry $entry `
            -RepoRoot $repoRoot `
            -PackageId $entry.apiPackage `
            -Action {
            param($apiPackage)

            Get-PublishedEvidenceFromPackages `
                -MetaPackage $metaPackage `
                -ApiPackage $apiPackage
        }
    }
}

$input = if ($Version -eq "current") {
    Get-CurrentEvidence
} else {
    Get-PublishedEvidence
}

$inputPath = Join-Path ([System.IO.Path]::GetTempPath()) "humanizer-language-coverage-$([guid]::NewGuid().ToString('N')).json"
try {
    $inputJson = ($input | ConvertTo-Json -Depth 20).Replace("`r`n", "`n") + "`n"
    [System.IO.File]::WriteAllText(
        $inputPath,
        $inputJson,
        [System.Text.UTF8Encoding]::new($false)
    )

    $arguments = @(
        "run",
        "--project", (Join-Path $PSScriptRoot "language-coverage/Humanizer.Docs.LanguageCoverage.csproj"),
        "--configuration", "Release",
        "--",
        $inputPath,
        [System.IO.Path]::GetFullPath($OutputPath)
    )
    if ($Check) {
        $arguments += "--check"
    }
    & dotnet @arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Language coverage generator failed for $Version."
    }
} finally {
    Remove-Item $inputPath -Force -ErrorAction SilentlyContinue
}
