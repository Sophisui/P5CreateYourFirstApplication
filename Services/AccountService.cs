using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P5CreateYourFirstApplication.Data;
using P5CreateYourFirstApplication.Models;
using P5CreateYourFirstApplication.Services.Interfaces;

namespace P5CreateYourFirstApplication.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountService(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            return result.Succeeded;
        }
    }
}
