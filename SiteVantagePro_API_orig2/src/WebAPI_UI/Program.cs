public partial class Program
{
    public static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddKeyVaultIfConfigured(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddWebServices();

        var app = builder.Build();

        //Initialise and seed the database
        try
        {
            await Task.Delay(5); //only used so I can comment out below when needed ... after initial deployment
            //await app.InitialiseDatabaseAsync();
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
            //app.UseWebAssemblyDebugging();
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

        //--https://stackoverflow.com/questions/51710388/nswag-net-core-api-versioning-configuration
        app.UseOpenApi();
        app.UseSwaggerUi3(settings =>
        {
            settings.CustomStylesheetPath = "/swagger/ui/custom.css";
        });

        //app.UseFileServer();
        //app.UseRouting();
        app.UseCors("CorsPolicy");

        //app.UseAuthentication();
        //app.UseAuthorization();
        app.UseSession();

        app.MapDefaultControllerRoute();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.UseExceptionHandler(options => { });

        app.MapEndpoints();

        app.MapRazorPages();

        app.Run();
    }
}

public partial class Program { }
