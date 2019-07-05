# Connect-AzAccount
# The SubscriptionId in which to create these objects

Param(
    $Tenant,
    $SubscriptionId,
    $ResourceGroupName= "PerfTestResourceGroup",
    $Location= "West US",
    $AdminSqlLogin,
    $Password,
    $ServerName = "testserverperf3",
    $DatabaseName = "austintestaccount"
)

try{
	Set-AzContext -SubscriptionId $subscriptionId 
}
catch{
	Connect-AzAccount -Tenant $Tenant -SubscriptionId $SubscriptionId
}

try{
	Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction Stop
	Write-Host "Resource group $ResourceGroupName exists"
}
catch{
	Write-Host "Started Creating resource group $ResourceGroupName"
	New-AzResourceGroup -Name $ResourceGroupName -Location $Location
	Write-Host "Finished Creating resource group $ResourceGroupName"
}

# Create a server with a system wide unique server name
Write-Host "Started Creating sql server with name $ServerName"
$server = New-AzSqlServer -ResourceGroupName $ResourceGroupName `
    -ServerName $ServerName `
    -Location $Location `
    -SqlAdministratorCredentials $(New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $AdminSqlLogin, $(ConvertTo-SecureString -String $Password -AsPlainText -Force))
Write-Host "Finished Creating sql server with name $ServerName"
# Create a server firewall rule that allows access from azure resources
Write-Host "Started Updating firewall rules"
Select-AzureSubscription -Default $SubscriptionId
New-AzureSqlDatabaseServerFirewallRule -ServerName $ServerName -AllowAllAzureServices
Write-Host "Finished updating firewall rules"
# Create a blank database with an S0 performance level
Write-Host "Started Creating database with name $DatabaseName"
$database = New-AzSqlDatabase  -ResourceGroupName $ResourceGroupName `
    -ServerName $ServerName `
    -DatabaseName $DatabaseName `
    -RequestedServiceObjectiveName "S0"
Write-Host "Finished Creating database with name $DatabaseName"
# Clean up deployment 
# Remove-AzResourceGroup -ResourceGroupName $resourceGroupName