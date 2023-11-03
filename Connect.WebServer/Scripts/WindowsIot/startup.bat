echo On
set "PWSH=%SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe"
%PWSH%  -command "c:\Publish\Scripts\startup.ps1"   
pause 
if errorlevel 1 exit %errorlevel% 