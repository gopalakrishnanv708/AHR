using AHR.Models;
using AHR.DBContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AHR.Controllers
{
    public class AccountController : Controller
    {
        private AccountDBContext accountDbContext;

        public AccountController()
        {
            accountDbContext = new AccountDBContext();
        }

        [Route("/Account/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/Account/Login")]
        [HttpPost("Login")]
        public async Task<IActionResult> ValidateLogin(Login login)
        {
            if (ModelState.IsValid)
            {
                var claims = new List<Claim>();

                if (accountDbContext.ValidateCredentials(login) && accountDbContext.Role.Equals("A"))
                {
                    
                    claims.Add(new Claim("email", login.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, login.Email));
                    claims.Add(new Claim(ClaimTypes.Role, accountDbContext.Role));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    bool t = HttpContext.User.Identity.IsAuthenticated;
                    return RedirectToAction("Index", "Home");
                }
                else if (accountDbContext.ValidateCredentials(login) && accountDbContext.Role.Equals("D"))
                {

                    claims.Add(new Claim("email", login.Email));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, login.Email));
                    claims.Add(new Claim(ClaimTypes.Role, accountDbContext.Role));
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    bool t = HttpContext.User.Identity.IsAuthenticated;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Login");
                }
                
            }
            else
            {
                TempData["Error"] = "Username or Pasword is incorrect";
                return View("Login");
            }

        }

        [Route("/Account/Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/Account/Register")]
        [HttpPost("Register")]
        public IActionResult ValidateRegister(Register register)
        {
            if (ModelState.IsValid)
            {
                if(accountDbContext.CreateUser(register))
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Error"] = "Email Id is already present. Try with diffrent Email Id. If Error persists, Please contact administrator.";
                    return View("Register");
                }
            }
            return View("Register");
        }

        [Authorize]
        public async  Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
