terraform {
  backend "azurerm" {
    resource_group_name  = "terraform-state-rg-latest"
    storage_account_name = "nationohtfstatejun2025"
    container_name       = "tfstate"
    key                  = "nationoh/dev.tfstate.latest"
    use_azuread_auth     = true
    use_oidc             = true
  }
}
