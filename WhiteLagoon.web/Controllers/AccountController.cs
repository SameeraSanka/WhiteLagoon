using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.web.ViewModels;

namespace WhiteLagoon.web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // this is fo accsess the dataa base
        private readonly UserManager<ApplicationUser> _userManager; // this is for manage users
        private readonly SignInManager<ApplicationUser> _signInManager; //responcible for sign in user and related operation
        private readonly RoleManager<IdentityRole> _roleManager; // managing user roles

        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        // login view
        public IActionResult LogIn(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var loginVM = new LoginVM()
            {
                RederecUrl = returnUrl,
            };
            return View(loginVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //register view
        public IActionResult Register(string returnUrl = null)
        {
			returnUrl ??= Url.Content("~/");
			//StaticDetails kiyna eka wenama hadapu clz ekek eke sttic content dala ona than wlata cl kranwa
			if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).Wait();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).Wait();
            }
            var registerVM = new RegisterVM()
            {
                RoleList = _roleManager.Roles.Select(Role =>new SelectListItem
                {
                    Text = Role.Name,
                    Value = Role.Name
                }),
                RedirectUrl = returnUrl,
            };

            return View(registerVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            var ApplicationUserObj = new ApplicationUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
                NormalizedEmail = registerVM.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = registerVM.Email,
                CreatedAt = DateTime.Now,
            };
            //mehema thama create krnne
            var result = await _userManager.CreateAsync(ApplicationUserObj, registerVM.Password);

            // add role to the user
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVM.Role))
                {
                    await _userManager.AddToRoleAsync(ApplicationUserObj, registerVM.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(ApplicationUserObj, StaticDetails.Role_Customer);
                }
                //meken krnne register unama signin krana eka
                await _signInManager.SignInAsync(ApplicationUserObj, isPersistent: false);

                if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(registerVM.RedirectUrl);
                }

            }
            else
            {
                //to give errors
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            registerVM = new RegisterVM()
            {
                RoleList = _roleManager.Roles.Select(Role => new SelectListItem
                {
                    Text = Role.Name,
                    Value = Role.Name
                })
            };
            return View(registerVM);

        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM loginVM)
        {
            if(ModelState.IsValid)
            {
                // PasswordSignInAsync meken wenne pw eka check krana ekak
                var result = await _signInManager
                    .PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure:false);
            
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.RederecUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVM.RederecUrl);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(loginVM);
        }

    }

}
