tools\NuGet\NuGet.exe restore src\Humanizer.sln

@%WINDIR%\Microsoft.Net\Framework\v4.0.30319\msbuild build.proj /m /clp:Verbosity=minimal
