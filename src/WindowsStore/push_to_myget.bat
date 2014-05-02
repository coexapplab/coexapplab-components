pushd .\NuGet\Sabbour.WindowsStore.Controls\

@echo *******************************************
@echo * PUSHING NUGET TO myGet                  *
@echo *******************************************

nuget push Sabbour.WindowsStore.Controls.1.0.1.0.nupkg cb449da5-5e6d-4496-95d7-bd4196fe4963 -Source https://www.myget.org/F/sabbour/api/v2/package

@echo *******************************************
@echo * PUBLISHED					*
@echo *******************************************