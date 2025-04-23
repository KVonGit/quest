# Set version
$questVersion = "5.9.0"

# Define paths - using resolved full paths
$sourceDir = Resolve-Path "..\Quest\bin\x86\Release"
$targetDir = Join-Path (Get-Location) "Quest-$questVersion-Portable"
$depsDir = Join-Path $targetDir "Prerequisites"

# Create directories
New-Item -ItemType Directory -Force -Path $targetDir | Out-Null
New-Item -ItemType Directory -Force -Path $depsDir | Out-Null

# Copy Quest files, excluding unwanted files
Get-ChildItem -Path $sourceDir -Recurse |
    Where-Object { 
        $_.Name -notmatch '.*\.vshost\..*' -and 
        $_.Extension -ne '.pdb' -and 
        $_.Extension -ne '.xml' -and 
        $_.DirectoryName -notmatch '\\x86$'
    } |
    ForEach-Object {
        # Calculate relative path from source root
        $relativePath = $_.FullName.Substring($sourceDir.Path.Length)
        # Construct target path
        $targetPath = Join-Path $targetDir $relativePath
        # Create parent directory if needed
        $targetParent = Split-Path -Parent $targetPath
        if (!(Test-Path $targetParent)) {
            New-Item -ItemType Directory -Force -Path $targetParent | Out-Null
        }
        # Copy file
        Copy-Item $_.FullName -Destination $targetPath -Force
    }

# Copy prerequisites from Dependencies folder if they exist, otherwise download
$depsSource = "..\Dependencies"
$vcRedistPath = "$depsDir\VC_redist.x86.exe"
$dotNetPath = "$depsDir\NDP48-x86-x64-AllOS-ENU.exe"

Write-Host "Checking prerequisites..."
if (Test-Path "$depsSource\VC_redist.x86.exe") {
    Write-Host "Copying VC++ Redistributable from Dependencies folder..."
    Copy-Item "$depsSource\VC_redist.x86.exe" -Destination $vcRedistPath -Force
} else {
    Write-Host "Downloading VC++ Redistributable..."
    Invoke-WebRequest -Uri "https://aka.ms/vs/17/release/VC_redist.x86.exe" -OutFile $vcRedistPath
}

# For .NET, just add the download link to README
$readmeContent = @"
Quest $questVersion Portable
==========================

Prerequisites:
1. .NET Framework 4.8
   - Download from: https://go.microsoft.com/fwlink/?linkid=2088631
   - Or install from Windows Update
2. Visual C++ Redistributable for Visual Studio 2022 (x86)
   - Install from Prerequisites\VC_redist.x86.exe

To run Quest:
1. Make sure prerequisites are installed
2. Launch Quest.exe

Note for Windows 7 users:
- Please ensure you install .NET Framework 4.8 first
- Make sure Windows 7 SP1 is installed
- All Windows Updates should be installed before running the prerequisites
"@ 
$readmeContent | Out-File -FilePath "$targetDir\README.txt"