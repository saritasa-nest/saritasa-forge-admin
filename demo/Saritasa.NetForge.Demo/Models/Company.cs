using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Company.
/// </summary>
[Owned]
public class Company
{
    /// <summary>
    /// Name.
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Employee count.
    /// </summary>
    public int EmployeeCount { get; set; }
}