using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Domain.Entities.Options;

namespace Saritasa.NetForge.Demo.Net7.Controllers;

public class HomeController : Controller
{
    private readonly SignInManager<User> signInManager;
    private readonly AdminOptions adminOptions;

    public HomeController(SignInManager<User> signInManager, AdminOptions adminOptions)
    {
        this.signInManager = signInManager;
        this.adminOptions = adminOptions;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromForm] LoginCommand command)
    {
        var result = await signInManager.PasswordSignInAsync(command.Username, command.Password, false, false);
        if (result.Succeeded)
        {
            return Redirect(adminOptions.AdminPanelEndpoint);
        }

        return View();
    }
}

public class LoginCommand
{
    public string Username { get; set; }

    public string Password { get; set; }
}