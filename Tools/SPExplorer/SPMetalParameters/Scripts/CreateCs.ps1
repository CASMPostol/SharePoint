Write-Host "This file creates all the c# classes for xnl schemas"
Get-Location | Write-host
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools"
$cpath = get-location
Write-host Processing
set-location ..\
xsd.exe "SPMetalParameters.xsd" /N:CAS.SharePoint.Tools.SPExplorer.SPMetalParameters /c  |write-host
set-location $cpath
Write-host Done...

