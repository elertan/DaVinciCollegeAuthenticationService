using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaVinciCollegeAuthenticationService.Data;
using DaVinciCollegeAuthenticationService.Models;
using DaVinciCollegeAuthenticationService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class ExcelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExcelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PostExcelSheet(IFormFile file)
        {
            byte[] fileBytes = null;
            var reader = new BinaryReader(file.OpenReadStream());
            fileBytes = reader.ReadBytes((int) file.Length);

            var fileDataResult = Encoding.UTF8.GetString(fileBytes);
            ViewBag.text = fileDataResult;
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
                    UserName = userName,
                    Email = userName + "@mydavinci.nl"
                    //Only works for students with an email like this 99027705@mydavinci.nl
                }).ToList();

            foreach (var user in newUsers)
            {
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) continue;
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                break;
            }

            return View(nameof(Index));
        }
    }
}