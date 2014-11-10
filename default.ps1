properties { 
  $base_dir = resolve-path .
  $build_dir = "$base_dir\build"
  $packageinfo_dir = "$base_dir\nuspecs"
  $packageinfo_dir_last = "$base_dir\nuspecs.last"
  $35_build_dir = "$build_dir\3.5"
  $40_build_dir = "$build_dir\4.0"
  $45_build_dir = "$build_dir\4.5"
  $release_dir = "$base_dir\Release"
  $release_dir_last = "$base_dir\Release.last"
  $sln_file = "$base_dir\BclEx-Abstract.sln"
  $tools_dir = "$base_dir\tools"
  $lib_dir = "$base_dir\lib"
  $packages_dir = "$base_dir\packages"
  $version = "1.0.0" #Get-Version-From-Git-Tag
  $35_config = "Release"
  $40_config = "Release.4"
  $45_config = "Release.45"
  $run_tests = $true
}
Framework "4.0"

#include .\psake_ext.ps1
	
task default -depends Package

task Clean {
	remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue
	remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue
	remove-item -force -recurse $release_dir_last -ErrorAction SilentlyContinue
}

task Init -depends Clean {
	new-item $build_dir -itemType directory
	new-item $release_dir -itemType directory
	new-item $release_dir_last -itemType directory
}

task Compile -depends Init {
	msbuild $sln_file /p:"OutDir=$35_build_dir;Configuration=$35_config" /m
	msbuild $sln_file /target:Rebuild /p:"OutDir=$40_build_dir;Configuration=$40_config" /m
	msbuild $sln_file /target:Rebuild /p:"OutDir=$45_build_dir;Configuration=$45_config" /m
}

task Test -depends Compile -precondition { return $run_tests } {
	$old = pwd
	cd $build_dir
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	#& $tools_dir\xUnit\xunit.console.clr4.exe "$40_build_dir\System.Abstract.Tests.dll" /noshadow
	cd $old
}

task Dependency {
	$package_files = @(Get-ChildItem src -include *packages.config -recurse)
	foreach ($package in $package_files)
	{
		Write-Host $package.FullName
		& $tools_dir\NuGet.exe install $package.FullName -o packages
	}
}

task Release -depends Dependency, Compile, Test {
	cd $build_dir
	& $tools_dir\7za.exe a $release_dir\BclEx-Abstract.zip `
		*\System.Abstract.dll `
		*\System.Abstract.xml `
    	..\license.txt
	if ($lastExitCode -ne 0) {
		throw "Error: Failed to execute ZIP command"
    }
}

task Bundle {
	& $tools_dir\ILMerge.exe /targetplatform:v4 /out:"$40_build_dir\Contoso.Bundle01Web.dll" `
"$40_build_dir\Contoso.Bundle.Bundle01Web.dll" `
"$40_build_dir\Common.Logging.dll" `
"$40_build_dir\Contoso.Abstract.Log4Net.dll" `
"$40_build_dir\contoso.Abstract.RhinoServiceBus.dll" `
"$40_build_dir\contoso.Abstract.ServerAppFabric.dll" `
"$40_build_dir\Contoso.Abstract.Unity.dll" `
"$40_build_dir\Contoso.Abstract.Web.dll" `
"$40_build_dir\log4net.dll" `
"$40_build_dir\Microsoft.ApplicationServer.Caching.Client.dll" `
"$40_build_dir\Microsoft.ApplicationServer.Caching.Core.dll" `
"$lib_dir\CommonServiceLocator\Microsoft.Practices.ServiceLocation.dll" `
"$40_build_dir\Microsoft.Practices.Unity.dll" `
"$40_build_dir\Rhino.ServiceBus.dll"
	& $tools_dir\ILMerge.exe /targetplatform:v2 /out:"$35_build_dir\Contoso.Bundle01Web.dll" `
"$35_build_dir\Contoso.Bundle.Bundle01Web.dll" `
"$35_build_dir\Common.Logging.dll" `
"$35_build_dir\Contoso.Abstract.Log4Net.dll" `
"$35_build_dir\contoso.Abstract.RhinoServiceBus.dll" `
"$35_build_dir\contoso.Abstract.ServerAppFabric.dll" `
"$35_build_dir\Contoso.Abstract.Unity.dll" `
"$35_build_dir\Contoso.Abstract.Web.dll" `
"$35_build_dir\log4net.dll" `
"$35_build_dir\Microsoft.ApplicationServer.Caching.Client.dll" `
"$35_build_dir\Microsoft.ApplicationServer.Caching.Core.dll" `
"$lib_dir\CommonServiceLocator\Microsoft.Practices.ServiceLocation.dll" `
"$35_build_dir\Microsoft.Practices.Unity.dll" `
"$35_build_dir\Rhino.ServiceBus.dll"
}

task Package -depends Release, Bundle {
	$spec_files = @(Get-ChildItem $packageinfo_dir -include *.nuspec -recurse)
	foreach ($spec in $spec_files)
	{
		& $tools_dir\NuGet.exe pack $spec.FullName -o $release_dir -Symbols -BasePath $base_dir
	}
	$spec_files = @(Get-ChildItem $packageinfo_dir_last -include *.nuspec -recurse)
	foreach ($spec in $spec_files)
	{
		#& $tools_dir\NuGet.exe pack $spec.FullName -o $release_dir_last -Symbols -BasePath $base_dir
	}
}

task Push -depends Package {
	$spec_files = @(Get-ChildItem $release_dir -include *.nupkg -recurse)
	foreach ($spec in $spec_files)
	{
		& $tools_dir\NuGet.exe push $spec.FullName -source "https://www.nuget.org"
	}
	$spec_files = @(Get-ChildItem $release_dir_last -include *.nupkg -recurse)
	foreach ($spec in $spec_files)
	{
		#& $tools_dir\NuGet.exe push $spec.FullName -source "https://www.nuget.org"
	}
}

