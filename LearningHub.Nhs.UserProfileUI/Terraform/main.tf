resource "azurerm_resource_group" "UserProfileResourceGroup" {
    name        = var.ResourceGroupName
    location    = var.ResourceGroupLocation
}

resource "azurerm_service_plan" "UserProfileServicePlan" {
  name                = var.AppServicePlanName
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  sku_name			  = "B1"
  os_type			  = "Windows"
}

/*resource "azurerm_linux_web_app" "UserProfileLinuxWebApp" {
  name                = var.WebAppName
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  service_plan_id     = azurerm_service_plan.UserProfileServicePlan.id
  site_config {
	app_command_line  = "dotnet Learninghub.Nhs.UserProfileUI.dll"
	application_stack {
	  dotnet_version = "6.0"
	}
  }
}*/

resource "azurerm_windows_web_app" "UserProfileLinuxWebApp" {
  name                = var.WebAppName
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