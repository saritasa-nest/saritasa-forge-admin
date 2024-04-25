using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Model to store information about an error related to a component property.
/// </summary>
public class ComponentErrorModel
{
    /// <summary>
    /// Metadata information about the property associated with the error.
    /// </summary>
    public PropertyMetadataDto Property { get; set; } = null!;

    /// <summary>
    /// Error message describing the issue with the associated property.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Indicating whether there is an error associated with the property.
    /// </summary>
    public bool IsError
    {
        get => !string.IsNullOrEmpty(ErrorMessage);
    }
}
