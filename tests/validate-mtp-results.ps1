$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$validator = Join-Path $PSScriptRoot '../tools/validate-mtp-results.ps1'
$root = Join-Path ([IO.Path]::GetTempPath()) ('humanizer-trx-fixtures-' + [guid]::NewGuid().ToString('N'))
$modules = @(
    @('Humanizer.Tests', 'net8.0'), @('Humanizer.Tests', 'net10.0'), @('Humanizer.Tests', 'net11.0'),
    @('Humanizer.SourceGenerators.Tests', 'net10.0'), @('Humanizer.SourceGenerators.Tests', 'net11.0'),
    @('Humanizer.Analyzers.Tests.Roslyn38', 'net11.0'), @('Humanizer.Analyzers.Tests.Roslyn48', 'net11.0'), @('Humanizer.Analyzers.Tests.Roslyn414', 'net11.0')
)
function New-Fixture([string]$name, [bool]$windows = $false) {
    $directory = Join-Path $root $name
    [IO.Directory]::CreateDirectory($directory) | Out-Null
    $selected = @($modules)
    if ($windows) { $selected += ,@('Humanizer.Tests', 'net48') }
    foreach ($module in $selected) {
        $assembly, $tfm = $module
        $token = if ($tfm -eq 'net48') { '.NET_Framework_4.8' } else { $tfm }
        $extension = if ($tfm -eq 'net48') { 'exe' } else { 'dll' }
        $framework = if ($tfm -eq 'net48') { '.NETFramework,Version=v4.8' } else { '.NETCoreApp,Version=v' + $tfm.Substring(3) }
        $provider = if ($windows) { 'NLS' } else { 'ICU' }
        $codeBase = "D:\a\1\s\artifacts\bin\$assembly\release_$tfm\$assembly.$extension"
        $xml = @"
<TestRun xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <Results><UnitTestResult testId="1" executionId="00000000-0000-0000-0000-000000000001" outcome="Passed"/><UnitTestResult testId="2" executionId="00000000-0000-0000-0000-000000000002" outcome="Passed"/></Results>
  <TestDefinitions>
    <UnitTest id="1"><TestMethod className="Humanizer.Tests.Infrastructure.GlobalizationProviderTests" name="RecordsActualGlobalizationProvider" codeBase="$codeBase"/></UnitTest>
    <UnitTest id="2"><TestMethod className="ExistingTests" name="KeepsExistingBehavior" codeBase="$codeBase"/></UnitTest>
  </TestDefinitions>
  <ResultSummary outcome="Completed"><Counters total="2" executed="2" passed="2" failed="0" error="0" timeout="0" aborted="0" inconclusive="0" passedButRunAborted="0" notRunnable="0" notExecuted="0" disconnected="0" inProgress="0" pending="0"/></ResultSummary>
</TestRun>
"@
        [IO.File]::WriteAllText((Join-Path $directory "$($assembly)_$($token)_x64.trx"), $xml)
        $lines = @("assembly=$assembly", "fullName=$assembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "framework=$framework", "mvid=$([guid]::NewGuid())", 'process=123', 'runtime=11.0.0', "provider=$provider", "expected=$provider", 'invariant=false')
        [IO.File]::WriteAllLines((Join-Path $directory "$assembly.provider.$([guid]::NewGuid().ToString('N')).txt"), $lines)
    }
    return $directory
}
function Invoke-Validation([string]$directory, [bool]$windows = $false) {
    $platform = if ($windows) { 'WindowsNLS' } else { 'LinuxICU' }
    $provider = if ($windows) { 'NLS' } else { 'ICU' }
    & $validator -ResultsDirectory $directory -Platform $platform -ExpectedModernProvider $provider | Out-Null
}
$passed = 0
try {
    Invoke-Validation (New-Fixture 'complete-linux'); $passed++
    Invoke-Validation (New-Fixture 'complete-windows' $true) $true; $passed++
    $mixed = New-Fixture 'complete-windows-icu' $true
    foreach ($file in Get-ChildItem $mixed -Filter '*.provider.*.txt') {
        $text = [IO.File]::ReadAllText($file.FullName)
        if (!$text.Contains('framework=.NETFramework')) {
            [IO.File]::WriteAllText($file.FullName, $text.Replace('provider=NLS', 'provider=ICU').Replace('expected=NLS', 'expected=ICU'))
        }
    }
    & $validator -ResultsDirectory $mixed -Platform Windows -ExpectedModernProvider ICU | Out-Null
    $passed++
    $repeated = New-Fixture 'repeated-theory-definition'
    $file = Join-Path $repeated 'Humanizer.Tests_net8.0_x64.trx'
    $text = [IO.File]::ReadAllText($file).Replace('</Results>', '<UnitTestResult testId="2" executionId="00000000-0000-0000-0000-000000000003" outcome="Passed"/></Results>').Replace('total="2"', 'total="3"').Replace('executed="2"', 'executed="3"').Replace('passed="2"', 'passed="3"')
    [IO.File]::WriteAllText($file, $text)
    Invoke-Validation $repeated; $passed++
    $mutations = [ordered]@{
        missing = { param($d) Remove-Item (Join-Path $d 'Humanizer.Tests_net8.0_x64.trx') }
        duplicateExecution = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('00000000-0000-0000-0000-000000000002', '00000000-0000-0000-0000-000000000001'))) }
        duplicate = { param($d) $nested = Join-Path $d 'nested'; New-Item -ItemType Directory $nested | Out-Null; Copy-Item (Join-Path $d 'Humanizer.Tests_net8.0_x64.trx') $nested }
        wrongFramework = { param($d) Rename-Item (Join-Path $d 'Humanizer.SourceGenerators.Tests_net10.0_x64.trx') 'Humanizer.SourceGenerators.Tests_net8.0_x64.trx' }
        empty = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, '') }
        failed = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('failed="0"', 'failed="1"'))) }
        zeroPass = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('passed="2"', 'passed="0"'))) }
        onlyAttestation = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; $s = [IO.File]::ReadAllText($f).Replace('<UnitTestResult testId="2" executionId="00000000-0000-0000-0000-000000000002" outcome="Passed"/>', '').Replace('total="2"', 'total="1"').Replace('executed="2"', 'executed="1"').Replace('passed="2"', 'passed="1"'); [IO.File]::WriteAllText($f, ([regex]::Replace($s, '<UnitTest id="2">.*?</UnitTest>', ''))) }
        malformed = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, '<TestRun>') }
        traversal = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, '<!DOCTYPE TestRun [<!ENTITY external SYSTEM "file:///nonexistent">]><TestRun>&external;</TestRun>') }
        wrongRuntimeFramework = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('release_net8.0', 'release_net10.0'))) }
        codeBaseTraversal = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('release_net8.0', '..'))) }
        wrongAssembly = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('Humanizer.Tests.dll', 'Other.dll'))) }
        missingProvider = { param($d) Get-ChildItem $d -Filter 'Humanizer.Tests.provider.*.txt' | Select-Object -First 1 | Remove-Item }
        duplicateProvider = { param($d) $f = Get-ChildItem $d -Filter 'Humanizer.Tests.provider.*.txt' | Select-Object -First 1; Copy-Item $f.FullName (Join-Path $d "Humanizer.Tests.provider.$([guid]::NewGuid().ToString('N')).txt") }
        wrongProvider = { param($d) $f = Get-ChildItem $d -Filter '*.provider.*.txt' | Select-Object -First 1; [IO.File]::WriteAllText($f.FullName, ([IO.File]::ReadAllText($f.FullName).Replace('provider=ICU', 'provider=NLS'))) }
        invariant = { param($d) $f = Get-ChildItem $d -Filter '*.provider.*.txt' | Select-Object -First 1; [IO.File]::WriteAllText($f.FullName, ([IO.File]::ReadAllText($f.FullName).Replace('invariant=false', 'invariant=true'))) }
        failedFact = { param($d) $f = Join-Path $d 'Humanizer.Tests_net8.0_x64.trx'; [IO.File]::WriteAllText($f, ([IO.File]::ReadAllText($f).Replace('executionId="00000000-0000-0000-0000-000000000001" outcome="Passed"', 'executionId="00000000-0000-0000-0000-000000000001" outcome="NotExecuted"'))) }
        partialWrite = { param($d) [IO.File]::WriteAllText((Join-Path $d 'Humanizer.Tests.provider.abc.txt.tmp'), 'incomplete') }
    }
    foreach ($entry in $mutations.GetEnumerator()) {
        $directory = New-Fixture $entry.Key
        & $entry.Value $directory
        $rejected = $false
        try { Invoke-Validation $directory } catch { $rejected = $true; Write-Output "Rejected $($entry.Key): $($_.Exception.Message)" }
        if (!$rejected) { throw "Invalid fixture was accepted: $($entry.Key)" }
        $passed++
    }
    Write-Output "MTP fixture checks passed: $passed"
} finally { Remove-Item -LiteralPath $root -Recurse -Force }
