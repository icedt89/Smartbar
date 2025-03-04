[string] $nupackageRepositoryDirectory = $PSScriptRoot + "\..\Deployment\Packages"
[string] $packageAuthoringModuleFileName = "JanHafner.Smartbar.PackageAuthoring.dll"
[string] $packageAuthoringModuleDirectory = $PSScriptRoot + "\..\NuGetServer\Smartbar.PackageAuthoring\bin\Release"
[string] $packageAuthoringModuleFile = $packageAuthoringModuleDirectory + "\" + $packageAuthoringModuleFileName

Write-Host "Importing Module `"$packageAuthoringModuleFileName`" from `"$packageAuthoringModuleDirectory`"..."

Import-Module $packageAuthoringModuleFile;

Write-Host "Module `"$packageAuthoringModuleFileName`" successfully imported!"

[string[]] $nuspecFiles = @()
$nuspecFiles += $PSScriptRoot + "\Smartbar.ProcessApplication.Plugins.ShellLinkFiles\Smartbar.ProcessApplication.Plugins.ShellLinkFiles.nuspec"
$nuspecFiles += $PSScriptRoot + "\Smartbar.ProcessApplication.Plugins.Urls\Smartbar.ProcessApplication.Plugins.Urls.nuspec"
$nuspecFiles += $PSScriptRoot + "\Smartbar.ProcessApplication.Plugins.SystemTools\Smartbar.ProcessApplication.Plugins.SystemTools.nuspec"

foreach($file in $nuspecFiles) 
{ 
	Write-Host "Building .nupkg file for `"$file`" in $nupackageRepositoryDirectory"
	
	$buildSmartbarPluginResult = Build-SmartbarPlugin -LocalPackagesRepositoryDirectory $nupackageRepositoryDirectory -SourceNuspecPath $File
	if($buildSmartbarPluginResult.Successful)
	{
		Write-Host "Successfully built `"$($buildSmartbarPluginResult.FinalNugetPackageFile)`""
	}
	else
	{
		Write-Error "Could not build nuget package for nuspec file `"$(file)`": $($buildSmartbarPluginResult.Exception)"
		Continue
	}
}

Write-Host "Process completed!"

Write-Output $(List-SmartbarPlugin -LocalPackagesRepositoryDirectory $nupackageRepositoryDirectory) | select Id, Version