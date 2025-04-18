using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Director.
/// </summary>
[Owned]
public class Director
{
    /// <summary>
    /// Name.
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Age.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Is active.
    /// </summary>
    public bool IsActive { get; set; }

    // public DateTime Birthday { get; set; }
}