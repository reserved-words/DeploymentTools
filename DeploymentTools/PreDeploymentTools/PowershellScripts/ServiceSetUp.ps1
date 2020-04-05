param($DomainName, $AppName, $UserName, $Password, $LogDirectory)

$logfile =  $LogDirectory + "\$(get-date -format `"yyyy-MM-dd`").log";

try
{
	"Importing SetupTools" | out-file -Filepath $logfile -append
	Import-Module $PSScriptRoot\SetupTools.psm1

	$ServiceUsersGroupName = "TaskAccounts"

	"Adding TaskAccounts group" | out-file -Filepath $logfile -append
	$ServiceUsers = Add-Group -GroupName $ServiceUsersGroupName

	"Granting SeBatchLogonRight" | out-file -Filepath $logfile -append
	Grant-Right -Domain $DomainName -GroupName $ServiceUsersGroupName -Right "SeBatchLogonRight"

	"Creating Services directory" | out-file -Filepath $logfile -append
	Create-Directory -Path "C:\" -Name "Services"

	"Creating application directory" | out-file -Filepath $logfile -append
	Create-Directory -Path "C:\Services" -Name $AppName

	"Creating task user" | out-file -Filepath $logfile -append
	New-TaskUser -UserName $UserName -Password $Password -AppFolderName $AppName
}
catch
{
	$_.Exception.Message | out-file -Filepath $logfile -append
}