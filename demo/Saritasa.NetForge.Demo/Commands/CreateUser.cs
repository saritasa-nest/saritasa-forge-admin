using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Identity;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Commands;

/// <summary>
/// Create user command.
/// </summary>
[HelpOption]
[Command("create-user", Description = "Create user.")]
public class CreateUser
{
    private readonly ILogger<CreateUser> logger;
    private readonly UserManager<User> userManager;

    /// <summary>
    /// First name.
    /// </summary>
    [Option("--first-name", Description = "First name.")]
    [Required]
    public required string FirstName { get; init; }

    /// <summary>
    /// Last name.
    /// </summary>
    [Option("--last-name", Description = "Last name.")]
    [Required]
    public required string LastName { get; init; }

    /// <summary>
    /// Email.
    /// </summary>
    [Option("--email", Description = "User email for login.")]
    [Required]
    public required string Email { get; init; }

    /// <summary>
    /// Password.
    /// </summary>
    [Option("--password", Description = "Password.")]
    [Required]
    public required string Password { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="userManager">User manager.</param>
    public CreateUser(ILogger<CreateUser> logger, UserManager<User> userManager)
    {
        this.logger = logger;
        this.userManager = userManager;
    }

    /// <summary>
    /// Command line application execution callback.
    /// </summary>
    public async Task OnExecuteAsync()
    {
        var user = new User
        {
            Email = Email,
            UserName = Email,
            EmailConfirmed = true,
            FirstName = FirstName,
            LastName = LastName
        };
        
        var result = await userManager.CreateAsync(user, Password);
        logger.LogInformation("User creation result: {Result}.", result);
        
        if (result.Succeeded)
        {
            logger.LogInformation("User id: {UserId}.", user.Id);
        }
    }
}