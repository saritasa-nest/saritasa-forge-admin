namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Dummy model to dynamic add properties.
/// </summary>
internal class Dummy
{
    [PhoneMask("dddd-ddd-ddd", ErrorMessage = "{0} value does not match the mask {1}.")]
    public string Phone { get; set; }
}
