function Assert-GeneratedExtraTypes {
    param(
        [Parameter(Mandatory = $true)][string]$VersionLabel,
        [string[]]$Actual = @(),
        [string[]]$Expected = @()
    )

    $actualUnique = @($Actual | Sort-Object -Unique)
    $expectedUnique = @($Expected | Sort-Object -Unique)
    if ($Actual.Count -ne $actualUnique.Count -or
        $Expected.Count -ne $expectedUnique.Count -or
        ($actualUnique -join "`n") -ne ($expectedUnique -join "`n")) {
        throw "$VersionLabel generated PublicAPI extras differ. Actual: $($actualUnique -join ', '); expected: $($expectedUnique -join ', ')."
    }
}
