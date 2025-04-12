# Sentinel Pro Deployment Package

## System Requirements
- Windows 10/11 64-bit
- .NET 6.0 Runtime
- 500MB free disk space
- Administrator privileges for installation

## Included Files
- `SentinelPro-Setup.exe` - Main installer with Setup Wizard
- `Latest-Release-Notes.md` - Version history and changes

## Installation Instructions
1. Right-click SentinelPro-Setup.exe
2. Select 'Run as Administrator'
3. Follow setup wizard steps
4. Launch from Start Menu after completion

## Troubleshooting
- **Missing DLL Errors**: Install latest [.NET 6.0 Runtime](https://dotnet.microsoft.com/download)
- **Installation Blocked**: Check SmartScreen settings or right-click > Properties > Unblock
- **Service Errors**: Verify running with administrator privileges

## Security Notes
- SHA256 Checksum: [Will be auto-generated during build]
- Signed with Microsoft Authenticode

## Build Information
- Built using PowerShell 7
- Requires MSBuild Tools 2022
- Automated via CI/CD pipeline