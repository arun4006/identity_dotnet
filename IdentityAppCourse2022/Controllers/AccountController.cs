using IdentityAppCourse2022.interfaces;
using IdentityAppCourse2022.Models;
using IdentityAppCourse2022.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAppCourse2022.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ISendGrid_Email _sendGridEmail;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager
            ,ISendGrid_Email sendGridEmail)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _sendGridEmail = sendGridEmail;
        }
        public IActionResult Register(string? returnUrl=null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);

        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code=null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    ModelState.AddModelError("Email", "User Not Found");
                    return View();
                }
                var user_Token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, user_Token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

            }
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user == null)
                {
                    return RedirectToAction("ForgetPasswordConfirmation");
                }
               var code= await _userManager.GeneratePasswordResetTokenAsync(user);
               var callbackUrl = Url.Action("ResetPassword", "Account", 
                   new { userId=user.Id, code=code },protocol:HttpContext.Request.Scheme );
               await _sendGridEmail.SendEmailAsync(model.Email,"Reset Email Confirmation",
                 "please reset email by going to this " +
                 "<a href=\""+ callbackUrl +"\">Link</a>");

                return RedirectToAction("ForgetPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel,string? returnUrl)
        {
            registerViewModel.ReturnUrl = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
             var user=new AppUser { Email = registerViewModel.Email,UserName=registerViewModel.UserName};
             var result= await _userManager.CreateAsync(user,registerViewModel.Password);
             if (result.Succeeded)
             {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
             }
                ModelState.AddModelError("Password", "password must be uppercase and digits ");
            }
            return View(registerViewModel);
          }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Invalid login attempt");
                    return View(loginViewModel);
                }
            }
            return View(loginViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
    }
}
