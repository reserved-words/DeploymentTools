param($DomainName, $AppName, $UserName, $Password, $LogFile)

try
{
	"Importing SetupTools" | out-file -Filepath $LogFile -append
	Import-Module $PSScriptRoot\SetupTools.psm1

	$ServiceUsersGroupName = "TaskAccounts"

	"Adding TaskAccounts group" | out-file -Filepath $LogFile -append
	$ServiceUsers = Add-Group -GroupName $ServiceUsersGroupName

	"Granting SeBatchLogonRight" | out-file -Filepath $LogFile -append
	Grant-Right -Domain $DomainName -GroupName $ServiceUsersGroupName -Right "SeBatchLogonRight"

	"Creating Services directory" | out-file -Filepath $LogFile -append
	Create-Directory -Path "C:\" -Name "Services"

	"Creating application directory" | out-file -Filepath $LogFile -append
	Create-Directory -Path "C:\Services" -Name $AppName

	"Creating task user" | out-file -Filepath $LogFile -append
	New-TaskUser -UserName $UserName -Password $Password -AppFolderName $AppName
}
catch
{
	$_.Exception.Message | out-file -Filepath $LogFile -append
}