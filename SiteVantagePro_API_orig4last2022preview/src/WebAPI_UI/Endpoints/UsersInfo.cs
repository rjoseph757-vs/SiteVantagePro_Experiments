using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiteVantagePro_API.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using SiteVantagePro_API.Domain.Entities;
using SiteVantagePro_API.WebAPI_UI.Models;
using System.Security.Claims;

namespace SiteVantagePro_API.Web.Endpoints;

public class UsersInfo : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(nameof(UsersInfo))
            .MapGet(GetWeatherForecasts)
            .MapPost("Users/Logout")
            .MapGet("Users/manage/infonew";
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherForecasts(ISender sender)
    {
        return await sender.Send(new GetWeatherForecastsQuery());
    }

    // provide an end point to clear the cookie for logout
    [HttpPost]
    public async Task<InfoResponseFinal> Logout(ClaimsPrincipal user, SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        });

        public async Task<Results<Ok<InfoResponseFinal>>> ManageInfoNew(ClaimsPrincipal claimsPrincipal, UserManager<ApplicationUser> userManager) =>
        {
            if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
        });

private static async Task<InfoResponseFinal> CreateInfoResponseAsync(ClaimsPrincipal user, UserManager<ApplicationUser> userManager)
{
    return new()
    {
        Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
        IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user)
    };
}

private static async Task<InfoResponseFinal> CreateInfoResponseAsync<ClaimsPrincipal>(ClaimsPrincipal user, UserManager<ApplicationUser> userManager)
{
    return new InfoResponseFinal()
    {
        Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
        IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
        Claims = null
    };
}
}

}
