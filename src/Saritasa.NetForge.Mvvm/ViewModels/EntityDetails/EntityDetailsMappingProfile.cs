using AutoMapper;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels.EntityDetails;

/// <summary>
/// Mapping profile for details page.
/// </summary>
internal class EntityDetailsMappingProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public EntityDetailsMappingProfile()
    {
        CreateMap<GetEntityByIdDto, EntityDetailsModel>();
    }
}
