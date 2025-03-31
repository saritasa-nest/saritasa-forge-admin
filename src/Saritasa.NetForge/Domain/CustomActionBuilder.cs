using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Domain;

/// <summary>
/// A builder class for creating and configuring <see cref="CustomAction{TEntity}"/> instances.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class CustomActionBuilder<TEntity> where TEntity : class
{
    private readonly CustomAction<TEntity> customAction = new();

    /// <summary>
    /// Sets the name of the custom action.
    /// </summary>
    /// <param name="name">The name of the custom action.</param>
    /// <returns>The current instance of <see cref="CustomActionBuilder{TEntity}"/>.</returns>
    public CustomActionBuilder<TEntity> SetName(string name)
    {
        customAction.Name = name;
        return this;
    }

    /// <summary>
    /// Sets the description of the custom action.
    /// </summary>
    /// <param name="description">The description of the custom action.</param>
    /// <returns>The current instance of <see cref="CustomActionBuilder{TEntity}"/>.</returns>
    public CustomActionBuilder<TEntity> SetDescription(string description)
    {
        customAction.Description = description;
        return this;
    }

    /// <summary>
    /// Sets the handler for the custom action.
    /// </summary>
    /// <param name="handler">The handler action that takes an <see cref="IServiceProvider"/> and an <see cref="IQueryable{TEntity}"/>.</param>
    /// <returns>The current instance of <see cref="CustomActionBuilder{TEntity}"/>.</returns>
    public CustomActionBuilder<TEntity> SetHandler(Action<IServiceProvider?, IQueryable<TEntity>> handler)
    {
        customAction.Handler = handler;
        return this;
    }

    /// <summary>
    /// Builds the custom action with the configured settings.
    /// </summary>
    /// <returns>The configured <see cref="CustomAction{TEntity}"/> instance.</returns>
    public CustomAction<TEntity> Build()
    {
        return customAction;
    }
}
