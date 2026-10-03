param(
    [Parameter(Mandatory)][string]$ResultsDirectory,
    [Parameter(Mandatory)][ValidateSet('Windows', 'WindowsNLS', 'LinuxICU')][string]$Platform,
    [Parameter(Mandatory)][ValidateSet('ICU', 'NLS')][string]$ExpectedModernProvider
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
if (($Platform -eq 'WindowsNLS' -and $ExpectedModernProvider -ne 'NLS') -or ($Platform -ne 'WindowsNLS' -and $ExpectedModernProvider -ne 'ICU')) { throw 'Platform and expected provider do not match.' }
$root = (Resolve-Path -LiteralPath $ResultsDirectory).ProviderPath
$frameworks = @('net8.0', 'net10.0', 'net11.0')
if ($Platform -ne 'LinuxICU') { $frameworks += 'net48' }
$expected = @{}
foreach ($framework in $frameworks) { $expected["Humanizer.Tests|$framework"] = $true }
foreach ($framework in @('net10.0', 'net11.0')) { $expected["Humanizer.SourceGenerators.Tests|$framework"] = $true }
foreach ($roslyn in @('38', '48', '414')) { $expected["Humanizer.Analyzers.Tests.Roslyn$roslyn|net11.0"] = $true }
$targetFrameworks = @{
    '.NETCoreApp,Version=v8.0' = 'net8.0'
    '.NETCoreApp,Version=v10.0' = 'net10.0'
    '.NETCoreApp,Version=v11.0' = 'net11.0'
    '.NETFramework,Version=v4.8' = 'net48'
}
$files = @(Get-ChildItem -LiteralPath $root -Recurse -File)
foreach ($file in $files) {
    if ($file.LinkType) { throw "Linked report is not allowed: $($file.FullName)" }
}
if (@($files | Where-Object Name -Like '*.provider.*.txt.tmp').Count) { throw 'Incomplete provider attestation write.' }
$attestations = @{}
foreach ($file in @($files | Where-Object Name -Like '*.provider.*.txt')) {
    $values = @{}
    foreach ($line in Get-Content -LiteralPath $file.FullName) {
        $parts = $line.Split('=', 2)
        if ($parts.Count -ne 2 -or $values.ContainsKey($parts[0])) { throw "Malformed provider attestation: $($file.Name)" }
        $values[$parts[0]] = $parts[1]
    }
    $keys = @('assembly', 'fullName', 'framework', 'mvid', 'process', 'runtime', 'provider', 'expected', 'invariant')
    if ($values.Count -ne $keys.Count -or @($keys | Where-Object { !$values.ContainsKey($_) }).Count) { throw "Incomplete provider attestation: $($file.Name)" }
    if (!$targetFrameworks.ContainsKey($values.framework)) { throw "Unexpected attested framework: $($values.framework)" }
    $framework = $targetFrameworks[$values.framework]
    $identity = "$($values.assembly)|$framework"
    if (!$expected.ContainsKey($identity) -or $attestations.ContainsKey($identity)) { throw "Unexpected or duplicate provider module: $identity" }
    if ($file.Name -notmatch ('^' + [regex]::Escape($values.assembly) + '\.provider\.[0-9a-f]{32}\.txt$')) { throw 'Provider filename does not match assembly.' }
    $assembly = [Reflection.AssemblyName]::new($values.fullName)
    if ($assembly.Name -ne $values.assembly -or [guid]::Parse($values.mvid) -eq [guid]::Empty -or [int]$values.process -le 0) { throw 'Invalid actual assembly/process identity.' }
    $null = [version]::Parse($values.runtime)
    $provider = if ($framework -eq 'net48') { 'NLS' } else { $ExpectedModernProvider }
    if ($values.provider -ne $provider -or $values.expected -ne $provider -or $values.invariant -ne 'false') { throw "Provider or invariant mismatch: $identity" }
    $attestations[$identity] = $values
}
$reports = @{}
foreach ($file in @($files | Where-Object Extension -EQ '.trx')) {
    if ($file.Name -notmatch '^(Humanizer\.(?:Tests|SourceGenerators\.Tests|Analyzers\.Tests\.Roslyn(?:38|48|414)))_(net8\.0|net10\.0|net11\.0|\.NET_Framework_4\.8)_x64\.trx$') { throw "Unexpected report filename: $($file.Name)" }
    $assembly = $Matches[1]
    $framework = if ($Matches[2] -eq '.NET_Framework_4.8') { 'net48' } else { $Matches[2] }
    $identity = "$assembly|$framework"
    if (!$expected.ContainsKey($identity) -or $reports.ContainsKey($identity)) { throw "Unexpected or duplicate report module: $identity" }
    $settings = [Xml.XmlReaderSettings]::new()
    $settings.DtdProcessing = [Xml.DtdProcessing]::Prohibit
    $settings.XmlResolver = $null
    $reader = [Xml.XmlReader]::Create($file.FullName, $settings)
    try { $xml = [Xml.XmlDocument]::new(); $xml.XmlResolver = $null; $xml.Load($reader) } finally { $reader.Dispose() }
    $ns = [Xml.XmlNamespaceManager]::new($xml.NameTable)
    $ns.AddNamespace('t', 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010')
    $counters = $xml.SelectNodes('/t:TestRun/t:ResultSummary/t:Counters', $ns)
    $summary = $xml.SelectSingleNode('/t:TestRun/t:ResultSummary', $ns)
    if ($counters.Count -ne 1 -or $summary.outcome -ne 'Completed') { throw "Incomplete test summary: $identity" }
    $counter = $counters[0]
    foreach ($failure in @('failed', 'error', 'timeout', 'aborted', 'inconclusive', 'passedButRunAborted', 'notRunnable', 'disconnected', 'inProgress', 'pending')) {
        if (!$counter.HasAttribute($failure) -or [long]$counter.GetAttribute($failure) -ne 0) { throw "Nonpassed test state $failure in $identity" }
    }
    $results = $xml.SelectNodes('/t:TestRun/t:Results/t:UnitTestResult', $ns)
    $definitions = $xml.SelectNodes('/t:TestRun/t:TestDefinitions/t:UnitTest', $ns)
    $definitionMap = @{}
    foreach ($definition in $definitions) {
        $id = $definition.id
        if ($definitionMap.ContainsKey($id)) { throw "Duplicate test definition: $identity" }
        $method = $definition.SelectSingleNode('t:TestMethod', $ns)
        if (!$method -or ($method.codeBase -split '[/\\]')[-1] -ne "$assembly.$(if ($framework -eq 'net48') { 'exe' } else { 'dll' })") { throw "Wrong report assembly: $identity" }
        $segments = $method.codeBase -split '[/\\]'
        if ($segments -contains '..' -or ($assembly -notlike 'Humanizer.Analyzers.Tests.*' -and !($segments -contains "release_$framework"))) { throw "Wrong report framework or traversal path: $identity" }
        $definitionMap[$id] = $method
    }
    $passed = 0; $skipped = 0; $fact = 0; $seenTests = @{}; $seenExecutions = @{}
    foreach ($result in $results) {
        if (!$definitionMap.ContainsKey($result.testId) -or !$result.HasAttribute('executionId')) { throw "Missing result definition or execution identity: $identity" }
        $execution = [guid]::Parse($result.executionId)
        if ($execution -eq [guid]::Empty -or $seenExecutions.ContainsKey($execution)) { throw "Duplicate result execution: $identity" }
        $seenExecutions[$execution] = $true
        $seenTests[$result.testId] = $true
        if ($result.outcome -eq 'Passed') { $passed++ } elseif ($result.outcome -eq 'NotExecuted') { $skipped++ } else { throw "Nonpassed test outcome: $identity" }
        $method = $definitionMap[$result.testId]
        if ($method.className -eq 'Humanizer.Tests.Infrastructure.GlobalizationProviderTests' -and $method.name -eq 'RecordsActualGlobalizationProvider') {
            if ($result.outcome -ne 'Passed') { throw "Provider test did not pass: $identity" }
            $fact++
        }
    }
    if ($definitions.Count -ne $seenTests.Count) { throw "Partial test definitions/results: $identity" }
    if ($fact -ne 1 -or $passed -le 1 -or [long]$counter.passed -ne $passed -or [long]$counter.notExecuted -ne $skipped -or [long]$counter.total -ne $results.Count -or [long]$counter.executed -ne $passed) { throw "Empty, partial or inconsistent test counters: $identity" }
    if (!$attestations.ContainsKey($identity)) { throw "Missing provider attestation: $identity" }
    $reports[$identity] = $true
    Write-Output "Verified $identity passed=$passed skipped=$skipped provider=$($attestations[$identity].provider)"
}
if ($reports.Count -ne $expected.Count -or $attestations.Count -ne $expected.Count -or @($expected.Keys | Where-Object { !$reports.ContainsKey($_) }).Count) { throw "Missing expected test modules: expected $($expected.Count), found $($reports.Count)." }
Write-Output "Complete MTP inventory verified: $($expected.Count) modules."
