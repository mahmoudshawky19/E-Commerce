using E_Commerce.Models;
using E_Commerce.Utility;
using E_Commerce.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace E_Commerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        //UserManager<ApplicationUser> _userManager = new();

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Register()
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(Sw.adminRole));
                await roleManager.CreateAsync(new(Sw.CompanyRole));
                await roleManager.CreateAsync(new(Sw.CustomerRole));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(ApplicationUserVm userVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = userVM.name,
                    Email = userVM.email ,
                    Address = userVM.Address
                };

                var result = await userManager.CreateAsync(applicationUser, userVM.Password);

                if (result.Succeeded)
                {
                   await userManager.AddToRoleAsync(applicationUser,Sw.CustomerRole );

                  await  signInManager.SignInAsync(applicationUser, false);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("Password", "error - does not match the constrains");
            }

            return View(userVM);
        }
        public  IActionResult  Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if(ModelState.IsValid)
            {  
           var applicationUser = await userManager.FindByNameAsync(loginVm.UserName);
                if(applicationUser != null)
                {
             var c =     await   userManager.CheckPasswordAsync(applicationUser, loginVm.Password);
                    if(c)
                    {
                       await signInManager.SignInAsync(applicationUser, loginVm.RemeberMe);
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                       ModelState.AddModelError("Password", "Invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "Invalid user"); 
                 }
            }
            return View();
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
         }

    }
}
