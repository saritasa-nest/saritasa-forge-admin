using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Demo.Net7.ViewModels;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Demo.Net7.Controllers;

/// <summary>
/// Home controller.
/// </summary>
public class HomeController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly AdminOptions adminOptions;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="signInManager">Signin manager.</param>
    /// <param name="adminOptions">Admin panel option.</param>
    public HomeController(SignInManager<User> signInManager, AdminOptions adminOptions)
    {
        this.signInManager = signInManager;
        this.adminOptions = adminOptions;
    }

    /// <summary>
    /// Display the home page.
    /// </summary>
    public ActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Handler of the login form for user authentication.
    /// </summary>
    /// <param name="viewModel">View model contain the login information.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Index(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

        if (result.Succeeded)
        {
            return Redirect(adminOptions.AdminPanelEndpoint);
        }

        if (result.IsNotAllowed)
        {
            ModelState.AddModelError(nameof(viewModel.Username), $"User {viewModel.Username} is not allow to sign-in.");
            return View();
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(nameof(viewModel.Username), $"User {viewModel.Username} is locked out.");
            return View();
        }

        ModelState.AddModelError(nameof(viewModel.Username), "Username or password is incorrect");
        return View();
    }
}