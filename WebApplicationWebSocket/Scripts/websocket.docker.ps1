cd ../..

Write-Output "Docker login..."

$result = docker login -u mickabdocker -p mauvaisTemps!08

# Evaluate success/failure
if($LASTEXITCODE -eq 0)
{
    # Success
    Write-Output "Docker Login Success"
    Write-Output "Docker Build Progress..."
    $result = docker build --tag mickabdocker/websocket:v1 --file ./WebApplicationWebSocket/Dockerfile .

    # Evaluate success/failure
    if($LASTEXITCODE -eq 0)
    {
        # Success
        Write-Output "Docker Build Success"
        Write-Output "Docker Push Progress..."
        $result = docker push mickabdocker/websocket:v1
     
        # Evaluate success/failure
        if($LASTEXITCODE -eq 0)
        {
            # Success
            Write-Output "Docker Push Success"
        }
        else
        {
            # Failed, you can reconstruct stderr strings with:
            $ErrorString = $result -join [System.Environment]::NewLine
            Write-Output "Docker Push"
        }
    }
    else
    {
        # Failed, you can reconstruct stderr strings with:
        $ErrorString = $result -join [System.Environment]::NewLine
        Write-Output "Docker Build"
    }
}
else
{
    # Failed, you can reconstruct stderr strings with:
    $ErrorString = $result -join [System.Environment]::NewLine
    Write-Output "Docker Login"
}