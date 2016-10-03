using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaVinciCollegeAuthenticationService.Services;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models.ApplicationViewModels;
using DaVinciCollegeAuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenProvider _tokenProvider;
        public ApplicationController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITokenProvider tokenProvider)
        {
            _context = context;
            _userManager = userManager;
            _tokenProvider = tokenProvider;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(new IndexViewModel() { Applications = user.Applications });
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
                var application = new Application() { Name = model.Name, LoginCallbackUrl = model.LoginCallbackUrl };
                _context.Applications.Add(application);
                var contextUser = _context.ApplicationUser.First(u => u.Id == user.Id);
                contextUser.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ApplicationController.Create));
            }

            return View(model);
        }
    }
}