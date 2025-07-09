FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY Client/Client.sln ./Client/
COPY ./Client/Client.csproj ./Client/
COPY ./Shared/Shared.csproj ./Shared/

RUN dotnet restore ./Client/Client.sln
COPY . .
WORKDIR /app/Client
RUN dotnet publish "./Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish

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

# Update appsettings.json with environment variables
if [ -n "\$API_URL" ] || [ -n "\$KEYCLOAK_URL" ]; then
    echo "Updating appsettings.json with environment variables..."
    
    # Create temporary config with environment variables
    jq ". + {
        \"ApiUrl\": (\$ENV.API_URL // .ApiUrl),
        \"KeycloakUrl\": (\$ENV.KEYCLOAK_URL // .KeycloakUrl),
        \"KeycloakRealm\": (\$ENV.KEYCLOAK_REALM // .KeycloakRealm // \"NationOH\"),
        \"KeycloakClientId\": (\$ENV.KEYCLOAK_CLIENT_ID // .KeycloakClientId // \"nationoh_client\")
    }" /usr/share/nginx/html/appsettings.json > /tmp/appsettings.json
    
    # Replace the original file
    mv /tmp/appsettings.json /usr/share/nginx/html/appsettings.json
    
    echo "Configuration updated:"
    cat /usr/share/nginx/html/appsettings.json
fi
EOF

# Make the script executable
RUN chmod +x /docker-entrypoint.d/30-update-config.sh