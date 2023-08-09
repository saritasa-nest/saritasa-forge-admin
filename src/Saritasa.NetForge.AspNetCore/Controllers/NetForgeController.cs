using Microsoft.AspNetCore.Mvc;
using Saritasa.NetForge.AspNetCore.Models;
using Saritasa.NetForge.Domain.Entities;

namespace Saritasa.NetForge.AspNetCore.Controllers;

/// <summary>
/// Gets DB metadata.
/// </summary>
public class NetForgeController : Controller
{
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    public NetForgeController(AdminOptions adminOptions)
    {
        this.adminOptions = adminOptions;
    }

    /// <summary>
    /// Gets the list of entities.
    /// </summary>
    public IActionResult Index()
    {
        var viewModel = new NetForgeModel
        {
            BaseHref = $"{adminOptions.AdminPanelEndpoint}/",
        };
        return View(viewModel);
    }
}
