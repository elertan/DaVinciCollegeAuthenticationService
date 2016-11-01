using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Models.SsoViewModels
{
    public class LoginViewModel
    {
        public Application Application { get; set; }

        public string ReturnUrl { get; set; }
    }
}
