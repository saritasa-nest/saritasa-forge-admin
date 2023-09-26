using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builder class for configuring entity options within the admin panel.
/// </summary>
/// <typeparam name="TEntity">The type of entity for which options are being configured.</typeparam>
public class EntityOptionsBuilder<TEntity> where TEntity : class
{
    private readonly EntityOptions options = new(typeof(TEntity));

    /// <summary>
    /// Sets the description for the entity being configured.
    /// </summary>
    /// <param name="description">The description to set for the entity.</param>
    public EntityOptionsBuilder<TEntity> SetDescription(string description)
    {
        options.Description = description;
        return this;
    }

    /// <summary>
    /// Creates and returns the configured entity options.
    /// </summary>
    public EntityOptions Create()
    {
        return options;
    }
}
