param(
    [string]$target = "Test",
    [string]$verbosity = "minimal",    
    [int]$maxCpuCount = 0
)

$msbuild = "msbuild.exe"

# Kill all MSBUILD.EXE processes because they could very likely have a lock against our
# MSBuild runner from when we last ran unit tests.
get-process -name "msbuild" -ea SilentlyContinue | %{ stop-process $_.ID -force }

if ($maxCpuCount -lt 1) {
    $maxCpuCountText = $Env:MSBuildProcessorCount
} else {
    $maxCpuCountText = ":$maxCpuCount"
}


$allArgs = @("build.proj", "/m$maxCpuCountText", "/nologo", "/verbosity:$verbosity", "/t:$target", "/property:RequestedVerbosity=$verbosity", $args)
& $msbuild $allArgs
