using System;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Models.SsoViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class SsoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SsoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     Provides a login page for the given app
        /// </summary>
        /// <param name="token">The token associated with the app</param>
        /// <param name="appSession"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("sso/login/{token}/{appSession}")]
        public async Task<IActionResult> Login(string token, string appSession)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid)) return View(null);

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            var model = new LoginViewModel {Application = app, ApplicationSession = appSession};
            return _signInManager.IsSignedIn(User) ? View("LoginContinue", model) : View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("sso/login/{token}/{appSession}")]
        public async Task<IActionResult> LoginPost(string userNumber, string password, string token, string appSession)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));

            var result = await _signInManager.PasswordSignInAsync(userNumber, password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Inloggen mislukt.");
                return RedirectToAction("Login", new {token, appSession});
            }
            return RedirectPermanent(app.LoginCallbackUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("sso/loginContinue")]
        public async Task<IActionResult> LoginContinue(string token, string appSession)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            return RedirectPermanent(app.LoginCallbackUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("sso/logout")]
        public async Task<IActionResult> Logout(string token, string appSession)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new {token, appSession});
        }
    }
}