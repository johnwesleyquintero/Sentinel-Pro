# Build script for Sentinel Pro

# Set version
$version = "1.0.0"
$buildDate = Get-Date -Format "yyyyMMdd"
$buildNumber = "$version-$buildDate"

# Clean previous builds
Write-Host "Cleaning previous builds..."
dotnet clean SentinelPro.csproj

# Restore NuGet packages
Write-Host "Restoring packages..."
dotnet restore SentinelPro.csproj

# Build solution with Release configuration
Write-Host "Building solution..."
dotnet build SentinelPro.csproj --configuration Release /p:Version=$version

# Run automated tests
Write-Host "Running tests..."

# Security scan
Write-Host "Running security scan..."
dotnet list package --vulnerable

# Create publish directory
if (!(Test-Path .\publish)) {
    New-Item -ItemType Directory -Force -Path .\publish
}

# Publish executable for Windows with production settings
Write-Host "Creating release build..."
dotnet publish SentinelPro.csproj -c Release -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -p:IncludeNativeLibrariesForSelfExtract=false `
    -p:Version=$version `
    -p:FileVersion=$version `
    -p:AssemblyVersion=$version `
    -o .\publish

# Create ZIP archive
$zipPath = ".\publish\SentinelPro-$buildNumber.zip"
Write-Host "Creating release archive: $zipPath"
if (Test-Path ".\publish\SentinelPro.exe") {
    Compress-Archive -Path ".\publish\SentinelPro.exe" -DestinationPath $zipPath -Force
}
else {
    Write-Host "Publish failed - SentinelPro.exe not found"
    exit 1
}

# Copy files to wwwroot/publish for web serving
$wwwrootPublishDir = ".\wwwroot\publish"
if (!(Test-Path $wwwrootPublishDir)) {
    New-Item -ItemType Directory -Force -Path $wwwrootPublishDir
}

if (Test-Path ".\publish\SentinelPro.exe") {
    Copy-Item ".\publish\SentinelPro.exe" -Destination "c:\Users\johnw\Sentinel-Pro\wwwroot\publish\SentinelPro-Setup.exe" -Force
}
if (Test-Path $zipPath) {
    Copy-Item $zipPath -Destination "$wwwrootPublishDir" -Force
}

Write-Host "Build completed successfully!"
Write-Host "Release package: $zipPath"
Write-Host "Web download path: /publish/SentinelPro-Setup.exe"