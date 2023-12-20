using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Demo.ViewModels;

/// <summary>
/// View model for user login.
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// Username of user.
    /// </summary>
    [Display(Name = "Username")]
    [Required(ErrorMessage = "Username is required.")]
    public required string Username { get; set; }

    /// <summary>
    /// Password of user.
    /// </summary>
    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required.")]
    public required string Password { get; set; }

    /// <summary>
    /// Value indicating whether the user's authentication should be remembered.
    /// </summary>
    public bool RememberMe { get; set; }
}