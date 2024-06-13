using System.ComponentModel.DataAnnotations;

namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents contact information of a user or entity.
/// </summary>
internal class ContactInfo
{
    /// <summary>
    /// The unique identifier for the contact information.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The full name of the contact person.
    /// </summary>
    [MinLength(5)]
    [MaxLength(10)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the contact person.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the contact person.
    /// </summary>
    [StringLength(12)]
    [PhoneMask("dddd-ddd-ddd", ErrorMessage = "The phone number have wrong format, format is: dddd-ddd-ddd")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FullName} - {Email}";
    }
}
