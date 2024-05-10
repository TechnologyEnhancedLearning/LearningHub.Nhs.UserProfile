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

resource "azurerm_application_gateway" "ApplicationGateway" {
  name                = "UKS-ELFH-DEV-AG03"   # Parameterize this
  resource_group_name = "UKS-ELFH-APPGW-RG"   # Parameterize this
  location            = "uksouth"             # Parameterize this

  sku {
    name     = "WAF_v2"
    tier     = "WAF_v2"
    capacity = 2
  }

  gateway_ip_configuration {
    name      = "appGatewayIpConfig"
    subnet_id = "/subscriptions/66516f71-f3d4-4911-b900-c6e4690a5b15/resourceGroups/UKS-ELFH-DEVNET-RG/providers/Microsoft.Network/virtualNetworks/UKS-ELFH-DEVNET-VNET/subnets/AGV2-SN"
  }

  frontend_port {
    name = "FrontendPort"
    port = 443
  }

  frontend_ip_configuration {
    name                 = "appGwPublicFrontendIp"
    public_ip_address_id = azurerm_public_ip.pip.id
  }

  backend_address_pool {
    name = "HTTPS.userprofile-dev.test-learninghub.org.uk" # Parameterize this
  }

  backend_http_settings {
    name                  = "HTTPS.userprofile-dev.test-learninghub.org.uk" # Parameterize this
    cookie_based_affinity = "Disabled"
    port                  = 443
    protocol              = "Https"
    request_timeout       = 60
  }

  

  http_listener {
    name                           = "HTTPS.userprofile-dev.test-learninghub.org.uk" # Parameterize this
    frontend_ip_configuration_name = "appGwPublicFrontendIp"
    frontend_port_name             = "FrontendPort"
    protocol                       = "Https"
  }

  ssl_certificate {
    name = "test-learninghub-nhs-org"
    key_vault_secret_id = data.azurerm_key_vault_secret.uk_learninghub_kv.id
  }

  request_routing_rule {
    name                       = "HTTPS.userprofile-dev.test-learninghub.org.uk" # Parameterize this
    rule_type                  = "Basic"
    http_listener_name         = "HTTPS.userprofile-dev.test-learninghub.org.uk"
    backend_address_pool_name  = "HTTPS.userprofile-dev.test-learninghub.org.uk"
    backend_http_settings_name = "HTTPS.userprofile-dev.test-learninghub.org.uk"
    priority                   = 40
  }
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