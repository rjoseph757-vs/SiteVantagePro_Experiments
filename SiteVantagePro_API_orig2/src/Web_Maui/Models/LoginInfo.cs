using System.ComponentModel.DataAnnotations;

namespace Web_Maui.Models;
public class LoginInfo
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    //[Password]
    public string Password { get; set; } = string.Empty;
}
