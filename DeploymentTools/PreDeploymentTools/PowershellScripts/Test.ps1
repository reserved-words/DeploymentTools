param($DirName1, $DirName2)

Import-Module $PSScriptRoot\SetupTools.psm1

Create-Directory -Path "C:\" -Name $DirName1

$NewDirectory = "C:\" + $DirName1

Create-Directory -Path $NewDirectory -Name $DirName2