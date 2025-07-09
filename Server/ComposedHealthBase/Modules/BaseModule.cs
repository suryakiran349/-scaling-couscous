
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection;

namespace ComposedHealthBase.Server.Modules
{
	public class BaseModule : IModule
	{
		public IServiceCollection RegisterModuleServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddDatabaseDeveloperPageExceptionFilter();		// CORS configuration - read from environment variables
		var corsAllowedOrigins = configuration["Cors:AllowedOrigins"] ?? "http://localhost:5002";
		var corsOrigins = corsAllowedOrigins.Split(',', StringSplitOptions.RemoveEmptyEntries);
		
		services.AddCors(options =>
		{
			options.AddPolicy("Client",
				policy => policy
					.WithOrigins(corsOrigins)
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials());
		});

		// JWT configuration - read from IdentityConfig section
		bool.TryParse(configuration["IdentityConfig:RequireHttpsMetadata"], out bool requireHttpsMetadata);

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = configuration["IdentityConfig:Issuer"];
				options.Audience = configuration["IdentityConfig:Audience"];
				options.RequireHttpsMetadata = requireHttpsMetadata;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true
					};
				});
		services.AddAuthorization();
		
		// Add health checks for container health probes
		services.AddHealthChecks();
		
		services.AddOpenApi();

			return services;
		}

	public WebApplication ConfigureModuleServices(WebApplication app, bool isDevelopment)
	{
		app.UseCors("Client");
		app.UseAuthentication();
		app.UseAuthorization();
		
		// Map health check endpoints for container probes
		app.MapHealthChecks("/health");
		app.MapHealthChecks("/health/ready");
		
		if (isDevelopment)
		{
			app.MapOpenApi();
			app.MapScalarApiReference();
		}
		else
		{
			app.UseHttpsRedirection();
		}
		return app;
	}
	}
}