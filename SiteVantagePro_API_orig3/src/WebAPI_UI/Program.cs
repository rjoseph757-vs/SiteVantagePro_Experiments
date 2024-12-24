using System.Security.Claims;

public partial class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddKeyVaultIfConfigured(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddWebServices(builder.Configuration);

        var app = builder.Build();

        //Initialise and seed the database
        try
        {
            await Task.Delay(1); //only used so I can comment out below when needed ... after initial deployment
            await app.InitializeDatabaseAsync();
        }
        catch (Exception)
        {
            //logger.LogError(ex, "An error occurred during database initialization.");
            throw;
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();// for Blazor apps?
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();
        app.UseStaticFiles();

       
        //app.UseRouting();
        app.UseCors("WasmCorsPolicy");

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            //options.RoutePrefix = "swagger"; //default
        });

        app.UseAuthentication();
        app.UseAuthorization();
        //app.UseSession();

        app.MapDefaultControllerRoute();
        app.MapControllers();
        app.MapRazorPages();

        app.MapFallbackToFile("index.html");

        app.UseExceptionHandler(options => { });

        app.MapEndpoints(); //Kickoff 'Extension method' to 'Endpoints' folder for structured code

        //for Blazor using Cookies
        // provide an end point to clear the cookie for logout
        app.MapPost("/Users/Logout", async (ClaimsPrincipal user, Microsoft.AspNetCore.Identity.SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        });

        ///*
        ////---------------------------------------------------------------------------
        ////Get the ApplicationUser as a 'MapGroup' set of endpoints
        ////app.MapGroup("/Users")
        ////  .MapIdentityApi<ApplicationUser>();


        //// Test using policy authorization (RequireAuthorization!!!)
        //app.MapGet("v1/api/foo", () =>
        //{
        //    return new[] { "One", "Two", "Three" };
        //})
        //.RequireAuthorization("api"); //policy defined above

        //// Apply the AdminOnly policy to this endpoint using 'decorator pattern' ... works in Controllers the same way!
        //app.MapGet("SuperAdminOnly", [Authorize(Policy = "SuperAdminOnly")] () => "Hello Super Admin!").RequireAuthorization();
        ////---------------------------------------------------------------------------
        //*/

        app.Run();
    }

   
}

public partial class Program { }
