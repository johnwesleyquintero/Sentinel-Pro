# Build script for Sentinel Pro

# Set version
$version = "1.0.0"
$buildDate = Get-Date -Format "yyyyMMdd"
$buildNumber = "$version-$buildDate"

# Clean previous builds
Write-Host "Cleaning previous builds..."
dotnet clean

# Restore NuGet packages
Write-Host "Restoring packages..."
dotnet restore

# Build solution with Release configuration
Write-Host "Building solution..."
dotnet build --configuration Release /p:Version=$version

# Run automated tests
Write-Host "Running tests..."
dotnet test --no-restore --configuration Release

# Security scan
Write-Host "Running security scan..."
dotnet list package --vulnerable

# Publish executable for Windows with production settings
Write-Host "Creating release build..."
dotnet publish SentinelPro.csproj -c Release -r win-x64 `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false `
    -p:Version=$version `
    -p:FileVersion=$version `
    -p:AssemblyVersion=$version `
    -o .\publish

# Create ZIP archive
$zipPath = ".\publish\SentinelPro-$buildNumber.zip"
Write-Host "Creating release archive: $zipPath"
Compress-Archive -Path ".\publish\SentinelPro.exe" -DestinationPath $zipPath -Force

# Copy files to wwwroot/publish for web serving
$wwwrootPublishDir = ".\wwwroot\publish"
if (!(Test-Path $wwwrootPublishDir)) {
    New-Item -ItemType Directory -Force -Path $wwwrootPublishDir
}

Copy-Item ".\publish\SentinelPro.exe" -Destination "c:\Users\johnw\Sentinel-Pro\wwwroot\publish\SentinelPro-Setup.exe" -Force
Copy-Item $zipPath -Destination "$wwwrootPublishDir" -Force

Write-Host "Build completed successfully!"
Write-Host "Release package: $zipPath"
Write-Host "Web download path: /publish/SentinelPro-Setup.exe"