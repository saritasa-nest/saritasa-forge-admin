using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.Domain.Exceptions;
using Saritasa.NetForge.Domain.Extensions;
using Saritasa.NetForge.Domain.UseCases.Interfaces;
using Saritasa.NetForge.Domain.UseCases.Metadata.DTOs;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.Domain.UseCases.Metadata.Services;
using Saritasa.NetForge.Extensions;

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
            StringId = entityMetadata.StringId,
            IsOwned = entityMetadata.IsOwned
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
        var propertyDtos = metadata.Properties
            .Where(property => property is { IsForeignKey: false })
            .Select(property => MapProperty(property));

        var navigationDtos = metadata.Navigations.Select(navigation => MapNavigation(navigation));

        propertyDtos = propertyDtos.Union(navigationDtos);

        return MapGetEntityDto(metadata) with { Properties = propertyDtos.ToList() };
    }

    private static PropertyMetadataDto MapProperty(
        PropertyMetadata property, NavigationMetadataDto? navigation = null)
    {
        return new PropertyMetadataDto
        {
            Name = property.Name,
            PropertyPath = property.PropertyPath,
            DisplayName = property.DisplayName,
            Description = property.Description,
            PropertyInformation = property.PropertyInformation,
            IsPrimaryKey = property.IsPrimaryKey,
            IsForeignKey = property.IsForeignKey,
            ClrType = property.ClrType,
            SearchType = property.SearchType,
            Order = property.Order,
            FormOrder = property.FormOrder,
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
            NavigationMetadata = navigation
        };
    }

    private static NavigationMetadataDto MapNavigation(
        NavigationMetadata navigation, NavigationMetadataDto? parentNavigation = null)
    {
        var navigationMetadata = new NavigationMetadataDto
        {
            IsCollection = navigation.IsCollection,
            Name = navigation.Name,
            PropertyPath = navigation.PropertyPath,
            DisplayName = navigation.DisplayName,
            Description = navigation.Description,
            ClrType = navigation.ClrType,
            SearchType = navigation.SearchType,
            Order = navigation.Order,
            FormOrder = navigation.FormOrder,
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
            IsAutoGrow = navigation.IsAutoGrow,
            IsOwnership = navigation.IsOwnership,
            NavigationMetadata = parentNavigation
        };

        navigationMetadata.TargetEntityProperties = navigation.TargetEntityProperties
            .ConvertAll(targetProperty => MapProperty(targetProperty, navigationMetadata));
        navigationMetadata.TargetEntityNavigations = navigation.TargetEntityNavigations
            .ConvertAll(targetNavigation => MapNavigation(targetNavigation, navigationMetadata));

        return navigationMetadata;
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
            CreateAction = entity.CreateAction,
            UpdateAction = entity.UpdateAction,
            DeleteAction = entity.DeleteAction,
            CustomActions = entity.CustomActions
        };
    }

    /// <inheritdoc />
    public bool ValidateEntity(object instance, ICollection<PropertyMetadataDto> properties, ref List<ValidationResult> errors)
    {
        var context = new ValidationContext(instance, serviceProvider, items: null);

        Validator.TryValidateObject(instance, context, errors, validateAllProperties: true);

        var notNullableProperties = properties
            .Where(property => property is { IsNullable: false, IsReadOnly: false })
            .Select(property => property.PropertyPath)
            .ToList();

        // Validate property that not have RequiredAttribute but have RequiredMemberAttribute or is not nullable (in ORM).
        var requiredProperties = properties
            .Where(property =>
                property.PropertyInformation is not null &&
                !property.PropertyInformation.IsDefined(typeof(RequiredAttribute), false) &&
                (property.PropertyInformation.CustomAttributes.Any(a => a.AttributeType.Name == nameof(RequiredMemberAttribute))
                    || notNullableProperties.Contains(property.PropertyPath)))
            .ToList();

        var requiredErrors = new List<ValidationResult>();
        foreach (var property in requiredProperties)
        {
            var value = instance.GetNestedPropertyValue(property.PropertyPath);
            var isError = false;

            switch (value)
            {
                case string str:
                    if (string.IsNullOrEmpty(str))
                    {
                        isError = true;
                    }

                    break;
                case DateTime dateTime:
                    if (dateTime == DateTime.MinValue)
                    {
                        isError = true;
                    }

                    break;
                case DateTimeOffset dateTimeOffset:
                    if (dateTimeOffset == DateTimeOffset.MinValue)
                    {
                        isError = true;
                    }

                    break;
                case DateOnly dateOnly:
                    if (dateOnly == DateOnly.MinValue)
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
                requiredErrors
                    .Add(new ValidationResult($"The {property.GetDisplayName()} field is required.", [property.PropertyPath]));
            }
        }

        if (requiredErrors.Count != 0)
        {
            errors.AddRange(requiredErrors);
        }

        return errors.Count == 0;
    }
}
