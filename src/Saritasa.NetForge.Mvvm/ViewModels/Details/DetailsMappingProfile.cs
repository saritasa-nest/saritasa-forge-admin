using AutoMapper;
using Saritasa.NetForge.UseCases.Metadata.GetEntityById;

namespace Saritasa.NetForge.Mvvm.ViewModels.Details;

/// <summary>
/// Mapping profile for details page.
/// </summary>
internal class DetailsMappingProfile : Profile
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public DetailsMappingProfile()
    {
        CreateMap<GetEntityByIdDto, DetailsModel>();
    }
}
