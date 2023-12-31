﻿using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Metadata.DTOs;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.UseCases.Metadata;

/// <summary>
/// Metadata mapping profile.
/// </summary>
public class MetadataMappingProfile : Profile
{
    /// <summary>
    /// Create maps.
    /// </summary>
    public MetadataMappingProfile()
    {
        CreateMap<EntityMetadata, EntityMetadataDto>();
        CreateMap<EntityMetadata, GetEntityByIdDto>();
        CreateMap<PropertyMetadata, PropertyMetadataDto>()
            .ForMember(dto => dto.IsNavigation, options => options.Ignore())
            .ForMember(dto => dto.IsNavigationCollection, options => options.Ignore())
            .ForMember(dto => dto.TargetEntityProperties, options => options.Ignore());
        CreateMap<NavigationMetadata, PropertyMetadataDto>()
            .ForMember(dto => dto.IsNavigation, options => options.MapFrom(entity => true))
            .ForMember(dto => dto.IsNavigationCollection, options => options.MapFrom(entity => entity.IsCollection))
            .ForMember(dto => dto.IsPrimaryKey, options => options.Ignore())
            .ForMember(dto => dto.IsCalculatedProperty, options => options.Ignore());
    }
}
