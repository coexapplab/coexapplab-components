@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

if "%nuget%" == "" (
   set nuget=NuGet.exe
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set projectname=Coex.AppLab.Components.WindowsStore.Controls

@echo ***************************************************************
@echo * Build %projectname% 	        *
@echo ***************************************************************
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild  %projectname%.csproj /verbosity:normal /fl /t:Rebuild /p:Configuration=%config%,OutputPath=bin\%config%\ /property:GenerateLibraryLayout=false /p:NoWarn=0618

@echo ***************************************************************
@echo * Copying files for NuGet structure               	        *
@echo ***************************************************************
mkdir NuGet\%projectname%\lib\win81
mkdir NuGet\%projectname%\lib\win81\%projectname%
mkdir NuGet\%projectname%\lib\win81\%projectname%\Assets

xcopy /y %projectname%.nuspec NuGet\%projectname%
xcopy /y bin\%config%\*.dll NuGet\%projectname%\lib\win81 
xcopy /y bin\%config%\*.pdb NuGet\%projectname%\lib\win81 
xcopy /y bin\%config%\*.dll NuGet\%projectname%\lib\win81 
xcopy /y bin\%config%\*.xr.xml NuGet\%projectname%\lib\win81 
xcopy /y bin\%config%\*.xbf NuGet\%projectname%\lib\win81\%projectname%
xcopy /ye Assets\* NuGet\%projectname%\lib\win81\%projectname%\Assets\

@echo *******************************************
@echo * BUILDING NUGET PACKAGE					*
@echo *******************************************
%nuget% pack NuGet\%projectname%\%projectname%.nuspec -symbols -o NuGet\%projectname% -p Configuration=%config% %version%