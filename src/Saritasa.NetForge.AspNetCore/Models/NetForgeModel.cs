namespace Saritasa.NetForge.AspNetCore.Models;

/// <summary>
/// Represents the index page model.
/// </summary>
public class NetForgeModel
{
    /// <summary>
    /// Base href to inject on the index page.
    /// </summary>
    public string BaseHref { get; set; } = string.Empty;

    //public NetForgeModel(AdminOptions adminOptions)
    //{
    //    this.adminOptions = adminOptions;
    //}

    //public void OnGet()
    //{
    //    BaseHref = $"{adminOptions.AdminPanelEndpoint}/";
    //}
}
