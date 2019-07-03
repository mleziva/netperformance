<# Create an Azure Cosmos Account for Core (SQL) API

requirements:
Azure powershll
https://docs.microsoft.com/en-us/powershell/azure/install-az-ps?view=azps-2.4.0#requirements

#ensure we're logged in
try {
    $ctx=Get-AzureRmContext -ErrorAction Stop
}
catch {
    Login-AzureRmAccount -SubscriptionId $subscriptionId -TenantId $tenantId -ErrorAction Stop
}
if($ctx.Subscription.TenantId -ne $tenantId){
    Login-AzureRmAccount -SubscriptionName $subscriptionId -TenantId $tenantId -ErrorAction Stop
}

#>
Param(
    $Tenant,
    $SubscriptionId,
    $ResourceGroupName= "PerfTestResourceGroup",
    $Location= "West US",
    $AccountName = "austintestaccount"
)

Connect-AzAccount -Tenant $Tenant -SubscriptionId $SubscriptionId

try{
	Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction Stop
	Write-Host "Resource group $ResourceGroupName exists"
}
catch{
	Write-Host "Started Creating resource group $ResourceGroupName"
	New-AzResourceGroup -Name $ResourceGroupName -Location $Location
	Write-Host "Finished Creating resource group $ResourceGroupName"
}
$locations = @(
    @{ "locationName"="West US"; "failoverPriority"=0 },
    @{ "locationName"="East US"; "failoverPriority"=1 }
)

$consistencyPolicy = @{
    "defaultConsistencyLevel"="BoundedStaleness";
    "maxIntervalInSeconds"=300;
    "maxStalenessPrefix"=100000
}

$CosmosDBProperties = @{
    "databaseAccountOfferType"="Standard";
    "locations"=$locations;
    "consistencyPolicy"=$consistencyPolicy;
    "enableMultipleWriteLocations"="true"
}

Write-Host "Started Creating document db with account $AccountName"
New-AzResource -ResourceType "Microsoft.DocumentDb/databaseAccounts" `
    -ApiVersion "2015-04-08" -ResourceGroupName $ResourceGroupName -Location $Location `
    -Name $AccountName -PropertyObject $CosmosDBProperties
Write-Host "Finished Creating document db with account $AccountName"