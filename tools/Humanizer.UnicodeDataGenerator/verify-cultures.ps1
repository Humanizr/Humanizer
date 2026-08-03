param(
    [Parameter(Mandatory)]
    [string] $UnicodeDataDirectory
)

$ErrorActionPreference = 'Stop'
$project = Join-Path $PSScriptRoot 'Humanizer.UnicodeDataGenerator.csproj'
$previousLang = $env:LANG
$previousLcAll = $env:LC_ALL

try {
    foreach ($culture in 'en_US.UTF-8', 'de_DE.UTF-8') {
        $env:LANG = $culture
        $env:LC_ALL = $culture
        dotnet run --project $project -- $UnicodeDataDirectory --check
        if ($LASTEXITCODE -ne 0) {
            throw "Unicode generation check failed under $culture."
        }
    }
}
finally {
    $env:LANG = $previousLang
    $env:LC_ALL = $previousLcAll
}
