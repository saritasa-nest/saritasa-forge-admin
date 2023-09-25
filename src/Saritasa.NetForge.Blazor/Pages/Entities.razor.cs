using Microsoft.AspNetCore.Components;
using Saritasa.NetForge.Mvvm.ViewModels.Metadata;

namespace Saritasa.NetForge.Blazor.Pages;

/// <summary>
/// List the entities.
/// </summary>
[Route("/entities")]
public partial class Entities : MvvmComponentBase<EntitiesViewModel>
{
}
