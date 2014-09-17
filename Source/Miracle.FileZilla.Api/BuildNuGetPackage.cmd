@echo off
..\..\NuGet\NuGet.exe pack Miracle.FileZilla.Api.csproj -prop Configuration=release
echo "run ..\..\NuGet\NuGet.exe push Miracle.FileZilla.Api... to publish"
pause