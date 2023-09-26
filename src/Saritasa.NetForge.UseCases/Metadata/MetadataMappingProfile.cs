﻿using AutoMapper;
using Saritasa.NetForge.Domain.Entities.Metadata;
using Saritasa.NetForge.UseCases.Metadata.DTOs;

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
    }
}
