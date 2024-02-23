resource "azurerm_resource_group" "UserProfileResourceGroup" {
    name        = var.ResourceGroupName
    location    = var.ResourceGroupLocation
}

resource "azurerm_service_plan" "UserProfileServicePlan" {
  name                = "userprofile-app-service-plan"
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  sku_name			  = "B1"
  os_type			  = "Linux"
}

resource "azurerm_linux_web_app" "UserProfileLinuxWebApp" {
  name                = "userprofile-app"
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

resource "azurerm_service_plan" "LearningCredentialsServicePlan" {
  name                = "learningcredentials-app-service-plan"
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  sku_name			  = "B1"
  os_type			  = "Linux"
}

resource "azurerm_linux_web_app" "LearningCredentialsLinuxWebApp" {
  name                = "learningcredentials-app"
  location            = azurerm_resource_group.UserProfileResourceGroup.location
  resource_group_name = azurerm_resource_group.UserProfileResourceGroup.name
  service_plan_id     = azurerm_service_plan.LearningCredentialsServicePlan.id
  site_config {
	app_command_line  = "dotnet Learninghub.Nhs.LearningCredentialsUI.dll"
	application_stack {
	  dotnet_version = "6.0"
	}
  }
}

resource "azurerm_mssql_server" "LearningCredentialMssqlServer" {
  name                          = "learning-credentials-mssql-server"
  location                      = azurerm_resource_group.UserProfileResourceGroup.location
  version                       = "12.0"
  administrator_login           = "exampleadmin"
  administrator_login_password  = var.sql_admin_password
  resource_group_name		    = azurerm_resource_group.UserProfileResourceGroup.name
}

resource "azurerm_mssql_database" "LearningCredentialsMssqlDatabase" {
  name                = "LearningCredentials"
  collation           = "SQL_Latin1_General_CP1_CI_AS"
  server_id			  = azurerm_mssql_server.LearningCredentialMssqlServer.id
}