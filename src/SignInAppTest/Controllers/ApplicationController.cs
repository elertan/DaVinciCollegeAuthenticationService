using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DaVinciCollegeAuthenticationService.Services;

namespace DaVinciCollegeAuthenticationService.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ITokenProvider _tokenProvider;
        public ApplicationController(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}