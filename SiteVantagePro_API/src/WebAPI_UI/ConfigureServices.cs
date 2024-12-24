using Azure.Identity;
using SiteVantagePro_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.WebAPI_UIServices;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using WebAPI_UI.Components.Account;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddRazorComponents()
        //    .AddInteractiveServerComponents()
        //    .AddInteractiveWebAssemblyComponents();

        //services.AddCascadingAuthenticationState();
        //services.AddScoped<IdentityUserAccessor>();
        //services.AddScoped<IdentityRedirectManager>();
        //services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

        //services.AddAuthentication(options =>
        //{
        //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
        //    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        //})
        //    .AddIdentityCookies();


        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Jason Taylor's Flexible Authorization Policy implementation
        //services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        //services.AddSingleton<IAuthorizationPolicyProvider, FlexibleAuthorizationPolicyProvider>();

        //builder.Services.AddApplicationInsightsTelemetry();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customize default API behaviour ???
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // add CORS policy for Wasm client
        services.AddCors(
            options => options.AddPolicy(
                "WasmCorsPolicy",
                 policy => policy.WithOrigins([configuration["BackendUrl"] ?? "https://localhost:5001",
                     configuration["FrontendUrl"] ?? "https://localhost:5002"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));


        //services.Configure<ApiBehaviorOptions>(options =>
        //    options.SuppressModelStateInvalidFilter = true);

        //-------------------------------------------------------------------------------
        // Register the Swagger generator, defining one or more Swagger documents
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "SiteVantagePro API", Version = "v1" });
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter ONLY the Login token in the text input below.\r\n\r\nExample: \"12345abcdef\""
            });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                        }
                    });
        });

        //Seed Database records for MVC
        services.AddScoped<ApplicationDbContextInitializer>();
        return services;
    }

    public static IServiceCollection AddKeyVaultIfConfigured(this IServiceCollection services, ConfigurationManager configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }

        return services;
    }
}
