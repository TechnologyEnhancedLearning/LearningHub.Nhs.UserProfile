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