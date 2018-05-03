$currentDirectory = split-path $MyInvocation.MyCommand.Definition

# See if we have the ClientSecret available
if([string]::IsNullOrEmpty($env:SignClientSecret)){
	Write-Host "Client Secret not found, not signing packages"
	return;
}

& nuget install SignClient -Version 0.9.1 -SolutionDir "$currentDirectory\..\" -Verbosity quiet -ExcludeVersion

# Setup Variables we need to pass into the sign client tool

$appSettings = "$currentDirectory\SignClient.json"

$appPath = "$currentDirectory\..\packages\SignClient\tools\netcoreapp2.0\SignClient.dll"

$nupgks = gci $Env:ArtifactDirectory\*.nupkg | Select -ExpandProperty FullName

foreach ($nupkg in $nupgks){
	Write-Host "Submitting $nupkg for signing"

	dotnet $appPath 'sign' -c $appSettings -i $nupkg -r $env:SignClientUser -s $env:SignClientSecret -n 'Humanizer' -d 'Humanizer' -u 'https://github.com/Humanizr/Humanizer' 

	Write-Host "Finished signing $nupkg"
}

Write-Host "Sign-package complete"