# Build and launch the Quest Viva desktop app (Electron shell + AppShell +
# WasmEditor/WasmPlayer), Phase 1 of docs/electron-desktop-app.md.
#
# Usage:
#   .\electron.ps1 [-Release]
#
# Options:
#   -Release   Build WasmEditor/WasmPlayer in Release mode (AOT-compiled,
#              slower build) instead of the default Debug interpreter build.
#
# Unlike dev.sh, this doesn't run dev servers — it builds the static bundles
# once, assembles them into src/ElectronApp/resources/app-static (same
# editor/AppBundle/player layout deploy-play.yml produces), and launches the
# packaged app. Re-run after changing AppShell/WasmEditor/WasmPlayer source.

param([switch]$Release)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $ScriptDir

$DotnetConfig = "Debug"
if ($Release) {
    $DotnetConfig = "Release"
}

Write-Host "Building WasmEditor ($DotnetConfig)..."
dotnet build src/WasmEditor/WasmEditor.csproj --configuration $DotnetConfig
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host ""
Write-Host "Building WasmPlayer ($DotnetConfig)..."
dotnet build src/WasmPlayer/WasmPlayer.csproj --configuration $DotnetConfig
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host ""
Write-Host "Building AppShell..."
if (-not (Test-Path src/AppShell/node_modules)) {
    Push-Location src/AppShell
    npm install
    if ($LASTEXITCODE -ne 0) { 
        Pop-Location
        exit $LASTEXITCODE 
    }
    Pop-Location
}
$env:PUBLIC_SHOW_HOME = "true"
Push-Location src/AppShell
npm run build
if ($LASTEXITCODE -ne 0) { 
    Pop-Location
    exit $LASTEXITCODE 
}
Pop-Location

Write-Host ""
Write-Host "Building ElectronApp..."
if (-not (Test-Path src/ElectronApp/node_modules)) {
    Push-Location src/ElectronApp
    npm install
    if ($LASTEXITCODE -ne 0) { 
        Pop-Location
        exit $LASTEXITCODE 
    }
    Pop-Location
}
if ($Release) {
    $env:WASM_CONFIG = "Release"
}
Push-Location src/ElectronApp
npm run build
if ($LASTEXITCODE -ne 0) { 
    Pop-Location
    exit $LASTEXITCODE 
}
Pop-Location

Write-Host ""
Write-Host "Launching Quest Viva desktop app..."
Write-Host ""
# cd directly since electron needs "." to resolve to src/ElectronApp/package.json
Push-Location src/ElectronApp
npx electron .
Pop-Location