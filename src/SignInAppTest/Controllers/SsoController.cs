using System;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models;
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
        /// <returns></returns>
        [AllowAnonymous]
        [Route("sso/login/{token}")]
        public async Task<IActionResult> Login(string token)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid)) return View(null);

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            return View(app);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sso/login/{token}")]
        public async Task<IActionResult> LoginPost(string userNumber, string password, string token)
        {
            Guid guid;
            if (!Guid.TryParse(token, out guid))
                return BadRequest();

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(guid));
            return RedirectPermanent(app.LoginCallbackUrl);
        }
    }
}