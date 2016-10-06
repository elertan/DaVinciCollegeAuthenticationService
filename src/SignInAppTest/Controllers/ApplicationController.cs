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
            return View(new IndexViewModel {Applications = applications});
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
                    Token = Guid.NewGuid()
                };
                _context.Applications.Add(application);
                var contextUser = _context.ApplicationUser.First(u => u.Id == user.Id);
                contextUser.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Route("application/delete/{applicationId}")]
        public async Task<IActionResult> Delete(int applicationId)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var applicationToRemove = _context.Applications.FirstOrDefault(a => a.Id == applicationId);
            if (applicationToRemove == null) return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(User);
            if (user != applicationToRemove.User) return RedirectToAction(nameof(Index));

            _context.Applications.Remove(_context.Applications.First(a => a.Id == applicationId));
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("application/update/{applicationId}")]
        public async Task<IActionResult> Update(int applicationId)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var applicationToUpdate = _context.Applications.FirstOrDefault(a => a.Id == applicationId);

            if (applicationToUpdate == null) return RedirectToAction(nameof(Index));
            var user = await _userManager.GetUserAsync(User);
            if (user == applicationToUpdate.User)
                return View(new UpdateViewModel {Application = applicationToUpdate});

            return RedirectToAction(nameof(Index));
        }
    }
}