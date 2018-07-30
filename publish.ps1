$APIKEY = (Get-Content ./Onitama.LuCHEF.Angsthaas/apikey.txt).Trim()

$sourcePath = ($PSScriptRoot + "/publish")
$targetPath = ($PSScriptRoot + "/published.zip")

$uri = "https://botchallenge.intern.infi.nl/api/upload/bot/$APIKEY"

dotnet publish -o $sourcePath

if(Test-Path $targetPath)  {
    Remove-Item ($PSScriptRoot + "/published.zip")
}

Add-Type -Assembly System.IO.Compression.FileSystem
$compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
[System.IO.Compression.ZipFile]::CreateFromDirectory($sourcePath, $targetPath, $compressionLevel, $false)

Remove-Item -Recurse -Force $sourcePath

$bytes = (New-Object Net.WebClient).UploadFile($uri, $targetPath)

# Writes response from server to command line:
[System.Text.Encoding]::UTF8.GetString($bytes)
