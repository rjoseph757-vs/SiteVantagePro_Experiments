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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            }
        );

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Stores.MaxLengthForKeys = 128;
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 3;
        })
        //.AddDefaultUI() --removed since we scaffolded the Pages for register/login, etc. under Areas
        .AddSignInManager()
        .AddRoles<ApplicationRole>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>>();

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(2);
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

        });

        // Configure JWT authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = bool.Parse(configuration["JWTSettings:ValidateIssuerSigningKey"]),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:IssuerSigningKey"])),
                ValidateIssuer = bool.Parse(configuration["JWTSettings:ValidateIssuer"]),
                ValidAudience = configuration["JWTSettings:ValidAudience"],
                ValidIssuer = configuration["JWTSettings:ValidIssuer"],
                ValidateAudience = bool.Parse(configuration["JWTSettings:ValidateAudience"]),
                RequireExpirationTime = bool.Parse(configuration["JWTSettings:RequireExpirationTime"]),
                ValidateLifetime = bool.Parse(configuration["JWTSettings:ValidateLifetime"])
            };
        });

        services.AddScoped(sp =>
            (IApplicationDbContext)sp.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IEmailSenderUI, EmailSenderUI>();
        services.AddTransient<IEmailSenderService, SendGridEmailService>();

        services.ConfigureApplicationCookie(o =>
        {
            o.ExpireTimeSpan = TimeSpan.FromDays(5);
            o.SlidingExpiration = true;
        });

        services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));

        //services.AddAuthorizationBuilder()
        //.AddPolicy(PoliciesConstants.Admin, policy => policy.RequireRole(RolesConstants.Admin));

        //services.AddAuthorizationBuilder()
        //.AddPolicy(PoliciesConstants.API, policy => policy.RequireRole(RolesConstants.API));

        services.AddAuthorizationBuilder()
                .AddPolicy(PoliciesConstants.CanPurge, policy => policy.RequireRole(RolesConstants.Admin));

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
