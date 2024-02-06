using AutoMapper;
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
        CreateMap<PropertyMetadata, PropertyMetadataDto>();
        CreateMap<NavigationMetadata, NavigationMetadataDto>()
            .ForMember(dto => dto.IsPrimaryKey, options => options.Ignore())
            .ForMember(dto => dto.IsCalculatedProperty, options => options.Ignore())
            .ForMember(dto => dto.IsValueGeneratedOnAdd, options => options.Ignore())
            .ForMember(dto => dto.IsValueGeneratedOnUpdate, options => options.Ignore());
    }
}
