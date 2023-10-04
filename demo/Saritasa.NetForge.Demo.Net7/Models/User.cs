using Microsoft.AspNetCore.Identity;

namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents the application's user.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}
