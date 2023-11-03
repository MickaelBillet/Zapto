Remove-Item -path "..\bin\Release" -recurse;
Write-Output "Clean Progress..."
$result = dotnet clean ..\Connect.WebServer.csproj;

# Evaluate success/failure
if($LASTEXITCODE -eq 0)
{
    # Success
    Write-Output "Clean Success"
    Write-Output "Build Progress..."
    $result = dotnet build ..\Connect.WebServer.csproj --force --configuration Release;

    # Evaluate success/failure
    if($LASTEXITCODE -eq 0)
    {
        # Success
        Write-Output "Compil Success"
        Write-Output "Publish Progress..."
        $result = dotnet publish ..\Connect.WebServer.csproj -c Release /p:PublishProfile=..\Properties\PublishProfiles\FolderProfile.pubxml;

        if($LASTEXITCODE -eq 0)
        {
            # Success
            Write-Output "Publish Success"
            cd ..\bin\Release\net7.0\linux-arm;
            $Cmd = "pscp -l pi -pw 280452mb -batch * 192.168.1.13:/home/pi/Desktop/connect" 
            Invoke-Expression "& $( $Cmd )";
            cd ..\..\..\..
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
        Write-Output "Compil failure"
    }
}
else
{
    # Failed, you can reconstruct stderr strings with:
    $ErrorString = $result -join [System.Environment]::NewLine
    Write-Output "Clean failure"
}
