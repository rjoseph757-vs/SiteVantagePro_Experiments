using SiteVantagePro_API.Domain.Constants;
using SiteVantagePro_API.Infrastructure.Data;
using SiteVantagePro_API.Infrastructure.Data.Interceptors;
using SiteVantagePro_API.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Infrastructure;
using Application.Common.Interfaces;
using SiteVantagePro_API.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SiteVantagePro_API.Infrastructure.Permissions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();
        services.AddScoped<SaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddControllersWithViews();
        services.AddRazorPages();

        // Configure the context to use SQL Database store.
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        //--https://github.com/iammukeshm/PermissionManagement.MVC/blob/master/PermissionManagement.MVC/Permission/PermissionAuthorizationHandler.cs
        //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Stores.MaxLengthForKeys = 128;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 3;
        })
        .AddApiEndpoints() //<<---- .NET 8
        .AddRoles<ApplicationRole>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        ;

        //services.AddIdentityCore<ApplicationUser>(options =>
        //{
        //    options.Stores.MaxLengthForKeys = 128;
        //    options.SignIn.RequireConfirmedAccount = true;
        //    options.SignIn.RequireConfirmedEmail = true;
        //    options.User.RequireUniqueEmail = true;
        //    options.Lockout.MaxFailedAccessAttempts = 3;
        //})
        ////.AddDefaultUI() --removed since we scaffolded the Pages for register/login, etc. under Areas
        //.AddRoles<ApplicationRole>()
        //.AddDefaultTokenProviders()
        //.AddEntityFrameworkStores<ApplicationDbContext>()
        //.AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

        //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

        //--https://stackoverflow.com/questions/72344638/i-get-401-when-i-combined-bearer-token-and-cookie-authentication-in-asp-net
        services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer("Bearer", options => { });

        var policy = new AuthorizationPolicyBuilder(IdentityConstants.ApplicationScheme, "Bearer") //"Identity.Application"
            .RequireAuthenticatedUser().Build();
        services.AddAuthorization(m => m.DefaultPolicy = policy);
        //------------------------------------------------------------------------------------------------------------------------------------


        services.AddAuthentication()
                .AddBearerToken(IdentityConstants.BearerScheme);

        // ------------------------------------------------------------------------------------------------------
        // Note: Policies that DON'T include Authentication Scheme = "IdentityConstants.BearerScheme" won't work!
        // ------------------------------------------------------------------------------------------------------

        // Establish policies
        services.AddAuthorizationBuilder()
          .AddPolicy(PoliciesConstants.api, policy =>
          {
              policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme); //MUST be added to work!!!
              policy.RequireAuthenticatedUser();
          });

        services.AddAuthorizationBuilder()
        .AddPolicy(PoliciesConstants.SuperAdminOnly, policy =>
            {
                policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme); //MUST be added to work!!!
                policy.RequireRole(RolesConstants.SuperAdmin);
            });

        services.AddAuthorizationBuilder()
                .AddPolicy(PoliciesConstants.CanPurge, policy =>
                {
                    policy.AddAuthenticationSchemes(IdentityConstants.BearerScheme); //MUST be added to work!!!
                    policy.RequireRole(RolesConstants.Admin);
                });

        //services.AddDistributedMemoryCache();

        //services.AddSession(options =>
        //{
        //    options.IdleTimeout = TimeSpan.FromMinutes(2);
        //    options.Cookie.HttpOnly = true;
        //    options.Cookie.SameSite = SameSiteMode.None;
        //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        //});

        services.AddScoped(sp =>
            (IApplicationDbContext)sp.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IEmailSenderUI, EmailSenderUI>(); //for UI
        services.AddTransient<IEmailSenderService, SendGridEmailService>(); //for Services

        services.ConfigureApplicationCookie(o =>
        {
            o.ExpireTimeSpan = TimeSpan.FromDays(5);
            o.SlidingExpiration = true;
        });

        services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));

        services.AddSingleton(TimeProvider.System);

        //services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
