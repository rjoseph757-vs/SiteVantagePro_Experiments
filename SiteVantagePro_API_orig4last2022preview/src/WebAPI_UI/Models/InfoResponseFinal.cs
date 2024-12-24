namespace SiteVantagePro_API.WebAPI_UI.Models;

public class InfoResponseFinal
{
    /// <summary>
    /// The email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// A value indicating whether the email has been confirmed yet.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }
}
