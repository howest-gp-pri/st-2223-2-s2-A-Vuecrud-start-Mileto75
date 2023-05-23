using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pri.Ca.Core.Entities;
using Pri.Ca.Web.Areas.Identity.ViewModels;
using System.Security.Claims;

namespace Pri.Ca.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterViewModel accountRegisterViewModel)
        {
            if(!ModelState.IsValid) 
            {
                return View();
            }
            //create user
            //check if username exists
            var user = await _userManager.FindByNameAsync(accountRegisterViewModel.Username);
            if(user != null)
            {
                ModelState.AddModelError("username", "Already exists!");
                return View(accountRegisterViewModel);
            }
            //create applicationUser
            user = new ApplicationUser();
            user.UserName = accountRegisterViewModel.Username;
            user.Firstname = accountRegisterViewModel.Firstname;
            user.Lastname = accountRegisterViewModel.Lastname;
            user.DateOfBirth = accountRegisterViewModel.DateOfBirth;
            //add to database use _usermanager.CreateAsync
            var result = await _userManager
                .CreateAsync(user,accountRegisterViewModel.Password);
            //add basic claims
            await _userManager.AddClaimAsync(user,new Claim(ClaimTypes.Role,"User"));
            await _userManager.AddClaimAsync(user,new Claim(ClaimTypes.DateOfBirth,user.DateOfBirth.ToString()));
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(accountRegisterViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(accountLoginViewModel);
            }
            //sign in user
            var result = await _signInManager
                .PasswordSignInAsync(accountLoginViewModel.Username,
                accountLoginViewModel.Password, false, false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials");
                return View(accountLoginViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
