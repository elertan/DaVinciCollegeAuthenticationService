using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using DaVinciCollegeAuthenticationService.Models;
using Microsoft.AspNetCore.Identity;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public ExcelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            ViewBag.text = "mark";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostExcelSheet(IFormFile file)
        {
            byte[] fileBytes = null;
            BinaryReader reader = new BinaryReader(file.OpenReadStream());
            fileBytes = reader.ReadBytes((int)file.Length);

            string fileDataResult = System.Text.Encoding.UTF8.GetString(fileBytes);
            ViewBag.text = fileDataResult;
            List<string> rows = new List<string>();
            foreach (var item in fileDataResult.Split(new char[] { '"' }))
            {
                if (item.Length > 1)
                {
                    int testResult;
                    if (int.TryParse(item[1].ToString(), out testResult))
                    {
                        rows.Add(item);
                    }
                }
            }

            foreach (var row in rows)
            {
                var splittedRow = row.Split(new char[] { ';' });//TODO: add 3 values to ApplicationUser the FirstName, MiddleName and LastName
                var userName = splittedRow[0];
                var firstName = splittedRow[1];
                var middleName = splittedRow[2];
                var lastName = splittedRow[3];

                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userName + "@mydavinci.nl"//Only works for students with an email like this 99027705@mydavinci.nl
                };

                //var mark = await _userManager.DeleteAsync(user);
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(user.Email, "Confirm your account", $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _signInManager.SignInAsync(user, false);
                }
            }

            return View(nameof(Index));
        }
    }
}
