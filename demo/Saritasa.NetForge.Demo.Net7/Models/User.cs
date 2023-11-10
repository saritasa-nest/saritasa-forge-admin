﻿using Microsoft.AspNetCore.Identity;

namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents the application's user.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// Gets or sets the date of birth of the user.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the age of the user.
    /// </summary>
    public int? Age => DateOfBirth == null ? null : DateTime.Now.Subtract((DateTime)DateOfBirth).Days / 364;
}
