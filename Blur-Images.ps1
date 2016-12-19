param(
    # The folder containing the images to blur
    [Parameter(Mandatory=$true)][string]$folder,
    # The image's width will be divided by this value to create the blur sigma.
    [Parameter(Mandatory=$true)][string]$blurDivisor,    
    # The output suffix to append to each output file.
    [Parameter(Mandatory=$true)][string]$outSuffix
)

function StartImageMagick($filePath, $blurString, $outFilePath) {
     Write-Host "Processing image with ImageMagick... and the following blurString: $($blurString)`n"
    $processInfo = New-Object System.Diagnostics.ProcessStartInfo;
    $processInfo.FileName = "magick.exe";
    $processInfo.RedirectStandardError = $true;
    $processInfo.RedirectStandardOutput = $true;
    $processInfo.UseShellExecute = $false;
    $processInfo.Arguments = "`"$($filePath)`" -gaussian-blur $($blurString) `"$($outFilePath)`"";    
    $process = New-Object System.Diagnostics.Process;
    $process.StartInfo = $processInfo;
    $process.Start() | Out-Null;
    $process.WaitForExit();
    $stdout = $process.StandardOutput.ReadToEnd();
    $stderr = $process.StandardError.ReadToEnd();
    Write-Host $stdout
    Write-Host "Errors:$stderr"
    Write-Host "Done!`n";   
}

function GetImage($file) {    
    begin {        
         [System.Reflection.Assembly]::LoadWithPartialName("System.Drawing") | Out-Null 
    }
    process {
          if (Test-Path $file) {
               $img=[System.Drawing.Image]::FromFile($file);
               $image=$img.Clone();
               $img.Dispose();
               $image | Add-Member `
                              -MemberType NoteProperty `
                              -Name Filename `
                              -Value $file.Fullname `
                              -PassThru
          } else {
               Write-Host "File not found: $file" -fore yellow;
          }   
     }    
    end{}
}

#Entry point:
 $fileList = Get-ChildItem $folder -File -Recurse
 foreach($file in $fileList) {
     $scaleDotIndex = $file.FullName.IndexOf("."); #will break if the filepath has a folder name with a dot in it
     $outPath = $file.FullName.Insert($scaleDotIndex, $outSuffix);
     $imageObject = GetImage $file.FullName;
     $blurSigma = $imageObject.Width / $blurDivisor;
     $blurRadius = $imageObject.Width / 4;
     $blurArgs = "$($blurRadius)x$($blurSigma)";   
     StartImageMagick $file.FullName $blurArgs $outPath;
 }

