namespace Saritasa.NetForge.Domain.Entities.Options
{
/// <summary>
/// Represents a custom action that can be executed with a specified handler.
/// </summary>
public class CustomAction
{
    /// <summary>
    /// Gets or sets the name of the custom action.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the custom action.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the handler for the custom action.
    /// The handler is an action that takes an <see cref="IServiceProvider"/> and an <see cref="IQueryable{T}"/> of objects.
    /// </summary>
    public Action<IServiceProvider?, IQueryable<object>>? Handler { get; set; }
}
}
