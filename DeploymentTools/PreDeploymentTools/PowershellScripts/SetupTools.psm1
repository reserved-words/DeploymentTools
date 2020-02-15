Import-Module $PSScriptRoot\UserRights.psm1

function Add-User {
	param([string]$UserName, [string]$Password, [string]$GroupName)

	$SecurePassword = ConvertTo-SecureString $Password -AsPlainText -Force

	$LocalUsers = Get-LocalUser | % {$_.name} 

	if ($LocalUsers -Contains $UserName){
		Remove-LocalUser -Name $UserName
	}

	New-LocalUser $UserName -Password $SecurePassword -PasswordNeverExpires -UserMayNotChangePassword
}

function Add-GroupMember {
	param([string]$UserName, [string]$GroupName)

	$User = Get-LocalUser -Name $UserName

	$LocalGroups = Get-LocalGroup | % {$_.name} 

	$GroupMembers = Get-LocalGroupMember -Name $GroupName 
	$UserIsMember = $GroupMembers -Contains $User

	if (-Not $UserIsMember){
		Add-LocalGroupMember -Name $GroupName -Member $UserName
	}
}

function New-User {
    param([string]$UserName, [string]$Password, [string]$GroupName, [string]$AppFolderLocation, [string]$AppFolderName)
    
    Add-User -UserName $UserName -Password $Password -GroupName $GroupName    
    Add-GroupMember -User $UserName -GroupName $GroupName
    Grant-FolderAccess -FolderLocation $AppFolderLocation -FolderName $AppFolderName -Name $UserName
}

function Add-Group {
	param([string]$GroupName)

	$LocalGroups = Get-LocalGroup | % {$_.name} 
	
	if ($LocalGroups -Contains $GroupName){
        return Get-LocalGroup -Name $GroupName
    	# Remove-LocalGroup -Name $GroupName
	}

    return New-LocalGroup -Name $GroupName
}

function Grant-FolderAccess {
    param([string]$FolderLocation, [string]$FolderName, [string]$Name)

    New-Item -Path $FolderLocation -Name $FolderName -ItemType "directory" -Force

    $FolderPath = $FolderLocation + "\" + $FolderName

    $CurrentAccess = Get-Acl $FolderPath

    $FileSystemRights = "ReadAndExecute"
    $InheritanceFlags = "ContainerInherit,ObjectInherit" 
    $PropagationFlags = "None"
    $AccessControlType = "Allow"

    $NewRule = New-Object system.security.accesscontrol.filesystemaccessrule($Name,$FileSystemRights,$InheritanceFlags,$PropagationFlags,$AccessControlType)
    $CurrentAccess.SetAccessRule($NewRule)
    Set-Acl $FolderPath $CurrentAccess
}

function Grant-Right {
    param([string]$Domain, [string]$GroupName, [string]$Right)

    $FullAccountName = $DomainName + "\" + $GroupName
    Grant-UserRight -Account $FullAccountName -Right $Right
}

function New-TaskUser {
    param([string]$UserName, [string]$Password, [string]$AppFolderName)

    
    New-User -UserName $UserName -Password $Password -GroupName "TaskAccounts" -AppFolderLocation "C:\Services" -AppFolderName $AppFolderName
}

function Create-Directory {
    param([string]$Path, [string]$Name)

    $FullPath = $Path + "\" + $Name
    $DirectoryExists = Test-Path $FullPath

    if(-Not $DirectoryExists){
        New-Item -Path $Path -Name $Name -ItemType "directory"
    }
}