@echo off
echo Building...
dotnet build .\src\Agora.API\Agora.API.csproj /p:useTestcontainer=true
if %errorlevel% neq 0 (
    echo Build failed.
    exit /b %errorlevel%
)
echo Build completed successfully.
echo Running...
dotnet run --project .\src\Agora.API\Agora.API.csproj --no-build
