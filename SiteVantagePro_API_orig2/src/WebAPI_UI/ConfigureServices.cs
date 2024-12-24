using Azure.Identity;
using SiteVantagePro_API.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SiteVantagePro_API.WebUI.Shared.Authorization;
using SiteVantagePro_API.Application.Common.Services.Identity;
using SiteVantagePro_API.WebAPI_UIServices;
using NSwag.Generation.Processors.Security;
using NSwag;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, FlexibleAuthorizationPolicyProvider>();

        //builder.Services.AddApplicationInsightsTelemetry();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customize default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        //--------------------------------
        // Register the Swagger generator, defining one or more Swagger documents
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument(configure =>
        {
            configure.Version = "v1";
            configure.Title = "SiteAdvantage Pro API";
            configure.Description = "ASP.NET 8.x API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        //services.AddOpenApiDocument(config =>
        //{
        //    config.PostProcess = document =>
        //    {
        //        document.Info.Version = "v1";
        //        document.Info.Title = "SiteVantagePro API";
        //        document.Info.Description = "ASP.NET 8.x API";
        //        //document.Info.TermsOfService = "None";
        //    };
        //});
        //--------------------------------

        //Seed Database records for MVC
        services.AddScoped<ApplicationDbContextInitialiser>();
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
