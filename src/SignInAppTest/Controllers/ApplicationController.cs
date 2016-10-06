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
            var applications = _context.Applications.Where(a => a.User == user).ToList();
            return View(new IndexViewModel() { Applications = applications });
        }


        public IActionResult Create()
        {
            return View(new CreateViewModel() { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var application = new Application() { Name = model.Name, LoginCallbackUrl = model.LoginCallbackUrl, Token = Guid.NewGuid() };
                _context.Applications.Add(application);
                var contextUser = _context.ApplicationUser.First(u => u.Id == user.Id);
                contextUser.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ApplicationController.Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int applicationId)
        {
            if (ModelState.IsValid)
            {
                var applicationToRemove = _context.Applications.FirstOrDefault(a => a.Id == applicationId);
                if (applicationToRemove != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == applicationToRemove.User)
                    {
                        _context.Applications.Remove(_context.Applications.First(a => a.Id == applicationId));
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(ApplicationController.Index));
        }

        public async Task<IActionResult> Update(int applicationId)
        {
            if (ModelState.IsValid)
            {
                var applicationToUpdate = _context.Applications.FirstOrDefault(a => a.Id == applicationId);
                if (applicationToUpdate != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == applicationToUpdate.User)
                    {
                        return View(new UpdateViewModel() { Application = applicationToUpdate });
                    }
                }
            }

            return RedirectToAction(nameof(ApplicationController.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var applicationToUpdate = await _context.Applications.FirstOrDefaultAsync(a => a == model.Application);
                if (applicationToUpdate != null)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == applicationToUpdate.User)
                    {
                        applicationToUpdate.Name = model.Application.Name;
                        applicationToUpdate.LoginCallbackUrl = applicationToUpdate.LoginCallbackUrl;

                        _context.Applications.Update(applicationToUpdate);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(ApplicationController.Index));
                    }
                }
            }

            return RedirectToAction(nameof(ApplicationController.Index));
        }
    }
}