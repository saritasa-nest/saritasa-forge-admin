namespace Saritasa.NetForge.Mvvm.ViewModels.CreateEntity;

/// <summary>
/// View model for create entity page.
/// </summary>
public class CreateEntityViewModel : BaseViewModel
{
    /// <summary>
    /// Entity details model.
    /// </summary>
    public CreateEntityModel Model { get; private set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public CreateEntityViewModel(string stringId)
    {
        Model = new CreateEntityModel { StringId = stringId };
    }
}
