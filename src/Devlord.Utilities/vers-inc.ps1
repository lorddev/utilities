#
# This script will increment the build number in an AssemblyInfo.cs file
# er, I mean csproj
# Inspired by https://gist.github.com/bradjolicoeur/e77c508089aea6614af3
#

$assemblyInfoPath = $args[0] 

$contents = [System.IO.File]::ReadAllText($assemblyInfoPath)

$versionString = [RegEx]::Match($contents,"(<Version>)(?:\d+\.\d+\.\d+\.\d+)(<\/Version>)")
Write-Host ("AssemblyVersion: " +$versionString.Value)

#Parse out the current build number from the AssemblyFileVersion
$currentBuild = [RegEx]::Match($versionString,"(\.)(\d+)(<\/)").Groups[2]
Write-Host ("Current Build: " + $currentBuild.Value) 

#Increment the build number
$newBuild= [int]$currentBuild.Value +  1
Write-Host ("New Build: " + $newBuild)

#update AssemblyFileVersion and AssemblyVersion, then write to file
Write-Host ("Setting version in assembly info file ")
$contents = [RegEx]::Replace($contents, "(<Version>\d+\.\d+\.\d+\.)(?:\d+)(<\/Version>)", ("`${1}" + $newBuild.ToString() + "`${2}"))

[System.IO.File]::WriteAllText($assemblyInfoPath, $contents)