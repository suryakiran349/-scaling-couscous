variable "key_vault_name" {
  description = "Key vault name"
  type        = string
}

variable "location" {
  description = "Location for the resources"
  type        = string
}

variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
}

variable "subnet_ids" {
  description = "Map of subnet names to subnet IDs"
  type        = map(string)
}

variable "vnet_id" {
  description = "Virtual network ID"
  type        = string
}

variable "keycloak_managed_identity_object_id" {
  description = "Object ID of the Keycloak managed identity for key vault access"
  type        = string
  default     = "" # Default to empty to make it optional
}

variable "github_actions_service_principal_object_id" {
  description = "Object ID of the GitHub Actions service principal for key vault access"
  type        = string
  default     = "4e4da53e-e85a-430f-9bcf-168ca0d53bc6" # Default to empty to make it optional
}