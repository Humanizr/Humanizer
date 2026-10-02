function New-DefaultDocumentationApiInput {
    param(
        [Parameter(Mandatory = $true)]$ApiInput,
        [Parameter(Mandatory = $true)]$ExpectedRecords
    )

    $xmlAliases = @(
        foreach ($record in $ExpectedRecords) {
            if ($record.Id -match '^M:.+\.op_CheckedExplicit\(.*\)~.+$') {
                [PSCustomObject]@{
                    CanonicalId = $record.Id
                    Alias = $record.Id.Substring(0, $record.Id.LastIndexOf("~"))
                    Label = "Checked conversion"
                }
            } elseif ($record.Kind -ceq "Method" -and
                $record.Name -cin @("op_Implicit", "op_Explicit", "op_CheckedExplicit")) {
                if ([string]::IsNullOrWhiteSpace($record.MethodReturnType)) {
                    throw "Conversion method XML return type is missing: $($record.Id)"
                }
                [PSCustomObject]@{
                    CanonicalId = $record.Id
                    Alias = "$($record.Id)~$($record.MethodReturnType)"
                    Label = "Conversion method"
                }
            }
        }
    )
    if ($xmlAliases.Count -eq 0) {
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
    $referenceAliases = [System.Collections.Generic.Dictionary[string, string]]::new(
        [System.StringComparer]::Ordinal
    )
    foreach ($record in $xmlAliases) {
        $alias = $record.Alias
        if (-not $aliases.Add($alias)) {
            continue
        }
        $matches = @(
            $xmlAliases |
                Where-Object {
                    [string]::Equals(
                        $_.Alias,
                        $alias,
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($matches.Count -ne 1) {
            throw (
                "$($record.Label) XML alias $alias matched " +
                "$($matches.Count) assembly-derived IDs."
            )
        }

        $canonicalId = $matches[0].CanonicalId
        $assemblyAliasMatches = @(
            $ExpectedRecords |
                Where-Object {
                    [string]::Equals(
                        $_.Id,
                        $alias,
                        [System.StringComparison]::Ordinal
                    )
                }
        )
        if ($assemblyAliasMatches.Count -gt 0) {
            throw "$($record.Label) XML alias collides with an assembly ID: $alias"
        }
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
            throw "$($record.Label) XML alias already exists: $alias"
        }
        if ($alias.StartsWith("$canonicalId~", [System.StringComparison]::Ordinal)) {
            $referenceAliases.Add($canonicalId, $alias)
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
            throw "$($record.Label) XML ID is duplicated: $canonicalId"
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

    foreach ($reference in $document.SelectNodes('//*[@cref]')) {
        $referenceId = $reference.GetAttribute("cref")
        if ($referenceAliases.ContainsKey($referenceId)) {
            $reference.SetAttribute("cref", $referenceAliases[$referenceId])
            $addedAlias = $true
        }
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
