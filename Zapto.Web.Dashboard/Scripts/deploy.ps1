$sourcePath="C:\Users\micka\source\zapto\Zapto.Framework\Framework.Core"
$destinationPath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard.Deploy\"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Framework\Framework.Infrastructure.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\AirZapto.Framework\AirZapto.Applications.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\AirZapto.Framework\AirZapto.Domain"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\AirZapto.Framework\AirZapto.Infrastructure.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Connect.Framework\Connect.Application.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Connect.Framework\Connect.Domain"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Connect.Framework\Connect.Infrastructure.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\WeatherZapto.Framework\WeatherZapto.Application.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\WeatherZapto.Framework\WeatherZapto.Domain"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\WeatherZapto.Framework\WeatherZapto.Infrastructure.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Web.Component\Zapto.Component.Charts"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Web.Component\Zapto.Component.Common"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Web.Component\Zapto.Component.Dashboard"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Component.AirPollution"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Web.HealthCheck"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Component.Home"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Component.Services"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force

$sourcePath="C:\Users\micka\source\zapto\Zapto.Web.Dashboard\Zapto.Web.Dashboard"
if (-not (Test-Path -Path $destinationPath)) {
    New-Item -ItemType Directory -Path $destinationPath
}

Copy-Item -Path $sourcePath -Destination $destinationPath -Recurse -Force