using Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Client.RoleManagement;
using MudBlazor.Services;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load configuration from appsettings.json
var config = builder.Configuration;
var apiUrl = config["ApiUrl"] ?? "http://localhost:5003";
var keycloakUrl = config["KeycloakUrl"] ?? "http://localhost:8180";
var keycloakRealm = config["KeycloakRealm"] ?? "NationOH";
var keycloakClientId = config["KeycloakClientId"] ?? "nationoh_client";

// Configure API client
builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiUrl))
   .AddHttpMessageHandler(sp =>
   {
	   var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
		   .ConfigureHandler(authorizedUrls: new[] { apiUrl });
	   return handler;
   });

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("api"));

// Configure OIDC authentication with environment-specific URLs
builder.Services.AddOidcAuthentication(options =>
{
	options.ProviderOptions.Authority = $"{keycloakUrl}/realms/{keycloakRealm}";
	options.ProviderOptions.ClientId = keycloakClientId;
	options.ProviderOptions.ResponseType = "code";
	options.ProviderOptions.DefaultScopes.Add("nationoh_webapi-scope");
	options.UserOptions.RoleClaim = "role";
}).AddAccountClaimsPrincipalFactory<ParseRoleClaimsPrincipalFactory>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();