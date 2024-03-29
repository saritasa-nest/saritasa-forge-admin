﻿using AutoMapper;
using Saritasa.NetForge.Domain.Exceptions;
using CommunityToolkit.Mvvm.Messaging;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Interfaces;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels.EditEntity;

/// <summary>
/// View model for edit entity page.
/// </summary>
public class EditEntityViewModel : BaseViewModel
{
    /// <summary>
    /// Entity details model.
    /// </summary>
    public EditEntityModel Model { get; set; }

    /// <summary>
    /// Instance primary key.
    /// </summary>
    public string InstancePrimaryKey { get; set; }

    private readonly IEntityService entityService;
    private readonly IMapper mapper;
    private readonly IOrmDataService dataService;
    private readonly IFileService fileService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public EditEntityViewModel(
        string stringId,
        string instancePrimaryKey,
        IEntityService entityService,
        IMapper mapper,
        IOrmDataService dataService,
        IFileService fileService)
    {
        Model = new EditEntityModel { StringId = stringId };
        InstancePrimaryKey = instancePrimaryKey;

        this.entityService = entityService;
        this.mapper = mapper;
        this.dataService = dataService;
        this.fileService = fileService;
    }

    /// <summary>
    /// Whether the entity exists.
    /// </summary>
    public bool IsEntityExists { get; private set; } = true;

    /// <summary>
    /// Is entity was updated.
    /// </summary>
    public bool IsUpdated { get; set; }

    /// <inheritdoc/>
    public override async Task LoadAsync(CancellationToken cancellationToken)
    {
        try
        {
            var entity = await entityService.GetEntityByIdAsync(Model.StringId, cancellationToken);
            Model = mapper.Map<EditEntityModel>(entity);
            Model = Model with
            {
                Properties = Model.Properties
                    .Where(property => property is
                    {
                        IsCalculatedProperty: false,
                        IsValueGeneratedOnAdd: false,
                        IsValueGeneratedOnUpdate: false,
                        IsHiddenFromListView: false,
                    })
                    .ToList()
            };

            var includedNavigationNames = Model.Properties
                .Where(property => property is NavigationMetadataDto)
                .Select(property => property.Name);

            Model.EntityInstance = await dataService
                .GetInstanceAsync(InstancePrimaryKey, Model.ClrType!, includedNavigationNames, CancellationToken);

            Model.OriginalEntityInstance = Model.EntityInstance.CloneJson();
        }
        catch (NotFoundException)
        {
            IsEntityExists = false;
        }
    }

    /// <summary>
    /// Updates entity.
    /// </summary>
    public async Task UpdateEntityAsync()
    {
        var message = WeakReferenceMessenger.Default.Send(new UploadImageMessage());

        foreach (var image in message.ChangedFiles)
        {
            await fileService.CreateFileAsync(image.PathToFile!, image.FileContent!, CancellationToken);
        }

        await dataService.UpdateAsync(Model.EntityInstance!, Model.OriginalEntityInstance!, CancellationToken);
        IsUpdated = true;
    }
}
