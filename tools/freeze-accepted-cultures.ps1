param(
    [Parameter(Mandatory)]
    [string] $RepositoryRoot
)

$localeCodes = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
Get-ChildItem (Join-Path $RepositoryRoot 'src/Humanizer/Locales') -Filter '*.yml' |
    ForEach-Object { [void] $localeCodes.Add($_.BaseName) }

$rows = foreach ($culture in [System.Globalization.CultureInfo]::GetCultures([System.Globalization.CultureTypes]::AllCultures)) {
    if ([string]::IsNullOrEmpty($culture.Name)) {
        continue
    }

    for ($current = $culture; -not [string]::IsNullOrEmpty($current.Name); $current = $current.Parent) {
        if ($localeCodes.Contains($current.Name)) {
            [pscustomobject]@{
                AcceptedName = $culture.Name
                LocaleProfileOwner = $current.Name
            }
            break
        }
    }
}

$rows |
    Sort-Object AcceptedName |
    ConvertTo-Json -Depth 2
