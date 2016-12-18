#Script for invoking Inkscape to convert an SVG file to scaled PNGs for all 3 platforms

param(
    # The filepath of the SVG to scale.
    [Parameter(Mandatory=$true)][string]$inputFile    
)

#Inkscape uses a baseline DPI of 90dpi = 100%, for some reason.
$100scale = 90;
$140scale = 126;
$150scale = 135;
$200scale = 180;
$240scale = 216;
$250scale = 225;
$300scale = 270;
$iosFolder = ".\do not commit\resources\ios";
$wp8Folder = ".\do not commit\resources\wp8";
$androidFolder = ".\do not commit\resources\android";
$androidFolderMdpi = ".\do not commit\resources\android\drawable-mdpi";
$androidFolderHdpi = ".\do not commit\resources\android\drawable-hdpi";
$androidFolderXhdpi = ".\do not commit\resources\android\drawable-xhdpi";
$androidFolderXXhdpi = ".\do not commit\resources\android\drawable-xxhdpi";


function ScaleImage($fullPath) {    
    $fileName = [System.IO.Path]::GetFileNameWithoutExtension($fullPath);

    #Generate iOS images
    if(-not (Test-Path $iosFolder)) {
        New-Item -ItemType Directory -Force -Path $iosFolder | Out-Null;
    }        
    $fullOutPath = CanonicalizePath "$($iosFolder)\$($fileName)";
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).png`" -d $100scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath)@2x.png`" -d $200scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath)@3x.png`" -d $300scale"    

    #Generate WP images
    if(-not (Test-Path $wp8Folder)) {
        New-Item -ItemType Directory -Force -Path $wp8Folder | Out-Null;
    }
    $fullOutPath = CanonicalizePath "$($wp8Folder)\$($fileName)";
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-100.png`" -d $100scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-140.png`" -d $140scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-150.png`" -d $150scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-200.png`" -d $200scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-240.png`" -d $240scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-250.png`" -d $250scale"
    StartInkscape "-f `"$($fullPath)`" -e `"$($fullOutPath).scale-300.png`" -d $300scale"    

    #loops are for suckers

    #Generate Android images
    if(-not( Test-Path $androidFolder)) {
        New-Item -ItemType Directory -Force -Path $androidFolder | Out-Null;
    }
    if(-not( Test-Path $androidFolderMdpi)) {
        New-Item -ItemType Directory -Force -Path $androidFolderMdpi | Out-Null;
    }
    if(-not( Test-Path $androidFolderHdpi)) {
        New-Item -ItemType Directory -Force -Path $androidFolderHdpi | Out-Null;
    }
    if(-not( Test-Path $androidFolderXHdpi)) {
        New-Item -ItemType Directory -Force -Path $androidFolderXHdpi | Out-Null;
    }
    if(-not( Test-Path $androidFolderXXhdpi)) {
        New-Item -ItemType Directory -Force -Path $androidFolderXXhdpi | Out-Null;
    }
    $mdpiPath = CanonicalizePath("$($androidFolderMdpi)\$($fileName)");
    StartInkscape "-f `"$($fullPath)`" -e `"$($mdpiPath).png`" -d $100scale"
    $hdpiPath = CanonicalizePath("$($androidFolderHdpi)\$($fileName)");
    StartInkscape "-f `"$($fullPath)`" -e `"$($hdpiPath).png`" -d $150scale"
    $xhdpiPath = CanonicalizePath("$($androidFolderXhdpi)\$($fileName)");
    StartInkscape "-f `"$($fullPath)`" -e `"$($xhdpiPath).png`" -d $200scale"
    $xxhdpiPath = CanonicalizePath("$($androidFolderXXhdpi)\$($fileName)");
    StartInkscape "-f `"$($fullPath)`" -e `"$($xxhdpiPath).png`" -d $300scale"
}

function StartInkscape($argString) {
    Write-Host "Processing svg with Inkscape...`n"
    $processInfo = New-Object System.Diagnostics.ProcessStartInfo;
    $processInfo.FileName = "inkscape.exe";
    $processInfo.RedirectStandardError = $true;
    $processInfo.RedirectStandardOutput = $true;
    $processInfo.UseShellExecute = $false;
    $processInfo.Arguments = $argString;    
    $process = New-Object System.Diagnostics.Process;
    $process.StartInfo = $processInfo;
    $process.Start() | Out-Null;
    $process.WaitForExit();
    $stdout = $process.StandardOutput.ReadToEnd();
    $stderr = $process.StandardError.ReadToEnd();
    Write-Host $stdout
    Write-Host "Errors:`n$stderr"
    Write-Host "Done!`n";    
}

function CanonicalizePath($path) {
    $path = [System.IO.Path]::Combine( ((Get-Location).Path), ($path) );
    $path = [System.IO.Path]::GetFullPath($path);
    return $path;
}

#Entry point is here:

#Check for Inkscape
if ((Get-Command "inkscape.exe" -ErrorAction SilentlyContinue) -eq $null) {
    Write-Host "Inkscape must be installed on the path (as inkscape.exe) for this script to work. Exiting..."
    return;
}

#normalize input path
$fullPath = [System.IO.Path]::Combine( ((Get-Location).Path), ($inputFile) );
$fullPath = [System.IO.Path]::GetFullPath($fullPath);
ScaleImage $fullPath;