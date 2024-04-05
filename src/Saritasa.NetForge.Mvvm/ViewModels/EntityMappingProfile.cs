using AutoMapper;
using Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;
using Saritasa.NetForge.Mvvm.ViewModels.EditEntity;
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
        CreateMap<GetEntityByIdDto, CreateEntityModel>()
            .ForMember(model => model.EntityInstance, options => options.Ignore());
        CreateMap<GetEntityByIdDto, EditEntityModel>()
            .ForMember(model => model.EntityInstance, options => options.Ignore())
            .ForMember(model => model.OriginalEntityInstance, options => options.Ignore());
    }
}
