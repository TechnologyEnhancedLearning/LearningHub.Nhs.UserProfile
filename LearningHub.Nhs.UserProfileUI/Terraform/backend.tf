terraform {
  required_providers {
    azurerm = {
        source  = "hashicorp/azurerm"
        version = "=3.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name     = "TerraformStorageRG"
    storage_account_name    = "userprofilesa11"
    container_name          = "tfstate"
    key                     = "userprofile.terraform.tfstate"
  }
}
provider "azurerm" {
  features {}
}