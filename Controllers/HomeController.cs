using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P5CreateYourFirstApplication.Data;
using P5CreateYourFirstApplication.Models;
using P5CreateYourFirstApplication.Services.Interfaces;
using System.Diagnostics;

namespace P5CreateYourFirstApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVoitureService _voitureService;
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public HomeController(
            IVoitureService voitureService,
            ApplicationDbContext context,
            IAccountService accountService,
            ILogger<HomeController> logger)
        {
            _voitureService = voitureService;
            _context = context;
            _accountService = accountService;
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _accountService.LoginAsync(model))
                {
                    _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                    return LocalRedirect(model.ReturnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var voitures = await _voitureService.GetAllAsync();
            return View(voitures);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
