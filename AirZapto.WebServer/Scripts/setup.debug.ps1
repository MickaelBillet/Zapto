cd C:\Users\mbillet.NOVACATH\Logiciel\Zapto\AirZapto.WebServices\;

Remove-Item -path ".\bin\Debug" -recurse
Write-Output "Clean Progress..."

$result = dotnet clean .\AirZapto.WebServices.csproj

# Evaluate success/failure
if($LASTEXITCODE -eq 0)
{
    # Success
    Write-Output "Clean Success"
    Write-Output "Build Progress..."
    $result = dotnet build .\AirZapto.WebServices.csproj --force --configuration Debug

    # Evaluate success/failure
    if($LASTEXITCODE -eq 0)
    {
        # Success
        Write-Output "Build Success"
        Write-Output "Publish Progress..."
        $result = dotnet publish .\AirZapto.WebServices.csproj -c debug /p:PublishProfile=.\Properties\PublishProfiles\FolderProfile.pubxml
            
        if($LASTEXITCODE -eq 0)
        {
            # Success
            Write-Output "Publish Success"
            cd C:\Users\mbillet.NOVACATH\Logiciel\Zapto\AirZapto.WebServices\bin\Debug\net6.0\linux-arm\;
            $Cmd = "pscp -l pi -pw 280452mb -batch * 192.168.1.13:/home/pi/Desktop/zapto" 
            Invoke-Expression "& $( $Cmd )";
        }
        else
        {
            # Failed, you can reconstruct stderr strings with:
            $ErrorString = $result -join [System.Environment]::NewLine
            Write-Output "Publish failure"
        }
    }
    else
    {
        # Failed, you can reconstruct stderr strings with:
        $ErrorString = $result -join [System.Environment]::NewLine
        Write-Output "Build failure"
    }
}
else
{
    # Failed, you can reconstruct stderr strings with:
    $ErrorString = $result -join [System.Environment]::NewLine
    Write-Output "Clean failure"
}