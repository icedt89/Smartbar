[string] $nupackageRepositoryDirectory = $PSScriptRoot + "\..\Deployment\Packages"
[string] $packageAuthoringModuleFileName = "JanHafner.Smartbar.PackageAuthoring.dll"
[string] $packageAuthoringModuleDirectory = $PSScriptRoot + "\..\NuGetServer\Smartbar.PackageAuthoring\bin\Release"
[string] $packageAutoringModuleFile = $packageAuthoringModuleDirectory + "\" + $packageAuthoringModuleFileName

Write-Host "Importing Module `"$packageAuthoringModuleFileName`" from `"$packageAuthoringModuleDirectory`"..."

Import-Module $packageAutoringModuleFile;

Write-Host "Module `"$packageAuthoringModuleFileName`" successfully imported!"

$allNugetPackages = Get-ChildItem -Path $nupackageRepositoryDirectory -Recurse -Include *.nupkg

foreach($file in $allNugetPackages) 
{ 
	Write-Host "Publishing .nupkg file `"$file`" to (default) repository"
	
	$publishSmartbarPluginResult = Publish-SmartbarPlugin -PluginPackageFile $file
	if($publishSmartbarPluginResult.Successful)
	{
		Write-Host "Successfully published `"$file`""
	}
	else
	{
		Write-Error "Could not publish nuget package `"$file`": $($publishSmartbarPluginResult.Exception)"
		Continue
	}
}

Write-Host "Process completed!"

Write-Output $(List-SmartbarPlugin) | select Id, Version