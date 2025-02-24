using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProjectLapShop.Models;
using ProjectLapShop.Utilities;

namespace ProjectLapShop.Controllers
{
    public class UsersController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        IEmailSender _emailSender;

        public UsersController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IEmailSender emailSender )
        {
            _userManager=userManager;
            _signInManager=signInManager;
            _emailSender=emailSender;
        }
        public IActionResult Login(string ReturnUrl)
        {
            UserModel model = new UserModel()
            {
                ReturnUrl = ReturnUrl
            };
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        public IActionResult Register()
        {
            return View(new UserModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);
            ApplicationUser user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email=model.Email,
                UserName = model.Email,
            };
            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
                    var myUser=await _userManager.FindByEmailAsync(user.Email);
                    await _userManager.AddToRoleAsync(myUser, "Customer");

                    if (loginResult.Succeeded)
                        return Redirect("~/Users/Login");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }
                
            return View(new UserModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model,string ReturnUrl)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };
            try
            {
                var loginResult = await _signInManager.PasswordSignInAsync(user.Email, model.Password, true, true);
                if (loginResult.Succeeded)
                {
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("~/");
                    else
                        return Redirect(model.ReturnUrl);
                }
            }
            catch (Exception ex)
            {

            }
            return View(new UserModel());
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }
        // Step 2: Handle email submission and send code
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View();
            }

            // Generate 6-digit code
            var confirmationCode = new Random().Next(100000, 999999).ToString();

            // Store the code and expiration time in TempData (or database for production)
            TempData["Code"] = confirmationCode;
            TempData["Email"] = email;
            TempData["CodeExpiry"] = DateTime.UtcNow.AddMinutes(3);

            // Send email
            await _emailSender.SendEmailAsync(email, "Password Reset Code",
                $"Your password reset code is: {confirmationCode}");

            return RedirectToAction("ConfirmCode");
        }

        // Step 3: Show code input form
        public IActionResult ConfirmCode()
        {
            return View();
        }
        // Step 4: Handle code verification
        [HttpPost]
        public IActionResult ConfirmCode(string code)
        {
            var storedCode = TempData.Peek("Code")?.ToString(); // Use Peek to retain TempData for subsequent requests
            var email = TempData.Peek("Email")?.ToString();
            var expiry = (DateTime?)TempData.Peek("CodeExpiry");

            if (storedCode == null || expiry == null || email == null || DateTime.UtcNow > expiry)
            {
                ModelState.AddModelError("", "Code has expired or is invalid.");
                return View();
            }

            if (code != storedCode)
            {
                ModelState.AddModelError("", "Invalid code.");
                return View();
            }

            // Store email for password reset
            TempData["Email"] = email;
            return RedirectToAction("ResetPassword");
        }


        public IActionResult ResetPassword()
        {
            return View();
        }
        // Step 6: Handle password update
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string newPassword)
        {
            var email = TempData["Email"]?.ToString();
            if (email == null)
            {
                return BadRequest("Invalid request.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordSuccess");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }


        public IActionResult ResetPasswordSuccess()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
//admin
//m@gmail.com
//ahmedH@123->pass
//customer
//mans.gmail.com
//ahmed@H123->pass
