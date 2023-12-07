using AutoMapper;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Mapping profile for entities.
/// </summary>
internal class EntityMappingProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityMappingProfile()
    {
        CreateMap<GetEntityByIdDto, EntityDetailsModel>();
        CreateMap<GetEntityByIdDto, CreateEntityModel>();
    }
}
