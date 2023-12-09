namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents contact information of a user or entity.
/// </summary>
public class ContactInfo
{
    /// <summary>
    /// The unique identifier for the contact information.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The full name of the contact person.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the contact person.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the contact person.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
}
