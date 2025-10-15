#!/usr/bin/env pwsh
#Requires -Version 7.0

<#
.SYNOPSIS
    Builds and installs pwsh-argument-completer module.

.DESCRIPTION
    This script builds the native executable and installs the module to ~/.local/pwsh-modules/pwsh-argument-completer.
    After installation, add the module path to your PowerShell profile if needed.

.EXAMPLE
    ./install.ps1
#>

[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'

# Determine platform-specific runtime identifier
$rid = if ($IsWindows) {
    'win-x64'
} elseif ($IsMacOS) {
    if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq 'Arm64') {
        'osx-arm64'
    } else {
        'osx-x64'
    }
} elseif ($IsLinux) {
    'linux-x64'
} else {
    throw "Unsupported platform"
}

Write-Host "Building pwsh-argument-completer for $rid..." -ForegroundColor Cyan

# Build the project
$projectPath = Join-Path $PSScriptRoot 'src' 'PowerShellArgumentCompleter.csproj'
$publishPath = Join-Path $PSScriptRoot 'src' 'publish'

dotnet publish $projectPath -c Release -r $rid -o $publishPath

if ($LASTEXITCODE -ne 0) {
    throw "Build failed with exit code $LASTEXITCODE"
}

# Determine executable name
$exeName = if ($IsWindows) { 'pwsh-argument-completer.exe' } else { 'pwsh-argument-completer' }
$exePath = Join-Path $publishPath $exeName

if (-not (Test-Path $exePath)) {
    throw "Executable not found at $exePath"
}

Write-Host "Build successful!" -ForegroundColor Green

# Create installation directory
$installDir = Join-Path $HOME '.local' 'pwsh-modules' 'PwshArgumentCompleter'

if (Test-Path $installDir) {
    Write-Host "Removing existing installation at $installDir..." -ForegroundColor Yellow
    Remove-Item $installDir -Recurse -Force
}

Write-Host "Creating installation directory at $installDir..." -ForegroundColor Cyan
New-Item -ItemType Directory -Path $installDir -Force | Out-Null

# Copy executable
Write-Host "Copying executable..." -ForegroundColor Cyan
Copy-Item $exePath $installDir

# Copy module files
Write-Host "Copying module files..." -ForegroundColor Cyan
$moduleDir = Join-Path $PSScriptRoot 'module'
Copy-Item (Join-Path $moduleDir 'PwshArgumentCompleter.psm1') $installDir
Copy-Item (Join-Path $moduleDir 'PwshArgumentCompleter.psd1') $installDir

Write-Host ""
Write-Host "Installation complete!" -ForegroundColor Green
Write-Host ""
Write-Host "To use the module, add this line to your PowerShell profile:" -ForegroundColor Cyan
Write-Host "    Import-Module ~/.local/pwsh-modules/PwshArgumentCompleter/PwshArgumentCompleter.psd1" -ForegroundColor White
Write-Host ""
Write-Host "To edit your profile, run:" -ForegroundColor Cyan
Write-Host "    code `$PROFILE" -ForegroundColor White
