#!/bin/bash
# Script to configure Azure OIDC for Terraform GitHub Actions
# Usage: ./setup-azure-oidc.sh <AZURE_SUBSCRIPTION_ID> <AZURE_TENANT_ID> <AZURE_APP_NAME> <GITHUB_REPO>

set -euo pipefail

if [ $# -ne 4 ]; then
  echo "Usage: $0 <AZURE_SUBSCRIPTION_ID> <AZURE_TENANT_ID> <AZURE_APP_NAME> <GITHUB_REPO>"
  exit 1
fi

SUBSCRIPTION_ID=ce851b7b-7bc7-4917-9352-0ce88dec94d5
TENANT_ID=797f4846-ba00-4fd7-ba43-dac1f8f63013
APP_NAME="scaling-oidc"
# Use org/repo format for GITHUB_REPO, not full URL
GITHUB_REPO=$4

# 1. Create Azure AD App Registration (or use existing)
APP_ID=$(az ad app create --display-name "$APP_NAME" --query appId -o tsv)
APP_ID="${APP_ID//[[:space:]]/}"
echo "Created App Registration with AppId: $APP_ID"

# 2. Wait for Azure AD propagation before creating SP
echo "Waiting for app to be available before creating Service Principal..."

MAX_RETRIES=10
for i in $(seq 1 $MAX_RETRIES); do
  echo "🔍 Attempt $i: Checking AppId visibility in Graph..."
  if az ad app show --id "$APP_ID" &>/dev/null; then
    echo "App visible. Proceeding to create Service Principal..."
    break
  fi
  echo "App not visible yet. Retrying in 10 seconds..."
  sleep 10
done

# 3. Create Service Principal
if az ad sp create --id "$APP_ID"; then
  echo "✅ Service Principal created."
else
  echo "❌ Failed to create Service Principal. Exiting."
  exit 1
fi

# 4. Confirm SP created
az ad sp show --id "$APP_ID" --only-show-errors

# 5. Assign Contributor role
az role assignment create --assignee "$APP_ID" --role Contributor --scope "/subscriptions/$SUBSCRIPTION_ID"
echo "✅ Assigned Contributor role."

# 6. Add Federated Credential for GitHub OIDC (with required audiences)
az ad app federated-credential create --id "$APP_ID" \
  --parameters '{
    "name": "github-oidc",
    "issuer": "https://token.actions.githubusercontent.com",
    "subject": "repo:'"$GITHUB_REPO"':ref:refs/heads/main",
    "description": "GitHub Actions OIDC for $GITHUB_REPO",
    "audiences": ["api://AzureADTokenExchange"]
  }'


echo "✅ Added federated credential."

# 7. Output GitHub secrets
echo ""
echo "🔐 Add these secrets to your GitHub repo (Settings > Secrets > Actions):"
echo "  AZURE_CLIENT_ID=$APP_ID"
echo "  AZURE_TENANT_ID=$TENANT_ID"
echo "  AZURE_SUBSCRIPTION_ID=$SUBSCRIPTION_ID"
