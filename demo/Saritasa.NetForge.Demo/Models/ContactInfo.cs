﻿using System.ComponentModel.DataAnnotations.Schema;
using Saritasa.NetForge.Domain.Attributes;
using Saritasa.NetForge.Demo.Constants;
using Saritasa.NetForge.Domain.Enums;

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
    [NetForgeProperty(SearchType = SearchType.ContainsCaseInsensitive)]
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
    [NetForgeProperty(SearchType = SearchType.ContainsCaseInsensitive)]
    public required string PhoneNumber { get; set; }
    
    /// <summary>
    /// The duration of the contact's availability.
    /// </summary>
    public TimeSpan? AvailabilityDuration { get; set; }
    
    /// <summary>
    /// Date when contact was created.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FullName} - {Email}";
    }
}

