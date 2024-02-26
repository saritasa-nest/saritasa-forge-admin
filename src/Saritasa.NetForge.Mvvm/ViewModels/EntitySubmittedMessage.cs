namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Message about entity submit. For example: create or edit entity.
/// </summary>
public record EntitySubmittedMessage
{
    /// <summary>
    /// Is any of recipients raised an error.
    /// </summary>
    public bool HasErrors { get; set; }
}
