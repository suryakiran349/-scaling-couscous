# Development Environment Variables - Consolidated Infrastructure

# Basic Configuration
resource_group_name = "nohdevoct2025"
location            = "uksouth"
environment         = "dev"

# GitHub Actions Service Principal Object ID for Key Vault access
github_actions_service_principal_object_id = "019d3538-07e6-4a12-8c97-5102eaa66c6b"
# Container Configuration
container_cpu          = 0.5
container_memory       = "1Gi"
aspnetcore_environment = "Development"

# Container Images (applying commit 244a9e0 changes - using actual images instead of nginx)
server_image   = "nohdevoct2025registry.azurecr.io/app.server:latest"
frontend_image = "nohdevoct2025registry.azurecr.io/app.client:latest"
# Allowed Hosts
allowed_hosts = "*"

# Image Tags
image_tags = {
  keycloak = "26.1"
}

# Application Gateway Configuration
app_gateway_sku_tier = "WAF_v2"

# Subscription ID
subscription_id = "ce851b7b-7bc7-4917-9352-0ce88dec94d5"