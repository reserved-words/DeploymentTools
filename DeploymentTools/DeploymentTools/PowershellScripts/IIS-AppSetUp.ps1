param($DomainName, $AppName)

Import-Module $PSScriptRoot\SetupTools.psm1

Create-Directory -Path "C:\" -Name "WebApps"
Create-Directory -Path "C:\WebApps" -Name $AppName