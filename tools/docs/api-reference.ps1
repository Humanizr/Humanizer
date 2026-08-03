function Test-UsesIndexedApiReference {
    param([Parameter(Mandatory = $true)]$Entry)

    if ($Entry.version -eq "current") {
        return $true
    }

    return [System.Management.Automation.SemanticVersion]::Parse(
        $Entry.version
    ).Major -ge 4
}

function Add-ApiLanding {
    param(
        [Parameter(Mandatory = $true)][string]$ApiRoot,
        [Parameter(Mandatory = $true)]$Entry
    )

    $provenance = if ($Entry.version -eq "current") {
        if ($Entry.label -notmatch "^(?<major>[0-9]+)(?:\.|$)") {
            throw "Current API version label has no major version: $($Entry.label)"
        }
        "Generated from the Humanizer $($Matches['major']) ``$($Entry.referenceTfm)`` reference assembly."
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
    if (Test-UsesIndexedApiReference -Entry $Entry) {
        $namespacePages = [System.Collections.Generic.List[object]]::new()
        Get-ChildItem $ApiRoot -Filter "*.md" -File |
            Where-Object Name -ne "index.md" |
            ForEach-Object {
                $content = Get-Content -Raw $_.FullName
                if ($content -match "(?m)^## (?<namespace>[^\r\n]+) Namespace\r?$") {
                    $namespacePages.Add(
                        [PSCustomObject]@{
                            Content = $content
                            File = $_
                            Namespace = $Matches["namespace"]
                        }
                    )
                }
            }
        $namespacePages.Sort(
            [System.Comparison[object]] {
                param($left, $right)

                $comparison = [StringComparer]::Ordinal.Compare(
                    [string]$left.Namespace,
                    [string]$right.Namespace
                )
                if ($comparison -eq 0) {
                    $comparison = [StringComparer]::Ordinal.Compare(
                        [string]$left.File.Name,
                        [string]$right.File.Name
                    )
                }
                return $comparison
            }
        )
        if ($namespacePages.Count -eq 0) {
            throw "The API generator did not emit a namespace index."
        }

        $namespaceSections = [System.Collections.Generic.List[string]]::new()
        foreach ($namespacePage in $namespacePages) {
            $heading = "### [$($namespacePage.Namespace) Namespace]($($namespacePage.File.Name))"
            $content = [regex]::new(
                "(?m)^## [^\r\n]+ Namespace\r?$"
            ).Replace($namespacePage.Content, $heading, 1).Trim()
            $namespaceSections.Add($content)
        }
        $landing += @"


Search this version for a type or member, or browse the generated index below. Each type page lists its signature, members, parameters, return values, and XML documentation.

## Namespaces

$($namespaceSections -join "`n`n")
"@
    }

    [System.IO.File]::WriteAllText(
        (Join-Path $ApiRoot "index.md"),
        "$landing`n",
        [System.Text.UTF8Encoding]::new($false)
    )
}

function Invoke-DefaultDocumentationApi {
    param(
        [Parameter(Mandatory = $true)]$ApiInput,
        [Parameter(Mandatory = $true)]$ExpectedRecords,
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)][string]$RepositoryRoot,
        [string]$ConfigurationFilePath,
        [string]$AccessModifiers = "Api"
    )

    $defaultDocumentationInput = New-DefaultDocumentationApiInput `
        -ApiInput $ApiInput `
        -ExpectedRecords $ExpectedRecords
    try {
        $arguments = @(
            "tool", "run", "defaultdocumentation", "--",
            "--AssemblyFilePath", $defaultDocumentationInput.ApiInput.Dll,
            "--DocumentationFilePath", $defaultDocumentationInput.ApiInput.Xml,
            "--OutputDirectoryPath", $OutputPath,
            "--AssemblyPageName", "assembly",
            "--GeneratedAccessModifiers", $AccessModifiers,
            "--IncludeUndocumentedItems", "true",
            "--GeneratedPages", "Namespaces,Types",
            "--Sections", "Default",
            "--LinksOutputFilePath", $LinksPath,
            "--LinksBaseUrl", "./"
        )
        if ($ConfigurationFilePath) {
            $arguments += @(
                "--ConfigurationFilePath",
                $ConfigurationFilePath
            )
        }
        Invoke-DocsCheckedCommand `
            -FilePath "dotnet" `
            -ArgumentList $arguments `
            -WorkingDirectory $RepositoryRoot
    } finally {
        if ($null -ne $defaultDocumentationInput.TemporaryXml) {
            Remove-Item $defaultDocumentationInput.TemporaryXml -Force
        }
    }
}

function New-DefaultDocumentationApiInput {
    param(
        [Parameter(Mandatory = $true)]$ApiInput,
        [Parameter(Mandatory = $true)]$ExpectedRecords
    )

    $checkedRecords = @(
        $ExpectedRecords |
            Where-Object Id -Match '^M:.+\.op_CheckedExplicit\(.*\)~.+$'
    )
    if ($checkedRecords.Count -eq 0) {
        return [PSCustomObject]@{
            ApiInput = $ApiInput
            TemporaryXml = $null
        }
    }

    $document = [System.Xml.XmlDocument]::new()
    $document.PreserveWhitespace = $true
    $document.Load([System.IO.Path]::GetFullPath($ApiInput.Xml))
    $members = $document.SelectSingleNode('/doc/members')
    if ($null -eq $members) {
        return [PSCustomObject]@{
            ApiInput = $ApiInput
            TemporaryXml = $null
        }
    }

    $memberNodes = @($document.SelectNodes('/doc/members/member'))
    $aliases = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    $addedAlias = $false
    foreach ($record in $checkedRecords) {
        $separator = $record.Id.LastIndexOf("~")
        $alias = $record.Id.Substring(0, $separator)
        if (-not $aliases.Add($alias)) {
            continue
        }
        $matches = @(
            $checkedRecords |
                Where-Object {
                    $_.Id.StartsWith(
                        "$alias~",
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($matches.Count -ne 1) {
            throw (
                "Checked conversion XML alias $alias matched " +
                "$($matches.Count) assembly-derived IDs."
            )
        }

        $canonicalId = $matches[0].Id
        $aliasNodes = @(
            $memberNodes |
                Where-Object {
                    [string]::Equals(
                        $_.GetAttribute("name"),
                        $alias,
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($aliasNodes.Count -gt 0) {
            throw "Checked conversion XML alias already exists: $alias"
        }
        $canonicalNodes = @(
            $memberNodes |
                Where-Object {
                    [string]::Equals(
                        $_.GetAttribute("name"),
                        $canonicalId,
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($canonicalNodes.Count -eq 0) {
            continue
        }
        if ($canonicalNodes.Count -ne 1) {
            throw "Checked conversion XML ID is duplicated: $canonicalId"
        }

        $aliasNode = $canonicalNodes[0].CloneNode($true)
        $aliasNode.SetAttribute("name", $alias)
        if ($members.LastChild.NodeType -notin @(
                [System.Xml.XmlNodeType]::Whitespace,
                [System.Xml.XmlNodeType]::SignificantWhitespace
            )) {
            [void]$members.AppendChild($document.CreateWhitespace("`n    "))
        }
        [void]$members.AppendChild($aliasNode)
        [void]$members.AppendChild($document.CreateWhitespace("`n    "))
        $memberNodes += $aliasNode
        $addedAlias = $true
    }

    if (-not $addedAlias) {
        return [PSCustomObject]@{
            ApiInput = $ApiInput
            TemporaryXml = $null
        }
    }

    $temporaryXml = Join-Path (
        [System.IO.Path]::GetTempPath()
    ) "humanizer-defaultdocumentation-$([guid]::NewGuid().ToString('N')).xml"
    $document.Save($temporaryXml)
    return [PSCustomObject]@{
        ApiInput = [PSCustomObject]@{
            Dll = $ApiInput.Dll
            Xml = $temporaryXml
        }
        TemporaryXml = $temporaryXml
    }
}

function Initialize-ApiAccessReader {
    if ($null -ne ("Humanizer.Docs.ApiAccessReader" -as [type])) {
        return
    }

    Add-Type -Path (Join-Path $PSScriptRoot "api-access.cs")
}

function Get-AssemblyApiMemberInventory {
    param([Parameter(Mandatory = $true)][string]$AssemblyPath)

    Initialize-ApiAccessReader
    $records = @(
        [Humanizer.Docs.ApiAccessReader]::Read(
            [System.IO.Path]::GetFullPath($AssemblyPath)
        )
    )
    if ($records.Count -eq 0) {
        throw "The reference assembly has no public or protected API records."
    }

    $types = @{}
    foreach ($record in $records | Where-Object Kind -eq "Type") {
        $types[$record.TypeId] = [PSCustomObject]@{
            Access = $record.Access
            Constructors = [System.Collections.Generic.List[object]]::new()
            DisplayName = $record.DisplayName
            Kind = $record.TypeKind
            Namespace = $record.Namespace
            PageName = $record.PageName
            QualifiedDisplayName = $record.QualifiedDisplayName
            RelativeDisplayName = if ([string]::IsNullOrEmpty(
                    $record.Namespace
                )) {
                $record.QualifiedDisplayName
            } else {
                $record.QualifiedDisplayName.Substring(
                    $record.Namespace.Length + 1
                )
            }
            TypeId = $record.TypeId
        }
    }
    foreach ($record in $records | Where-Object Kind -eq "Constructor") {
        if (-not $types.ContainsKey($record.TypeId)) {
            throw "Assembly API member has no declaring type: $($record.Id)"
        }
        $types[$record.TypeId].Constructors.Add($record)
    }

    return [PSCustomObject]@{
        Records = $records
        Types = $types
    }
}

function Select-AssemblyApiRecords {
    param(
        [Parameter(Mandatory = $true)]$Inventory,
        [switch]$PublicOnly
    )

    return @(
        $Inventory.Records |
            Where-Object {
                -not $PublicOnly -or $_.Access -eq "public"
            }
    )
}

function Select-AssemblyApiTypes {
    param(
        [Parameter(Mandatory = $true)]$Inventory,
        [switch]$PublicOnly
    )

    $selected = @{}
    foreach ($typeId in $Inventory.Types.Keys) {
        $type = $Inventory.Types[$typeId]
        if ($PublicOnly -and $type.Access -ne "public") {
            continue
        }
        $selected[$typeId] = [PSCustomObject]@{
            Access = $type.Access
            Constructors = @(
                $type.Constructors |
                    Where-Object {
                        -not $PublicOnly -or $_.Access -eq "public"
                    }
            )
            DisplayName = $type.DisplayName
            Kind = $type.Kind
            Namespace = $type.Namespace
            PageName = $type.PageName
            QualifiedDisplayName = $type.QualifiedDisplayName
            RelativeDisplayName = $type.RelativeDisplayName
            TypeId = $type.TypeId
        }
    }
    return $selected
}

function Get-ApiLinkRecords {
    param([Parameter(Mandatory = $true)][string]$LinksPath)

    return @(
        Get-Content $LinksPath |
            Select-Object -Skip 1 |
            ForEach-Object {
                $parts = @($_ -split "\|", 3)
                if ($parts.Count -ne 3) {
                    throw "Generated a malformed API link: $_"
                }
                $target = @($parts[1] -split "#", 2)
                [PSCustomObject]@{
                    Id = $parts[0]
                    Label = $parts[2]
                    Page = $target[0]
                    Target = $parts[1]
                }
            }
    )
}

function ConvertTo-ApiMarkdownTitle {
    param([Parameter(Mandatory = $true)][string]$Value)

    return $Value.
        Replace("\", "\\").
        Replace(".", "\.").
        Replace("<", "\<").
        Replace(">", "\>")
}

function Set-ApiCanonicalTypeRoutes {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)]$AssemblyTypes,
        [Parameter(Mandatory = $true)]$ExpectedRecords
    )

    $lines = [System.Collections.Generic.List[string]]::new()
    $lines.AddRange([string[]](Get-Content $LinksPath))
    $renames = @{}
    $generatedTypes = [System.Collections.Generic.List[object]]::new()
    for ($index = 1; $index -lt $lines.Count; $index++) {
        $parts = @($lines[$index] -split "\|", 3)
        if ($parts.Count -ne 3) {
            throw "Generated a malformed API link: $($lines[$index])"
        }
        if ($parts[0] -match '^M:.+\.op_CheckedExplicit\(.*\)$') {
            $expectedPrefix = "$($parts[0])~"
            $matches = @(
                $ExpectedRecords |
                    Where-Object {
                        $_.Id.StartsWith(
                            $expectedPrefix,
                            [System.StringComparison]::Ordinal
                        )
                    }
            )
            if ($matches.Count -ne 1) {
                throw (
                    "Generated checked conversion ID $($parts[0]) matched " +
                    "$($matches.Count) assembly-derived IDs."
                )
            }
            $parts[0] = $matches[0].Id
        }
        $target = @($parts[1] -split "#", 2)
        if ($renames.ContainsKey($target[0])) {
            $target[0] = $renames[$target[0]]
        }
        if ($parts[0].StartsWith("T:") -and
            $AssemblyTypes.ContainsKey($parts[0].Substring(2))) {
            $type = $AssemblyTypes[$parts[0].Substring(2)]
            $expectedPage = "$($type.PageName).md"
            if ($target[0] -ne $expectedPage) {
                if ($renames.ContainsKey($target[0]) -and
                    $renames[$target[0]] -ne $expectedPage) {
                    throw "Generated API types share a noncanonical page: $($target[0])"
                }
                $renames[$target[0]] = $expectedPage
                $target[0] = $expectedPage
            }
            $parts[2] = $type.RelativeDisplayName
            $generatedTypes.Add([PSCustomObject]@{
                Page = $expectedPage
                Type = $type
            })
        }
        if ($renames.ContainsKey($target[0])) {
            $target[0] = $renames[$target[0]]
        }
        $parts[1] = $target -join "#"
        $lines[$index] = $parts -join "|"
    }

    foreach ($generatedFile in Get-ChildItem $OutputPath -Filter "*.md" -File) {
        $content = Get-Content -Raw $generatedFile.FullName
        foreach ($oldPage in $renames.Keys) {
            $content = $content.Replace(
                $oldPage,
                $renames[$oldPage],
                [System.StringComparison]::Ordinal
            )
        }
        foreach ($generatedType in $generatedTypes) {
            $type = $generatedType.Type
            if ($type.Kind -ne "delegate") {
                continue
            }
            $pagePattern = [regex]::Escape($generatedType.Page)
            $namespaceLinkPattern = (
                "(?m)^(?<prefix>\| )\[[^\]]+\]\(" + $pagePattern +
                "(?: '[^']*')?\)(?<suffix> \|.*)$"
            )
            $label = [System.Net.WebUtility]::HtmlEncode(
                $type.RelativeDisplayName
            )
            $title = ConvertTo-ApiMarkdownTitle `
                -Value $type.QualifiedDisplayName
            $content = [regex]::Replace(
                $content,
                $namespaceLinkPattern,
                { param($match)
                    "$($match.Groups['prefix'].Value)[$label]($($generatedType.Page) '$title')$($match.Groups['suffix'].Value)"
                },
                1
            )
        }
        [System.IO.File]::WriteAllText(
            $generatedFile.FullName,
            $content,
            [System.Text.UTF8Encoding]::new($false)
        )
    }

    foreach ($oldPage in @($renames.Keys | Sort-Object)) {
        $oldPath = Join-Path $OutputPath $oldPage
        $newPath = Join-Path $OutputPath $renames[$oldPage]
        if (-not (Test-Path $oldPath -PathType Leaf)) {
            throw "Generated API route source is missing: $oldPage"
        }
        if (Test-Path $newPath) {
            throw "Generated API canonical route already exists: $($renames[$oldPage])"
        }
        Move-Item $oldPath $newPath
    }

    foreach ($generatedType in $generatedTypes) {
        $type = $generatedType.Type
        if ($type.Kind -ne "delegate") {
            continue
        }
        $pagePath = Join-Path $OutputPath $generatedType.Page
        $content = Get-Content -Raw $pagePath
        $headingDisplay = $type.RelativeDisplayName.
            Replace("<", "\<").
            Replace(">", "\>")
        $kindTitle = $type.Kind.Substring(0, 1).ToUpperInvariant() +
            $type.Kind.Substring(1)
        $content = [regex]::new(
            "(?m)^## .+ (?:Class|Struct|Interface|Enum|Delegate)\r?$"
        ).Replace(
            $content,
            "## $headingDisplay $kindTitle",
            1
        )
        [System.IO.File]::WriteAllText(
            $pagePath,
            $content,
            [System.Text.UTF8Encoding]::new($false)
        )
    }

    [System.IO.File]::WriteAllText(
        $LinksPath,
        "$($lines -join "`n")`n",
        [System.Text.UTF8Encoding]::new($false)
    )
}

function Add-ImplicitApiConstructors {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)]$ApprovedTypes
    )

    $links = @(Get-Content $LinksPath)
    $linkIds = [System.Collections.Generic.HashSet[string]]::new(
        [System.StringComparer]::Ordinal
    )
    $typePages = @{}
    foreach ($line in $links | Select-Object -Skip 1) {
        $parts = @($line -split "\|", 3)
        [void]$linkIds.Add($parts[0])
        if ($parts[0].StartsWith("T:")) {
            $typePages[$parts[0].Substring(2)] = (@(
                $parts[1] -split "#", 2
            ))[0]
        }
    }
    $newLinks = [System.Collections.Generic.List[string]]::new()
    foreach ($typeId in @($ApprovedTypes.Keys | Sort-Object)) {
        foreach ($constructor in @(
            $ApprovedTypes[$typeId].Constructors |
                Where-Object {
                    $_.ParameterCount -eq 0 -and
                    -not $linkIds.Contains($_.Id)
                }
        )) {
            if (-not $typePages.ContainsKey($typeId)) {
                throw "Approved parameterless constructor has no generated type page: $typeId"
            }
            $page = $typePages[$typeId]
            $pagePath = Join-Path $OutputPath $page
            $content = Get-Content -Raw $pagePath
            $pageName = [System.IO.Path]::GetFileNameWithoutExtension($page)
            $anchor = "$pageName.$($constructor.DisplayName)()"
            $escapedName = "$($constructor.DisplayName)\(\)"
            $toc = "- *Constructors*`n  - **[$escapedName]($page#$anchor)**`n"
            $tocPattern = (
                "(?m)^- \*(?:Fields|Properties|Methods|Events|Operators|" +
                "Explicit Interface Implementations)\*\r?$"
            )
            $hasExistingMembers = $content -match $tocPattern
            if ($hasExistingMembers) {
                $content = [regex]::new($tocPattern).Replace(
                    $content,
                    { param($match) "$toc$($match.Value)" },
                    1
                )
            } else {
                $content = "$($content.TrimEnd())`n$toc"
            }
            $definition = @"
### Constructors

<a name='$anchor'></a>

## $escapedName Constructor

Initializes a new instance of the $($constructor.DisplayName) class.

``````csharp
$($constructor.DeclaredAccess) $($constructor.DisplayName)();
``````

"@
            $detailPattern = (
                "(?m)^### (?:Fields|Properties|Methods|Events|Operators|" +
                "Explicit Interface Implementations)\r?$"
            )
            if ($hasExistingMembers) {
                if ($content -notmatch $detailPattern) {
                    throw "Implicit constructor page has no generated detail insertion point: $page"
                }
                $content = [regex]::new($detailPattern).Replace(
                    $content,
                    { param($match) "$definition$($match.Value)" },
                    1
                )
            } else {
                $content = "$($content.TrimEnd())`n$definition"
            }
            [System.IO.File]::WriteAllText(
                $pagePath,
                $content,
                [System.Text.UTF8Encoding]::new($false)
            )
            $newLinks.Add(
                "$($constructor.Id)|$page#$anchor|$($constructor.DisplayName)()"
            )
        }
    }
    if ($newLinks.Count -gt 0) {
        $newLinks.Sort([StringComparer]::Ordinal)
        [System.IO.File]::WriteAllText(
            $LinksPath,
            "$(($links + @($newLinks)) -join "`n")`n",
            [System.Text.UTF8Encoding]::new($false)
        )
    }
}

function Set-ApiMarkdownStructure {
    param([Parameter(Mandatory = $true)][string]$OutputPath)

    $memberPattern = (
        "^## (?<member>.+ (?:Constructor|Field|Property|Method|Operator|" +
        "Event|Explicit Interface Implementation))$"
    )
    $categoryPattern = (
        "^### (?:Constructors|Fields|Properties|Methods|Events|Operators|" +
        "Explicit Interface Implementations)$"
    )
    foreach ($generatedFile in Get-ChildItem $OutputPath -Filter "*.md" -File) {
        $lines = [System.IO.File]::ReadAllLines($generatedFile.FullName)
        $inMember = $false
        for ($index = 0; $index -lt $lines.Count; $index++) {
            if ($lines[$index] -match (
                    "^\| (?<category>Classes|Structs|Interfaces|Enums|Delegates) \| \|$"
                )) {
                $lines[$index] = "| $($Matches['category']) | Summary |"
                continue
            }
            if ($lines[$index] -match "^#{3,5} Type parameters$") {
                $lines[$index] = if ($inMember) {
                    "##### Type parameters"
                } else {
                    "### Type parameters"
                }
                continue
            }
            if ($lines[$index] -match $memberPattern) {
                $lines[$index] = "#### $($Matches['member'])"
                $inMember = $true
                continue
            }
            if ($lines[$index] -match $categoryPattern -or
                $lines[$index] -match "^## .+ (?:Class|Struct|Interface|Enum|Delegate)$") {
                $inMember = $false
                continue
            }
            if ($inMember -and $lines[$index] -match "^#{3,4} (?<heading>.+)$") {
                $lines[$index] = "##### $($Matches['heading'])"
            }
        }
        [System.IO.File]::WriteAllText(
            $generatedFile.FullName,
            "$(($lines -join "`n"))`n",
            [System.Text.UTF8Encoding]::new($false)
        )
    }
}

function Set-ApiTypeMetadata {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)]$AssemblyTypes
    )

    foreach ($line in Get-Content $LinksPath | Select-Object -Skip 1) {
        $parts = @($line -split "\|", 3)
        if (-not $parts[0].StartsWith("T:")) {
            continue
        }
        $typeId = $parts[0].Substring(2)
        if (-not $AssemblyTypes.ContainsKey($typeId)) {
            throw "Generated type page has no assembly identity: $typeId"
        }

        $relativePage = (@($parts[1] -split "#", 2))[0]
        $pagePath = Join-Path $OutputPath $relativePage
        $content = Get-Content -Raw $pagePath
        if ($content -notmatch (
                "(?m)^## .+ " +
                "(?:Class|Struct|Interface|Enum|Delegate)\r?$"
            )) {
            throw "Generated type page has no display heading: $relativePage"
        }

        $title = $AssemblyTypes[$typeId].QualifiedDisplayName
        $quotedTitle = $title.Replace("'", "''")
        $frontMatter = @"
---
title: '$quotedTitle'
sidebar_label: '$quotedTitle'
description: 'API reference for $quotedTitle.'
---
"@
        [System.IO.File]::WriteAllText(
            $pagePath,
            "$frontMatter`n$content",
            [System.Text.UTF8Encoding]::new($false)
        )
    }
}

function Assert-GeneratedApiMemberCompleteness {
    param(
        [Parameter(Mandatory = $true)]$ExpectedRecords,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $generated = @(
        Get-ApiLinkRecords -LinksPath $LinksPath |
            Where-Object Id -Match "^[TEFMP]:"
    )
    $expectedById = @{}
    foreach ($record in $ExpectedRecords) {
        $expectedById[$record.Id] = $record
    }
    $generatedById = @{}
    foreach ($record in $generated) {
        $generatedById[$record.Id] = $record
    }

    $differences = [System.Collections.Generic.List[string]]::new()
    foreach ($id in @($expectedById.Keys | Sort-Object)) {
        if (-not $generatedById.ContainsKey($id)) {
            $differences.Add("missing $id")
            continue
        }
        $expectedPage = "$($expectedById[$id].PageName).md"
        if ($generatedById[$id].Page -ne $expectedPage) {
            $differences.Add(
                "$id route expected=$expectedPage generated=$($generatedById[$id].Page)"
            )
        }
    }
    foreach ($id in @($generatedById.Keys | Sort-Object)) {
        if (-not $expectedById.ContainsKey($id)) {
            $differences.Add("unexpected $id")
        }
    }
    if ($differences.Count -gt 0) {
        throw "$VersionLabel generated API differs from its assembly-derived exact inventory: $($differences -join '; ')"
    }
}

function Assert-ApiHeadingOwnership {
    param(
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$VersionLabel
    )

    $memberPattern = (
        "^(?<level>#+) .+ (?:Constructor|Field|Property|Method|Operator|" +
        "Event|Explicit Interface Implementation)$"
    )
    $categoryPattern = (
        "^(?<level>#+) (?:Constructors|Fields|Properties|Methods|Events|" +
        "Operators|Explicit Interface Implementations)$"
    )
    $memberCount = 0
    $typeParameterCount = 0
    foreach ($generatedFile in Get-ChildItem $OutputPath -Filter "*.md" -File) {
        $content = Get-Content -Raw $generatedFile.FullName
        if ($content -notmatch "(?m)^## .+ (?:Class|Struct|Interface|Enum|Delegate)$") {
            continue
        }
        $inMember = $false
        foreach ($line in [System.IO.File]::ReadAllLines($generatedFile.FullName)) {
            if ($line -match "^(?<level>#+) Type parameters$") {
                if ($inMember) {
                    if ($Matches["level"].Length -ne 5) {
                        throw "$VersionLabel API member type parameters have the wrong heading level in $($generatedFile.Name): $line"
                    }
                    continue
                }
                if ($Matches["level"].Length -ne 3) {
                    throw "$VersionLabel API type parameters have the wrong heading level in $($generatedFile.Name): $line"
                }
                $typeParameterCount++
                $inMember = $false
                continue
            }
            if ($line -match $categoryPattern) {
                if ($Matches["level"].Length -ne 3) {
                    throw "$VersionLabel API category has the wrong heading level in $($generatedFile.Name): $line"
                }
                $inMember = $false
                continue
            }
            if ($line -match $memberPattern) {
                if ($Matches["level"].Length -ne 4) {
                    throw "$VersionLabel API member has the wrong heading level in $($generatedFile.Name): $line"
                }
                $memberCount++
                $inMember = $true
                continue
            }
            if ($inMember -and $line -match "^(?<level>#{2,6}) ") {
                if ($Matches["level"].Length -ne 5) {
                    throw "$VersionLabel API member subsection has the wrong heading level in $($generatedFile.Name): $line"
                }
            }
        }
    }
    if ($memberCount -eq 0) {
        throw "$VersionLabel API heading-ownership check found no generated members."
    }
    if ($typeParameterCount -eq 0) {
        throw "$VersionLabel API heading-ownership check found no generic type parameters."
    }
}

function Invoke-ApiReferenceGeneration {
    param(
        [Parameter(Mandatory = $true)]$ApiInput,
        [Parameter(Mandatory = $true)][string]$OutputPath,
        [Parameter(Mandatory = $true)][string]$LinksPath,
        [Parameter(Mandatory = $true)]$AssemblyInventory,
        [Parameter(Mandatory = $true)][string]$RepositoryRoot,
        [Parameter(Mandatory = $true)][string]$VersionLabel,
        [string]$ConfigurationFilePath,
        [ValidateSet("Api", "Public")][string]$AccessModifiers = "Api"
    )

    $expectedRecords = Select-AssemblyApiRecords `
        -Inventory $AssemblyInventory `
        -PublicOnly:($AccessModifiers -eq "Public")
    Invoke-DefaultDocumentationApi `
        -ApiInput $ApiInput `
        -ExpectedRecords $expectedRecords `
        -OutputPath $OutputPath `
        -LinksPath $LinksPath `
        -RepositoryRoot $RepositoryRoot `
        -ConfigurationFilePath $ConfigurationFilePath `
        -AccessModifiers $AccessModifiers
    if (-not $ConfigurationFilePath) {
        return
    }

    $expectedTypes = Select-AssemblyApiTypes `
        -Inventory $AssemblyInventory `
        -PublicOnly:($AccessModifiers -eq "Public")
    Set-ApiCanonicalTypeRoutes `
        -OutputPath $OutputPath `
        -LinksPath $LinksPath `
        -AssemblyTypes $expectedTypes `
        -ExpectedRecords $expectedRecords
    Add-ImplicitApiConstructors `
        -OutputPath $OutputPath `
        -LinksPath $LinksPath `
        -ApprovedTypes $expectedTypes
    Set-ApiMarkdownStructure -OutputPath $OutputPath
    Set-ApiTypeMetadata `
        -OutputPath $OutputPath `
        -LinksPath $LinksPath `
        -AssemblyTypes $expectedTypes
    Assert-GeneratedApiMemberCompleteness `
        -ExpectedRecords $expectedRecords `
        -LinksPath $LinksPath `
        -VersionLabel $VersionLabel
    Assert-ApiHeadingOwnership `
        -OutputPath $OutputPath `
        -VersionLabel $VersionLabel
}
