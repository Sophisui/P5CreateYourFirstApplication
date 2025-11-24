using Microsoft.AspNetCore.Identity;
using P5CreateYourFirstApplication.Models;

namespace P5CreateYourFirstApplication.Services.Interfaces
{
    public interface IAccountService
    { 
         Task<bool> LoginAsync(LoginViewModel model);
    }
}
