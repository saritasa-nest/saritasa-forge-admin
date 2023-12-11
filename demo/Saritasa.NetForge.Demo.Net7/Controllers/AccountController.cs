using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saritasa.NetForge.Demo.Net7.Models;
using Saritasa.NetForge.Demo.Net7.ViewModels;

namespace Saritasa.NetForge.Demo.Net7.Controllers;

/// <summary>
/// User account controller.
/// </summary>
[Route("account")]
public class AccountController : Controller
{
    private readonly SignInManager<User> signInManager;
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="signInManager">Signin manager.</param>
    public AccountController(SignInManager<User> signInManager)
    {
        this.signInManager = signInManager;
    }
    
    /// <summary>
    /// Returns the login page.
    /// </summary>
    [HttpGet("login")]
    public IActionResult Login()
    {
        if (signInManager.IsSignedIn(User))
        {
            return Redirect("/");
        }
        
        return View();
    }

    /// <summary>
    /// Handler of the login form for user authentication.
    /// </summary>
    /// <param name="viewModel">View model contain the login information.</param>
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var result = await signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

        if (result.Succeeded)
        {
            return Redirect("/");
        }

        if (result.IsNotAllowed)
        {
            ModelState.AddModelError(nameof(viewModel.Username), $"User {viewModel.Username} is not allow to sign-in.");
        }

        else if (result.IsLockedOut)
        {
            ModelState.AddModelError(nameof(viewModel.Username), $"User {viewModel.Username} is locked out.");
        }
        else
        {
            ModelState.AddModelError(nameof(viewModel.Username), "Username or password is incorrect.");
        }

        return View(viewModel);
    }
    
    /// <summary>
    /// Logouts user from the website.
    /// </summary>
    [Authorize]
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}