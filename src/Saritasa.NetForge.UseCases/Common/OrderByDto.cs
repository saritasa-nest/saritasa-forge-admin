namespace Saritasa.NetForge.UseCases.Common;

/// <summary>
/// DTO for ordering.
/// </summary>
public record OrderByDto
{
    /// <summary>
    /// Order by field.
    /// </summary>
    public string FieldName { get; init; } = string.Empty;

    /// <summary>
    /// Direction of order.
    /// </summary>
    public bool IsDescending { get; init; }
}
