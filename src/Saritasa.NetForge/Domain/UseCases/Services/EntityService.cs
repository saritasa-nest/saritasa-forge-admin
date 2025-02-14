using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Saritasa.NetForge.Domain.Dtos;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Enums;
using Saritasa.NetForge.Domain.Exceptions;
using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Domain.UseCases.Common;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.DTOs;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Domain.UseCases.Metadata.Services;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Common.Utils;

namespace Saritasa.NetForge.Domain.UseCases.Services;

/// <inheritdoc />
public class EntityService : IEntityService
{
    private readonly AdminMetadataService adminMetadataService;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Constructor for <see cref="EntityService"/>.
    /// </summary>
    public EntityService(AdminMetadataService adminMetadataService, IServiceProvider serviceProvider)
    {
        this.adminMetadataService = adminMetadataService;
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public Task<IEnumerable<EntityMetadataDto>> SearchEntitiesAsync(CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService.GetMetadata()
            .Where(entityMetadata => !entityMetadata.IsHidden);
        return Task.FromResult(MapEntityMetadata(metadata));
    }

    private static IEnumerable<EntityMetadataDto> MapEntityMetadata(IEnumerable<EntityMetadata> metadata)
    {
        return metadata.Select(entityMetadata => new EntityMetadataDto
        {
            DisplayName = entityMetadata.DisplayName,
            PluralName = entityMetadata.PluralName,
            Description = entityMetadata.Description,
            IsEditable = entityMetadata.IsEditable,
            Id = entityMetadata.Id,
            Group = entityMetadata.Group,
            StringId = entityMetadata.StringId
        });
    }

    /// <inheritdoc />
    public Task<GetEntityDto> GetEntityByIdAsync(string stringId, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .FirstOrDefault(entityMetadata => entityMetadata.StringId.Equals(stringId, StringComparison.OrdinalIgnoreCase));

        if (metadata is null)
        {
            throw new NotFoundException("Metadata for entity was not found.");
        }

        var metadataDto = GetEntityMetadataDto(metadata);

        return Task.FromResult(metadataDto);
    }

    /// <inheritdoc />
    public Task<GetEntityDto> GetEntityByTypeAsync(Type entityType, CancellationToken cancellationToken)
    {
        var metadata = adminMetadataService
            .GetMetadata()
            .FirstOrDefault(entityMetadata => entityMetadata.ClrType == entityType);

        if (metadata is null)
        {
            throw new NotFoundException("Metadata for entity was not found.");
        }

        var metadataDto = GetEntityMetadataDto(metadata);

        return Task.FromResult(metadataDto);
    }

    private static GetEntityDto GetEntityMetadataDto(EntityMetadata metadata)
    {
        var displayableProperties = metadata.Properties
            .Where(property => property is { IsForeignKey: false });

        var propertyDtos = displayableProperties.Select(MapProperty);

        var navigationDtos = metadata.Navigations.Select(MapNavigation);

        propertyDtos = propertyDtos.Union(navigationDtos);

        return MapGetEntityDto(metadata) with { Properties = propertyDtos.ToList() };
    }

    private static PropertyMetadataDto MapProperty(PropertyMetadata property)
    {
        return new PropertyMetadataDto
        {
            Name = property.Name,
            DisplayName = property.DisplayName,
            Description = property.Description,
            IsPrimaryKey = property.IsPrimaryKey,
            IsForeignKey = property.IsForeignKey,
            ClrType = property.ClrType,
            SearchType = property.SearchType,
            Order = property.Order,
            DisplayFormat = property.DisplayFormat,
            FormatProvider = property.FormatProvider,
            IsCalculatedProperty = property.IsCalculatedProperty,
            IsSortable = property.IsSortable,
            EmptyValueDisplay = property.EmptyValueDisplay,
            IsHidden = property.IsHidden,
            IsHiddenFromListView = property.IsHiddenFromListView,
            IsHiddenFromCreate = property.IsHiddenFromCreate,
            IsHiddenFromDetails = property.IsHiddenFromDetails,
            IsExcludedFromQuery = property.IsExcludedFromQuery,
            DisplayAsHtml = property.DisplayAsHtml,
            IsValueGeneratedOnAdd = property.IsValueGeneratedOnAdd,
            IsValueGeneratedOnUpdate = property.IsValueGeneratedOnUpdate,
            IsRichTextField = property.IsRichTextField,
            IsImage = property.IsImage,
            UploadFileStrategy = property.UploadFileStrategy,
            IsReadOnly = property.IsReadOnly,
            TruncationMaxCharacters = property.TruncationMaxCharacters,
            IsNullable = property.IsNullable,
            IsMultiline = property.IsMultiline,
            Lines = property.Lines,
            MaxLines = property.MaxLines,
            IsAutoGrow = property.IsAutoGrow,
            CanDisplayDetails = property.CanDisplayDetails,
            CanBeNavigatedToDetails = property.CanBeNavigatedToDetails,
        };
    }

    private static NavigationMetadataDto MapNavigation(NavigationMetadata navigation)
    {
        return new NavigationMetadataDto
        {
            IsCollection = navigation.IsCollection,
            TargetEntityProperties = navigation.TargetEntityProperties.ConvertAll(MapProperty),
            TargetEntityNavigations = navigation.TargetEntityNavigations.ConvertAll(MapNavigation),
            Name = navigation.Name,
            DisplayName = navigation.DisplayName,
            Description = navigation.Description,
            ClrType = navigation.ClrType,
            SearchType = navigation.SearchType,
            Order = navigation.Order,
            DisplayFormat = navigation.DisplayFormat,
            FormatProvider = navigation.FormatProvider,
            IsSortable = navigation.IsSortable,
            EmptyValueDisplay = navigation.EmptyValueDisplay,
            IsHidden = navigation.IsHidden,
            IsHiddenFromListView = navigation.IsHiddenFromListView,
            IsHiddenFromCreate = navigation.IsHiddenFromCreate,
            IsHiddenFromDetails = navigation.IsHiddenFromDetails,
            IsExcludedFromQuery = navigation.IsExcludedFromQuery,
            DisplayAsHtml = navigation.DisplayAsHtml,
            IsRichTextField = navigation.IsRichTextField,
            IsReadOnly = navigation.IsReadOnly,
            TruncationMaxCharacters = navigation.TruncationMaxCharacters,
            IsNullable = navigation.IsNullable,
            IsMultiline = navigation.IsMultiline,
            Lines = navigation.Lines,
            MaxLines = navigation.MaxLines,
            IsAutoGrow = navigation.IsAutoGrow
        };
    }

    private static GetEntityDto MapGetEntityDto(EntityMetadata entity)
    {
        return new GetEntityDto
        {
            Id = entity.Id,
            DisplayName = entity.DisplayName,
            PluralName = entity.PluralName,
            StringId = entity.StringId,
            Description = entity.Description,
            ClrType = entity.ClrType,
            SearchFunction = entity.SearchFunction,
            CustomQueryFunction = entity.CustomQueryFunction,
            IsKeyless = entity.IsKeyless,
            AfterUpdateAction = entity.AfterUpdateAction,
            CanAdd = entity.CanAdd,
            CanEdit = entity.CanEdit,
            CanDelete = entity.CanDelete,
            MessageOptions = entity.MessageOptions,
            ToStringFunc = entity.ToStringFunc,
        };
    }

    /// <inheritdoc />
    public bool ValidateEntity(object instance, ICollection<PropertyMetadataDto> properties, ref List<ValidationResult> errors)
    {
        var context = new ValidationContext(instance, serviceProvider, items: null);

        Validator.TryValidateObject(instance, context, errors, validateAllProperties: true);

        var isNotNullableProperties = properties
            .Where(property => property is { IsNullable: false, IsReadOnly: false })
            .Select(e => e.Name)
            .ToList();

        // Validate property that not have RequiredAttribute but have RequiredMemberAttribute or is not nullable (in ORM).
        var requiredProperties = instance.GetType().GetProperties()
            .Where(prop => !prop.IsDefined(typeof(RequiredAttribute), false) &&
                           (prop.CustomAttributes.Any(attr => attr.AttributeType.Name == "RequiredMemberAttribute") ||
                            isNotNullableProperties.Contains(prop.Name)))
            .ToList();

        var requiredErrors = new List<ValidationResult>();
        foreach (var property in requiredProperties)
        {
            var value = instance.GetPropertyValue(property.Name);
            var isError = false;

            switch (value)
            {
                case string str:
                    if (string.IsNullOrEmpty(str))
                    {
                        isError = true;
                    }

                    break;
                case DateTime dt:
                    if (dt == DateTime.MinValue)
                    {
                        isError = true;
                    }

                    break;
                case DateTimeOffset dt:
                    if (dt == DateTimeOffset.MinValue)
                    {
                        isError = true;
                    }

                    break;
                case DateOnly dt:
                    if (dt == DateOnly.MinValue)
                    {
                        isError = true;
                    }

                    break;
                default:
                    if (value is null)
                    {
                        isError = true;
                    }

                    break;
            }

            if (isError)
            {
                requiredErrors.Add(new ValidationResult($"The {property.Name} field is required.", [property.Name]));
            }
        }

        if (requiredErrors.Count != 0)
        {
            errors.AddRange(requiredErrors);
        }

        return errors.Count == 0;
    }
}
