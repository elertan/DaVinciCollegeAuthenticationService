using System;
using System.Linq;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Models.ApplicationViewModels;
using DaVinciCollegeAuthenticationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            ITokenProvider tokenProvider)
        {
            _context = context;
            _userManager = userManager;
            _tokenProvider = tokenProvider;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var applications = _context.Applications.Where(a => a.User == user).ToList();
            var domainName = Request.Host + (Request.Host.Port != null ? "" : ":" + Request.Host.Port);
            return View(new IndexViewModel {DomainName = domainName, Applications = applications});
        }


        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var application = new Application
                {
                    Name = model.Name,
                    LoginCallbackUrl = model.LoginCallbackUrl,
                    Token = Guid.NewGuid(),
                    Secret = model.Secret,
                    ValidFor = model.ValidFor,
                    ExtendExpiryOnRequest = model.ExtendExpiryOnRequest
                };
                _context.Applications.Add(application);
                var contextUser = _context.ApplicationUser.First(u => u.Id == user.Id);
                contextUser.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("Application/Delete/{applicationId}")]
        public async Task<IActionResult> Delete(int applicationId)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var applicationToRemove = _context.Applications.FirstOrDefault(a => a.Id == applicationId);
            if (applicationToRemove == null) return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(User);
            if (user != applicationToRemove.User) return RedirectToAction(nameof(Index));

            var auhalRemove =
                _context.ApplicationUserHasAuthLevels.Where(auhaul => auhaul.App.Id == applicationToRemove.Id);
            _context.ApplicationUserHasAuthLevels.RemoveRange(auhalRemove);

            _context.Applications.Remove(_context.Applications.First(a => a.Id == applicationId));
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("Application/Update/{applicationId}")]
        public async Task<IActionResult> Update(int applicationId)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var app = _context.Applications.FirstOrDefault(a => a.Id == applicationId);

            if (app == null)
                return RedirectToAction(nameof(Index));
            var user = await _userManager.GetUserAsync(User);
            if (user == app.User)
                return
                    View(new UpdateViewModel
                    {
                        Name = app.Name,
                        Secret = app.Secret,
                        LoginCallbackUrl = app.LoginCallbackUrl,
                        Token = app.Token.ToString(),
                        ValidFor = app.ValidFor,
                        ExtendExpiryOnRequest = app.ExtendExpiryOnRequest
                    });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Application/Update/{applicationId}")]
        public async Task<IActionResult> Update(UpdateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Token.Equals(Guid.Parse(model.Token)));
            if (app == null) return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(User);
            if (user != app.User) return RedirectToAction(nameof(Index));

            app.Name = model.Name;
            app.LoginCallbackUrl = model.LoginCallbackUrl;
            app.Secret = model.Secret;
            app.ValidFor = model.ValidFor;
            app.ExtendExpiryOnRequest = model.ExtendExpiryOnRequest;

            _context.Applications.Update(app);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("Application/EditUserAuthLevels/{applicationId}")]
        public async Task<IActionResult> EditUserAuthLevels(string applicationId)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id.ToString() == applicationId);
            if (app == null) return RedirectToAction(nameof(Index));

            app.ApplicationUsersHasAuthLevels =
                _context.ApplicationUserHasAuthLevels.Where(auhal => auhal.App.Id == app.Id).ToList();

            return
                View(new EditUserAuthLevelsViewModel
                {
                    Application = app
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Application/EditUserAuthLevels/{applicationId}")]
        public async Task<IActionResult> EditUserAuthLevels(string applicationId,
            ApplicationUserHasAuthLevel[] authLevels)
        {
            var app = await _context.Applications.FirstOrDefaultAsync(a => a.Id.ToString() == applicationId);
            if (app == null) return RedirectToAction(nameof(Index));

            app.ApplicationUsersHasAuthLevels =
                _context.ApplicationUserHasAuthLevels.Where(auhal => auhal.App.Id == app.Id).ToList();

            _context.ApplicationUserHasAuthLevels.RemoveRange(app.ApplicationUsersHasAuthLevels);
            app.ApplicationUsersHasAuthLevels.Clear();

            _context.ApplicationUserHasAuthLevels.AddRange(authLevels);
            app.ApplicationUsersHasAuthLevels.AddRange(authLevels);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}