# Build script for WorkspaceCleanup

# Clean previous builds
dotnet clean

# Restore NuGet packages
dotnet restore .\WorkspaceCleanup.Tests\SentinelPro.Tests.csproj

# Build solution with Release configuration
dotnet build --configuration Release

# Run automated tests
dotnet test .\WorkspaceCleanup.Tests\SentinelPro.Tests.csproj --no-restore

# Publish executable for Windows with production settings
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o .\publish

# Display success message
Write-Host "Build completed successfully! Output in .\publish directory"

# Add security scanning
Write-Host "Running security scan..."
dotnet list package --vulnerable

# Run performance profiling
Write-Host "Running performance profiling..."
dotnet counters monitor --process-id $PID --counters System.Runtime