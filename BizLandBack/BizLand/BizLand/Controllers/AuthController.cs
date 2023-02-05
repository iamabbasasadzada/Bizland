using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        //public IActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost]

        //public async Task<IActionResult> Register(RegisterVM registerVM)
        //{
        //    if (!ModelState.IsValid) return BadRequest();
        //    AppUser user = new AppUser
        //    {
        //        Name=registerVM.Name,
        //        Surname=registerVM.Surname,
        //        UserName=registerVM.Username,
        //        Email=registerVM.Email
        //    };
        //    IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
        //    if (!result.Succeeded)
        //    {
        //        foreach (var item in result.Errors)
        //        {
        //            ModelState.AddModelError("", item.Description);
        //        }
        //        return View();
        //    }
        //    await _signInManager.SignInAsync(user, true);
        //    return RedirectToAction("Index", "Home");
        //}
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM sign , string ReturnUrl)
        {
            AppUser user;
            if (sign.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(sign.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(sign.UsernameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Login or Password is incorrect");
                return View(sign);
            }
            var result = await _signInManager.PasswordSignInAsync(user, sign.Password, true, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login or Password is incorrect");
                return View(sign);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is lockouted for 5 minute");
                return View(sign);
            }
            if (ReturnUrl != null) return LocalRedirect(ReturnUrl);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
