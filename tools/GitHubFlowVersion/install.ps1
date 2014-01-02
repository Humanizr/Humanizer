param($installPath, $toolsPath, $package, $project)

$ErrorActionPreference = "Stop"

if ($project -ne $null)
{
    Foreach ($item in $project.ProjectItems)
    {
        if ($item.Name -eq "ToBeRemoved.txt")
        {
            $item.Delete()
        }
    }
}

$gitDir = $null
$workingDirectory = (Get-Item $project.FullName).Directory
Write-Host "Looking for .git directory, starting in $workingDirectory"
while ($true)
{
    $possibleGitDir = Join-Path $workingDirectory.FullName ".git"
    if (Test-Path $possibleGitDir)
    {
        $gitDir = $possibleGitDir
        Break
    }
    $parent = $workingDirectory.Parent
    if ($parent -eq $null)
    {
        Break
    }
    $workingDirectory = $parent;
}

if ($gitDir -ne $null)
{
    Write-Host "Found git directory for project at $gitDir"
    $repositoryDir = (get-item $gitDir -Force).Parent.FullName
    $gitHubFlowToolsDir = Join-Path $repositoryDir "tools\GitHubFlowVersion"
    if ((Test-Path $gitHubFlowToolsDir -PathType Container) -eq $false)
    {
        Write-Host "Creating directory $gitHubFlowToolsDir"
    }
    
    Write-Host "GitHubFlowVersion tools installed to $gitHubFlowToolsDir"
    Copy-Item $toolsPath –destination $gitHubFlowToolsDir -recurse -container -force
}
else
{
    Write-Host "Cannot find git directory"
}
