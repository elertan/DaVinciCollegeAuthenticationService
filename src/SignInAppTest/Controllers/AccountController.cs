﻿using System.Linq;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Models.AccountViewModels;
using DaVinciCollegeAuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using DaVinciCollegeAuthenticationService.Data;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public AccountController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result =
                    await _signInManager.PasswordSignInAsync(model.UserNumber, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                    return RedirectToAction(nameof(SendCode), new {ReturnUrl = returnUrl, model.RememberMe});
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                ModelState.AddModelError(string.Empty, "Inloggen mislukt.");
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //// GET: /Account/Register
        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Register(string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}

        //// POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (!ModelState.IsValid) return View(model);

        //    var user = new ApplicationUser
        //    {
        //        UserName = model.UserNumber,
        //        Email = _emailProvider.GetEmailByUserNumber(model.UserNumber)
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
        //        // Send an email with this link
        //        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
        //        //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
        //        //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
        //        await _signInManager.SignInAsync(user, false);
        //        _logger.LogInformation(3, "User created a new account with password.");
        //        return RedirectToLocal(returnUrl);
        //    }
        //    AddErrors(result);

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        ////
        //// POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    // Request a redirect to the external login provider.
        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        ////
        //// GET: /Account/ExternalLoginCallback
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        //{
        //    if (remoteError != null)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        //        return View(nameof(Login));
        //    }
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //        return RedirectToAction(nameof(Login));

        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //    if (result.Succeeded)
        //    {
        //        _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    if (result.RequiresTwoFactor)
        //        return RedirectToAction(nameof(SendCode), new {ReturnUrl = returnUrl});
        //    if (result.IsLockedOut)
        //        return View("Lockout");
        //    // If the user does not have an account, then ask the user to create an account.
        //    ViewData["ReturnUrl"] = returnUrl;
        //    ViewData["LoginProvider"] = info.LoginProvider;
        //    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel {Email = email});
        //}

        ////
        //// POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model,
        //    string returnUrl = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //            return View("ExternalLoginFailure");
        //        var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
        //        var result = await _userManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await _userManager.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, false);
        //                _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View(model);
        //}

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if ((userId == null) || (code == null))
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var applicationUser = _context.ApplicationUser.FirstOrDefault(a => a.UserName.ToLower() == model.UserName.ToLower());

            if (applicationUser != null)
            {
                var vertificationCode = Guid.NewGuid();

                var fullName = applicationUser.Firstname +  " ";
                if (applicationUser.Prefix != string.Empty && applicationUser.Prefix != " ")
                {
                    fullName += applicationUser.Prefix + " ";
                }
                fullName += applicationUser.Lastname;

                var message = $"Beste " + fullName + ",\n\nDruk op deze link om jouw account's wachtwoord te veranderen. http://localhost:2922/Account/ForgetPasswordVertification/" + vertificationCode + "\n\nAls jij deze aanvraag niet hebt gedaan, kan je deze mail negeren.\n\n Je hebt 30 minuten de tijd om het wachtwoord aan te vragen/resetten\n\nMet vriendelijke groet,\n\nHet DaVinci Authservice Team";
                await _emailSender.SendEmailAsync(applicationUser.Email, "Account Password Reset", message);
                _context.PasswordResets.Add(new PasswordReset { UserName = model.UserName, VertificationCode = vertificationCode, ValidTill = DateTime.Now.AddMinutes(30) });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return RedirectToAction(nameof(ForgotPasswordError));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Account/ForgetPasswordVertification/{vertificationCode}")]
        public IActionResult ForgetPasswordVertification(string vertificationCode)
        {
            Guid vertCode;
            if (Guid.TryParse(vertificationCode, out vertCode))
            {
                var passwordReset = _context.PasswordResets.FirstOrDefault(p => p.VertificationCode == vertCode);
                if (passwordReset != null && passwordReset.ValidTill > DateTime.Now)
                {
                    return View(new ForgetPasswordVerificationModel { PasswordReset = passwordReset });
                }
            }

            //The vertificationCode doesnt exist or has expired
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPasswordVertification(ForgetPasswordVerificationModel forgetPasswordModel, string vertificationCode, string userName)
        {
            if (!ModelState.IsValid)
            {
                forgetPasswordModel.PasswordReset = _context.PasswordResets.FirstOrDefault(p => p.VertificationCode == Guid.Parse(vertificationCode));
                return View(forgetPasswordModel);
            }

            if (ModelState.IsValid)
            {
                Guid vertCode;
                if (Guid.TryParse(vertificationCode, out vertCode))
                {
                    var passwordReset = _context.PasswordResets.FirstOrDefault(p => p.VertificationCode == vertCode);
                    if (passwordReset != null)
                    {
                        var userToChange = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());

                        if (userToChange != null)
                        {
                            var code = await _userManager.GeneratePasswordResetTokenAsync(userToChange);
                            var resetResult = await _userManager.ResetPasswordAsync(userToChange, code, forgetPasswordModel.NewPassword);
                            if (!resetResult.Succeeded)
                            {
                                foreach (var error in resetResult.Errors)
                                {
                                    ModelState.AddModelError(error.Code, error.Description);
                                }
                                forgetPasswordModel.PasswordReset = _context.PasswordResets.FirstOrDefault(p => p.VertificationCode == Guid.Parse(vertificationCode));
                                return View(forgetPasswordModel);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
            //The vertificationCode doesnt exist

        }

        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordError()
        {
            ViewBag.Error = "Gebruiker bestaat niet";
            return View();
        }

        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return View("Error");
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions =
                userFactors.Select(purpose => new SelectListItem {Text = purpose, Value = purpose}).ToList();
            return
                View(new SendCodeViewModel {Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return View("Error");

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
                return View("Error");

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            else if (model.SelectedProvider == "Phone")
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);

            return RedirectToAction(nameof(VerifyCode),
                new {Provider = model.SelectedProvider, model.ReturnUrl, model.RememberMe});
        }

        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
                return View("Error");
            return View(new VerifyCodeViewModel {Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe});
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result =
                await
                    _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe,
                        model.RememberBrowser);
            if (result.Succeeded)
                return RedirectToLocal(model.ReturnUrl);
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion
    }
}