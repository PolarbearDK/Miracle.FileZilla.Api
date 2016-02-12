param (
	[switch]$Pack = $true,
	[switch]$Push
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"

if($pack) {
	& msbuild Miracle.FileZilla.Api.csproj /p:Configuration=release
	if(!$?){throw "msbuild returned exit code $LASTEXITCODE"}
	
	& ..\.NuGet\NuGet.exe pack Miracle.FileZilla.Api.csproj -prop Configuration=release
	if(!$?){throw "NuGet returned exit code $LASTEXITCODE"}
}

if($push) {
	$filename = Get-ChildItem "Miracle.FileZilla.Api.*" | Sort-Object LastWriteTime -Descending | Select -First 1
	& ..\.NuGet\NuGet.exe push "$filename"
}
