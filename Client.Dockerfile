FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

# Copy project files for restore
COPY Client/Client.csproj ./Client/
COPY Shared/Shared.csproj ./Shared/

# Restore dependencies
RUN dotnet restore ./Client/Client.csproj

# Copy source code
COPY Client/ ./Client/
COPY Shared/ ./Shared/

# Build and publish
WORKDIR /app/Client
RUN dotnet publish "Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM nginx:stable-alpine
WORKDIR /app
EXPOSE 8080

# Install jq for JSON manipulation
RUN apk add --no-cache jq

# Copy nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Copy published app
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copy startup script
COPY <<EOF /docker-entrypoint.d/30-update-config.sh
#!/bin/sh
set -e

echo "Starting configuration update..."

# Check if appsettings.json exists
if [ ! -f "/usr/share/nginx/html/appsettings.json" ]; then
    echo "ERROR: appsettings.json not found!"
    exit 1
fi

echo "Original appsettings.json:"
cat /usr/share/nginx/html/appsettings.json

# Update appsettings.json with environment variables if any are provided
if [ -n "\$API_URL" ] || [ -n "\$KEYCLOAK_URL" ] || [ -n "\$KEYCLOAK_REALM" ] || [ -n "\$KEYCLOAK_CLIENT_ID" ]; then
    echo "Updating appsettings.json with environment variables..."
    echo "API_URL=\$API_URL"
    echo "KEYCLOAK_URL=\$KEYCLOAK_URL"
    echo "KEYCLOAK_REALM=\$KEYCLOAK_REALM"
    echo "KEYCLOAK_CLIENT_ID=\$KEYCLOAK_CLIENT_ID"
    
    # Create temporary config with environment variables (ensuring proper JSON format)
    if jq ". + {
        \"ApiUrl\": (\$ENV.API_URL // .ApiUrl),
        \"KeycloakUrl\": (\$ENV.KEYCLOAK_URL // .KeycloakUrl),
        \"KeycloakRealm\": (\$ENV.KEYCLOAK_REALM // .KeycloakRealm // \"NationOH\"),
        \"KeycloakClientId\": (\$ENV.KEYCLOAK_CLIENT_ID // .KeycloakClientId // \"nationoh_client\")
    }" /usr/share/nginx/html/appsettings.json > /tmp/appsettings.json; then
        # Validate the generated JSON
        if jq empty /tmp/appsettings.json 2>/dev/null; then
            # Replace the original file
            mv /tmp/appsettings.json /usr/share/nginx/html/appsettings.json
            echo "Configuration updated successfully:"
            cat /usr/share/nginx/html/appsettings.json
        else
            echo "ERROR: Generated JSON is invalid, keeping original config"
            rm -f /tmp/appsettings.json
        fi
    else
        echo "ERROR: jq failed to process configuration, keeping original config"
        rm -f /tmp/appsettings.json
    fi
else
    echo "No environment variables provided, using default configuration"
fi

echo "Configuration setup completed"
EOF

# Make the script executable
RUN chmod +x /docker-entrypoint.d/30-update-config.sh