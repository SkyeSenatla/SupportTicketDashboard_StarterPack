#!/usr/bin/env bash
# Ensures a working `dotnet` command is available in THIS shell session,
# without needing admin rights, an installer, or internet access.
#
# Usage (from the repo root, in bash - note the leading "source", it matters):
#   source ./tools/setup-dotnet.sh
#
# If dotnet is already installed system-wide, this does nothing.
# Otherwise, it looks for a portable SDK bundle at tools/dotnet-sdk.zip
# (ask a proctor for this if you don't have it - e.g. from a USB drive),
# extracts it to tools/dotnet-sdk/, and prepends that folder to PATH for
# this session only. Nothing is installed system-wide and nothing outside
# the tools/ folder is touched.
#
# Optional: pass a URL as the first argument to download the SDK bundle
# from there if no local zip is found (only do this if you actually have
# internet access to that host):
#   source ./tools/setup-dotnet.sh https://example.com/dotnet-sdk.zip

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SDK_ZIP="$SCRIPT_DIR/dotnet-sdk.zip"
EXTRACT_DIR="$SCRIPT_DIR/dotnet-sdk"
DOWNLOAD_URL="$1"

if command -v dotnet >/dev/null 2>&1 && dotnet --version >/dev/null 2>&1; then
  echo "dotnet is already available: $(dotnet --version)"
  return 0 2>/dev/null || exit 0
fi

echo "No working system-wide dotnet found. Looking for a portable SDK..."

if [ ! -d "$EXTRACT_DIR" ]; then
  if [ ! -f "$SDK_ZIP" ] && [ -n "$DOWNLOAD_URL" ]; then
    echo "No local SDK bundle at $SDK_ZIP - downloading from $DOWNLOAD_URL ..."
    curl -L -o "$SDK_ZIP" "$DOWNLOAD_URL"
  fi

  if [ ! -f "$SDK_ZIP" ]; then
    echo "Could not find a portable SDK at $SDK_ZIP."
    echo "Ask your proctor for the dotnet-sdk.zip bundle, place it at that exact path, then re-run this script."
    return 1 2>/dev/null || exit 1
  fi

  echo "Extracting portable SDK (this can take a minute)..."
  mkdir -p "$EXTRACT_DIR"
  unzip -q "$SDK_ZIP" -d "$EXTRACT_DIR"
fi

export PATH="$EXTRACT_DIR:$PATH"
export DOTNET_ROOT="$EXTRACT_DIR"

if dotnet --version >/dev/null 2>&1; then
  echo "Portable dotnet is now active for this shell: $(dotnet --version)"
  echo "This only applies to THIS shell - if you open a new one, source this script again first."
else
  echo "Extraction finished but dotnet still isn't working. Ask your proctor for help."
  return 1 2>/dev/null || exit 1
fi
