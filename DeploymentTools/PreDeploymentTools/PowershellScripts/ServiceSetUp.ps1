param($DomainName, $AppName, $UserName, $Password)

Import-Module $PSScriptRoot\SetupTools.psm1

$ServiceUsersGroupName = "TaskAccounts"

$ServiceUsers = Add-Group -GroupName $ServiceUsersGroupName

Grant-Right -Domain $DomainName -GroupName $ServiceUsersGroupName -Right "SeBatchLogonRight"

Create-Directory -Path "C:\" -Name "Services"
Create-Directory -Path "C:\Services" -Name $AppName

New-TaskUser -UserName $UserName -Password $Password -AppFolderName $AppName