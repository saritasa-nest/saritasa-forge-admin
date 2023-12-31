﻿using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Domain.Attributes;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Represents contact information of a user or entity.
/// </summary>
[NetForgeEntity(PluralName = "Contact Information", GroupName = GroupConstants.Shops)]
public class ContactInfo
{
    /// <summary>
    /// The unique identifier for the contact information.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The full name of the contact person.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// The email address of the contact person.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The phone number of the contact person.
    /// </summary>
    public required string PhoneNumber { get; set; }
}

