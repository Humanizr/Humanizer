param(
    [string]$target = "Test",
    [string]$verbosity = "minimal",    
    [int]$maxCpuCount = 0
)


# Kill all MSBUILD.EXE processes because they could very likely have a lock against our
# MSBuild runner from when we last ran unit tests.
get-process -name "msbuild" -ea SilentlyContinue | %{ stop-process $_.ID -force }



    if (test-path "env:\ProgramFiles(x86)") {
        $path = join-path ${env:ProgramFiles(x86)} "MSBuild\14.0\bin\MSBuild.exe"
        if (test-path $path) {
            $msbuild = $path
        }
    }
    if ($msbuild -eq $null) {
        $path = join-path $env:ProgramFiles "MSBuild\14.0\bin\MSBuild.exe"
        if (test-path $path) {
            $msbuild = $path
        }
    }
    if ($msbuild -eq $null) {
        throw "MSBuild could not be found in the path. Please ensure MSBuild v14 (from Visual Studio 2015) is in the path."
    }


if ($maxCpuCount -lt 1) {
    $maxCpuCountText = $Env:MSBuildProcessorCount
} else {
    $maxCpuCountText = ":$maxCpuCount"
}


$allArgs = @("build.proj", "/m$maxCpuCountText", "/nologo", "/verbosity:$verbosity", "/t:$target", "/property:RequestedVerbosity=$verbosity", $args)
& $msbuild $allArgs
