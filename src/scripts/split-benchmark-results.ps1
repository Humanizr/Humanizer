param(
  [Parameter(Mandatory)] [string]$InputDir,
  [string]$OutputRoot = "."
)

# ---- Editable short TFM map ----
# Keys are substrings or regexes you expect in the runtime name.
# Left to right match wins. Add or reorder as needed.
$TfmMap = @(
  @{ Pattern = '^\.NET\s+10(\.0)?$';   Tfm = 'net10.0' }
  @{ Pattern = '^\.NET\s+8(\.0)?$';    Tfm = 'net8.0'  }
  @{ Pattern = '^\.NET\s+Framework\s*4\.8$'; Tfm = 'net48'   }
)
$DefaultTfm = 'unknown'

# ---- Helpers ----
function Get-Runtime([object]$b) {
  # 1) Old path
  if ($b.PSObject.Properties.Name -contains 'Properties' -and $b.Properties.Runtime) { return [string]$b.Properties.Runtime }
  # 2) New nested path
  $rt = $b.BenchmarkCase.Job.Environment.Runtime.Name
  if ($rt) { return [string]$rt }
  # 3) Fallback from DisplayInfo, e.g. "...(Runtime=.NET 10.0)"
  $di = [string]$b.DisplayInfo
  if ($di -match 'Runtime=([^)]+)\)') { return $Matches[1].Trim() }
  return $null
}

function To-ShortTfm([string]$runtimeName) {
  foreach ($m in $TfmMap) {
    if ($runtimeName -match $m.Pattern) { return $m.Tfm }
  }
  return $DefaultTfm
}

# ---- Process all JSONs in directory ----
$files = Get-ChildItem -LiteralPath $InputDir -File -Filter *.json
if (-not $files) { throw "No .json files found in '$InputDir'." }

foreach ($f in $files) {
  try {
    $root = Get-Content -Raw -LiteralPath $f.FullName | ConvertFrom-Json -AsHashtable
    if (-not $root.ContainsKey('Benchmarks')) { Write-Warning "Skip '$($f.Name)': no 'Benchmarks'."; continue }

    $byRuntime = @{}
    foreach ($b in $root.Benchmarks) {
      $rt = Get-Runtime $b
      if (-not $rt) { Write-Warning "Skip one entry in '$($f.Name)': runtime not detected."; continue }
      if (-not $byRuntime.ContainsKey($rt)) { $byRuntime[$rt] = [System.Collections.ArrayList]::new() }
      [void]$byRuntime[$rt].Add($b)
    }

    if ($byRuntime.Count -eq 0) { Write-Warning "Skip '$($f.Name)': no entries with detectable runtime."; continue }

    foreach ($rt in $byRuntime.Keys) {
      $short = To-ShortTfm $rt
      $outDir = Join-Path $OutputRoot $short
      New-Item -ItemType Directory -Force -Path $outDir | Out-Null

      # Build per-runtime JSON preserving top-level fields except Benchmarks
      $out = [ordered]@{}
      foreach ($k in $root.Keys) { if ($k -ne 'Benchmarks') { $out[$k] = $root[$k] } }
      $out['Title']      = "{0} ({1})" -f $root['Title'], $rt
      $out['Benchmarks'] = $byRuntime[$rt]

      $json = $out | ConvertTo-Json -Depth 100
      $dest = Join-Path $outDir $f.Name
      Set-Content -LiteralPath $dest -Value $json -NoNewline
      Write-Host "Wrote $dest"
    }
  }
  catch {
    Write-Warning "Error processing '$($f.FullName)': $($_.Exception.Message)"
  }
}
