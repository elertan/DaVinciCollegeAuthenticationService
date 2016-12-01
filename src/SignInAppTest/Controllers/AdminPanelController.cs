using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Data;
using Microsoft.AspNetCore.Identity;
using DaVinciCollegeAuthenticationService.Services;
using DaVinciCollegeAuthenticationService.Models.AdminViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPanelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportUsersFromCSV()
        {
            return View(new ImportUsersFromCSVPostModel() { SuccessType = SuccessType.None });
        }

        [HttpPost]
        public async Task<IActionResult> ImportUsersFromCSVPost(IFormFile file)
        {
            var reader = new BinaryReader(file.OpenReadStream());
            var fileBytes = reader.ReadBytes((int)file.Length);

            var fileDataResult = Encoding.UTF8.GetString(fileBytes);
            var rows = new List<string>();
            foreach (var item in fileDataResult.Split('"'))
            {
                if (item.Length <= 1) continue;

                int testResult;
                if (int.TryParse(item[1].ToString(), out testResult))
                    rows.Add(item);
            }

            var newUsers = (from row in rows
                            select row.Split(';')
                into splittedRow
                            let userName = splittedRow[0]
                            let firstName = splittedRow[1]
                            let middleName = splittedRow[2]
                            let lastName = splittedRow[3]
                            select new ApplicationUser
                            {
                                Lastname = lastName,
                                Prefix = middleName,
                                Firstname = firstName,
                                UserName = userName,
                                Email = userName + "@mydavinci.nl"
                                //Only works for students with an email like this 99027705@mydavinci.nl
                            }).ToList();

            foreach (var user in newUsers)
            {
                var checkUser = _context.ApplicationUser.FirstOrDefault(a => a.UserName == user.UserName);
                if (checkUser == null)//User doesnt exist yet
                {
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        continue;
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return View(nameof(ImportUsersFromCSV), new ImportUsersFromCSVPostModel() { SuccessType = SuccessType.Failed });
                }
                else
                {
                    checkUser.Lastname = user.Lastname;
                    checkUser.Prefix = user.Prefix;
                    checkUser.Firstname = user.Firstname;
                    checkUser.UserName = user.UserName;
                    await _context.SaveChangesAsync();
                }
            }

            return View(nameof(ImportUsersFromCSV), new ImportUsersFromCSVPostModel() { SuccessType = SuccessType.Succeeded });
        }
    }
}
