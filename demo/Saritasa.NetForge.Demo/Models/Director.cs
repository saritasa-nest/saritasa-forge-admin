using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Domain.Attributes;

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
    [NetForgeProperty(IsSortable = true)]
    public required string Name { get; set; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Age.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Department.
    /// </summary>
    public Department Department { get; set; }

    /// <summary>
    /// Photo.
    /// </summary>
    public string? Photo { get; set; }

    /// <summary>
    /// Birthday.
    /// </summary>
    public DateOnly Birthday { get; set; }

    /// <summary>
    /// First work day as director.
    /// </summary>
    public DateTime DirectorSince { get; set; }

    /// <summary>
    /// Last work day with timezone info.
    /// </summary>
    public DateTimeOffset? LastWorkDay { get; set; }

    /// <summary>
    /// Start work time.
    /// </summary>
    public TimeOnly StartWorkTime { get; set; }

    /// <summary>
    /// Address.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Company.
    /// </summary>
    public required Company Company { get; set; }
}