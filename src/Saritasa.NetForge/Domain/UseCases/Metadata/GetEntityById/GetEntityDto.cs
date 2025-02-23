using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;

/// <summary>
/// DTO for <see cref="EntityMetadata"/>.
/// </summary>
public record GetEntityDto
{
    /// <inheritdoc cref="EntityMetadata.Id"/>
    public Guid Id { get; set; }

    /// <inheritdoc cref="EntityMetadata.DisplayName"/>
    public string DisplayName { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.PluralName"/>
    public string PluralName { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.StringId"/>
    public string StringId { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.Description"/>
    public string Description { get; set; } = string.Empty;

    /// <inheritdoc cref="EntityMetadata.ClrType"/>
    public Type? ClrType { get; set; }

    /// <inheritdoc cref="EntityMetadata.Properties"/>
    public ICollection<PropertyMetadataDto> Properties { get; set; } = new List<PropertyMetadataDto>();

    /// <inheritdoc cref="EntityMetadata.SearchFunction"/>
    public Func<IServiceProvider?, IQueryable<object>, string, IQueryable<object>>? SearchFunction { get; set; }

    /// <inheritdoc cref="EntityMetadata.CustomQueryFunction"/>
    public Func<IServiceProvider?, IQueryable<object>, IQueryable<object>>? CustomQueryFunction { get; set; }

    /// <inheritdoc cref="EntityMetadata.IsKeyless"/>
    public bool IsKeyless { get; set; }

    /// <summary>
    /// Action that called after entity update.
    /// </summary>
    public Action<IServiceProvider?, object, object>? AfterUpdateAction { get; set; }

    /// <inheritdoc cref="EntityMetadata.CanAdd"/>
    public bool CanAdd { get; set; }

    /// <inheritdoc cref="EntityMetadata.CanEdit"/>
    public bool CanEdit { get; set; }

    /// <inheritdoc cref="EntityMetadata.CanDelete"/>
    public bool CanDelete { get; set; }

    /// <inheritdoc cref="EntityMetadata.MessageOptions"/>
    public MessageOptions MessageOptions { get; set; } = new();

    /// <inheritdoc cref="EntityMetadata.ToStringFunc"/>
    public Func<object, string>? ToStringFunc { get; set; }

    /// <inheritdoc cref="EntityMetadata.CreateAction"/>
    public Action<IServiceProvider?, object>? CreateAction { get; set; }

    /// <inheritdoc cref="EntityMetadata.UpdateAction"/>
    public Action<IServiceProvider?, object>? UpdateAction { get; set; }
}
