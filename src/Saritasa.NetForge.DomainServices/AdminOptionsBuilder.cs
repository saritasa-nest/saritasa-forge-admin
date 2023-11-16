using System.Text.RegularExpressions;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.DomainServices.Interfaces;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.DomainServices;

/// <summary>
/// Builds the admin options.
/// </summary>
public class AdminOptionsBuilder
{
    private readonly AdminOptions options = new();

    /// <summary>
    /// Provider for injecting ORM-specific services.
    /// </summary>
    public IAdminOrmServiceProvider? AdminOrmServiceProvider { get; set; }

    /// <summary>
    /// Set the URL on which the admin panel will be hosted.
    /// </summary>
    /// <param name="url">URL.</param>
    public AdminOptionsBuilder UseEndpoint(string url)
    {
        options.AdminPanelEndpoint = url;
        return this;
    }

    /// <summary>
    /// Set the header title for the admin panel web page.
    /// </summary>
    /// <param name="title">Header title.</param>
    public AdminOptionsBuilder SetHeaderTitle(string title)
    {
        options.AdminPanelHeaderTitle = title;
        return this;
    }

    /// <summary>
    /// Set the html title for the admin panel web page.
    /// </summary>
    /// <param name="title">Html title.</param>
    public AdminOptionsBuilder SetHtmlTitle(string title)
    {
        options.AdminPanelHtmlTitle = title;
        return this;
    }

    /// <summary>
    /// Get options for the admin panel.
    /// </summary>
    public AdminOptions Create()
    {
        return options;
    }

    /// <summary>
    /// Configures options for a specific entity type within the admin panel.
    /// </summary>
    /// <param name="entityOptionsBuilderAction">An action that builds entity options.</param>
    /// <typeparam name="TEntity">The type of entity for which options are being configured.</typeparam>
    public AdminOptionsBuilder ConfigureEntity<TEntity>(
        Action<EntityOptionsBuilder<TEntity>> entityOptionsBuilderAction) where TEntity : class
    {
        var entityOptionsBuilder = new EntityOptionsBuilder<TEntity>();
        entityOptionsBuilderAction.Invoke(entityOptionsBuilder);
        options.EntityOptionsList.Add(entityOptionsBuilder.Create());
        return this;
    }

    /// <summary>
    /// Configures options for a specific entity type within the admin panel using the configuration.
    /// </summary>
    /// <param name="entityConfiguration">The instance of the entity configuration class.</param>
    /// <typeparam name="TEntity">The type of entity for which options are being configured.</typeparam>
    public AdminOptionsBuilder ConfigureEntity<TEntity>(IEntityAdminConfiguration<TEntity> entityConfiguration)
        where TEntity : class
    {
        var entityOptionsBuilder = new EntityOptionsBuilder<TEntity>();
        entityConfiguration.Configure(entityOptionsBuilder);
        options.EntityOptionsList.Add(entityOptionsBuilder.Create());
        return this;
    }

    /// <summary>
    /// Adds new groups to the configuration. Duplicates will be automatically filtered out based on group names.
    /// </summary>
    /// <param name="groups">The groups to be added.</param>
    public AdminOptionsBuilder AddGroups(IEnumerable<EntityGroup> groups)
    {
        foreach (var group in groups)
        {
            // Check for duplicated group's name.
            if (options.EntityGroupsList.Any(entityGroup => entityGroup.Name == group.Name))
            {
                continue;
            }
            options.EntityGroupsList.Add(group);
        }
        return this;
    }

    /// <summary>
    /// Configure the url to an external site.
    /// </summary>
    /// <param name="url">The url to be configured.</param>
    public AdminOptionsBuilder SetSiteUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return this;
        }

        if (IsUrlValid(url))
        {
            options.SiteUrl = url;
        }
        return this;
    }

    private static bool IsUrlValid(string url)
    {
        const string pattern =
            @"^(https?:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}(\/[a-zA-Z0-9\-\/]*)?)|(\/[a-zA-Z0-9\-\/]+)$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(url);
    }
}
