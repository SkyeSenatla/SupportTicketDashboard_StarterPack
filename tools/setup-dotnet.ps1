# Ensures a working `dotnet` command is available in THIS terminal session,
# without needing admin rights, an installer, or internet access.
#
# Usage (from the repo root, in PowerShell):
#   .\tools\setup-dotnet.ps1
#
# If dotnet is already installed system-wide, this does nothing.
# Otherwise, it looks for a portable SDK bundle at tools\dotnet-sdk.zip
# (ask a proctor for this if you don't have it - e.g. from a USB drive),
# extracts it to tools\dotnet-sdk\, and prepends that folder to PATH for
# this session only. Nothing is installed system-wide and nothing outside
# the tools\ folder is touched.
#
# Optional: if -DownloadUrl is given and no local zip is found, this will
# try to download the SDK bundle from that URL instead (only do this if
# you actually have internet access to that host).

param(
    [string]$SdkZipPath = "$PSScriptRoot\dotnet-sdk.zip",
    [string]$DownloadUrl = ""
)

function Test-DotnetWorks {
    try {
        & dotnet --version *> $null
        return $LASTEXITCODE -eq 0
    } catch {
        return $false
    }
}

if (Test-DotnetWorks) {
    Write-Host "dotnet is already available: $(dotnet --version)" -ForegroundColor Green
    return
}

Write-Host "No working system-wide dotnet found. Looking for a portable SDK..." -ForegroundColor Yellow

$extractPath = "$PSScriptRoot\dotnet-sdk"

if (-not (Test-Path $extractPath)) {
    if ((-not (Test-Path $SdkZipPath)) -and $DownloadUrl -ne "") {
        Write-Host "No local SDK bundle at $SdkZipPath - downloading from $DownloadUrl ..." -ForegroundColor Yellow
        Invoke-WebRequest -Uri $DownloadUrl -OutFile $SdkZipPath
    }

    if (-not (Test-Path $SdkZipPath)) {
        Write-Host "Could not find a portable SDK at $SdkZipPath." -ForegroundColor Red
        Write-Host "Ask your proctor for the dotnet-sdk.zip bundle and place it at that exact path, then re-run this script." -ForegroundColor Red
        exit 1
    }

    Write-Host "Extracting portable SDK (this can take a minute)..." -ForegroundColor Yellow
    Expand-Archive -Path $SdkZipPath -DestinationPath $extractPath -Force
}

$env:PATH = "$extractPath;$env:PATH"
$env:DOTNET_ROOT = $extractPath
$env:DOTNET_MULTILEVEL_LOOKUP = "0"

if (Test-DotnetWorks) {
    Write-Host "Portable dotnet is now active for this terminal: $(dotnet --version)" -ForegroundColor Green
    Write-Host "This only applies to THIS terminal window - if you open a new one, re-run this script first." -ForegroundColor Yellow
} else {
    Write-Host "Extraction finished but dotnet still isn't working. Ask your proctor for help." -ForegroundColor Red
    exit 1
}
