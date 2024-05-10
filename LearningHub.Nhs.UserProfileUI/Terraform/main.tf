resource "azurerm_dns_cname_record" "UserProfileDNSRecord" {
  name                = var.DNSRecordName
  resource_group_name = var.DNSZoneResourceGroupName
  zone_name           = var.DNSZoneName
  ttl                 = 3600
  record              = var.DNSRecordValue
}

resource "azurerm_public_ip" "pip" {
  name                = "UKS-ELFH-DEV-AG03-PIP"
  resource_group_name = "UKS-ELFH-APPGW-RG"
  location            = "uksouth"
  allocation_method   = "Static"
  sku                 = "Standard"
}

data "azurerm_key_vault_secret" "uk_learninghub_kv" {
  name                = "test-learninghub-nhs-org"
  key_vault_id        = "/subscriptions/66516f71-f3d4-4911-b900-c6e4690a5b15/resourceGroups/UKS-ELFH-DEVLEARNINGHUBNHSUK-RG/providers/Microsoft.KeyVault/vaults/UKS-DEVLEARNINGHUB-KV"
}

resource "azurerm_resource_group" "UserProfileResourceGroup" {
    name        = "UserProfileRG"
    location    = "uksouth"
}

resource "azurerm_service_plan" "UserProfileServicePlan" {
  name                = "learninghub-userprofile-app-service-plan"
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  sku_name			  = "B1"
  os_type			  = "Linux"
}

resource "azurerm_linux_web_app" "UserProfileLinuxWebApp" {
  name                = "learninghub-userprofile-app"
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  service_plan_id     = azurerm_service_plan.UserProfileServicePlan.id
  site_config {
	app_command_line  = "dotnet Learninghub.Nhs.UserProfileUI.dll"
	application_stack {
	  dotnet_version = "6.0"
	}
  }
}