using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Models.SsoViewModels;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Sso/Login/{token}")]
        public async Task<IActionResult> Login(string token, string returnUrl)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid)) return View(null);

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            if (app == null)
                return View("InvalidLoginToken");

            var model = new LoginViewModel {Application = app, ReturnUrl = returnUrl};
            return _signInManager.IsSignedIn(User) ? View("LoginContinue", model) : View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Sso/Login/{token}")]
        public async Task<IActionResult> LoginPost(string userNumber, string password, string token, string returnUrl)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));

            var result = await _signInManager.PasswordSignInAsync(userNumber, password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Inloggen mislukt.");
                return RedirectToAction("Login", new {token});
            }

            var authLevel =
                await _context.ApplicationUserHasAuthLevels.FirstOrDefaultAsync(
                    auhal => (auhal.App.Id == app.Id) && (auhal.UserNumber == userNumber));

            var user = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.UserName == userNumber);
            var payload = new Dictionary<string, object>
            {
                {"userNumber", userNumber},
                {"firstName", user.Firstname},
                {"prefix", user.Prefix},
                {"lastName", user.Lastname},
                {"expiry", DateTime.Now.AddSeconds(app.ValidFor).Ticks.ToString()},
                {
                    "authLevel", authLevel?.AuthLevel ?? 1
                }
            };

            var secretKey = Encoding.UTF8.GetBytes(app.Secret);
            var jwt = JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);

            _context.Accesstokens.Add(new Accesstoken {App = app, Token = jwt, ValidTill = DateTime.Now.AddSeconds(app.ValidFor)});
            await _context.SaveChangesAsync();

            var parametersToAdd = new Dictionary<string, string>
            {
                {"token", jwt}
            };

            if (returnUrl != null)
                parametersToAdd.Add("returnUrl", returnUrl);

            var url = QueryHelpers.AddQueryString(app.LoginCallbackUrl, parametersToAdd);
            return RedirectPermanent(url);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Sso/LoginContinue")]
        public async Task<IActionResult> LoginContinue(string token, string returnUrl)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            if (app == null) return BadRequest();

            var authLevel =
                await _context.ApplicationUserHasAuthLevels.FirstOrDefaultAsync(
                    auhal => (auhal.App.Id == app.Id) && (auhal.UserNumber == User.Identity.Name));


            var user = await _context.ApplicationUser.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            var payload = new Dictionary<string, object>
            {
                {"userNumber", User.Identity.Name},
                {"firstName", user.Firstname},
                {"prefix", user.Prefix},
                {"lastName", user.Lastname},
                {"expiry", DateTime.Now.AddSeconds(app.ValidFor).Ticks.ToString()},
                {
                    "authLevel", authLevel?.AuthLevel ?? 1
                }
            };

            var secretKey = Encoding.UTF8.GetBytes(app.Secret);
            var jwt = JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);

            _context.Accesstokens.Add(new Accesstoken {App = app, Token = jwt, ValidTill = DateTime.Now.AddSeconds(app.ValidFor)});
            await _context.SaveChangesAsync();

            var parametersToAdd = new Dictionary<string, string>
            {
                {"token", jwt}
            };

            if (returnUrl != null)
                parametersToAdd.Add("returnUrl", returnUrl);

            var url = QueryHelpers.AddQueryString(app.LoginCallbackUrl, parametersToAdd);
            return RedirectPermanent(url);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Sso/Logout")]
        public async Task<IActionResult> Logout(string token)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new {token});
        }

        public async Task<IActionResult> Cancel(string token)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            if (app == null) return BadRequest();

            var parametersToAdd = new Dictionary<string, string>
            {
                {"err", "canceled"}
            };

            var url = QueryHelpers.AddQueryString(app.LoginCallbackUrl, parametersToAdd);
            return RedirectPermanent(url);
        }

        [Route("Sso/ValidateAuth/{appId}/{token}")]
        public async Task<IActionResult> ValidateAuth(string appId, string token)
        {
            Guid guid;
            if (!Guid.TryParse(appId, out guid))
                return Json(new {err = "Invalid AppId"});

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            if (app == null) return Json(new {err = "Invalid AppId"});

            var accessToken = await _context.Accesstokens.FirstOrDefaultAsync(at => at.Token == token);
            if (accessToken == null) return Json(new {err = "Invalid Accesstoken"});

            if (!accessToken.App.Token.Equals(guid)) Json(new {err = "AppId doesn't match with Accesstoken"});

            var secretKey = Encoding.UTF8.GetBytes(app.Secret);

            var data = JWT.Decode<Dictionary<string, dynamic>>(token, secretKey, JwsAlgorithm.HS256);

            // Expiry is still not reached?
            if (accessToken.ValidTill > DateTime.Now)
            {
                if (!app.ExtendExpiryOnRequest) return Json(new {token});

                data["expiry"] = DateTime.Now.AddSeconds(app.ValidFor).Ticks.ToString();
                token = JWT.Encode(data, secretKey, JwsAlgorithm.HS256);
                accessToken.Token = token;
                accessToken.ValidTill = DateTime.Now.AddSeconds(app.ValidFor);
                await _context.SaveChangesAsync();
                return Json(new {token});
            }
            _context.Accesstokens.Remove(accessToken);
            await _context.SaveChangesAsync();

            return Json(new {err = "expired"});
        }
    }
}