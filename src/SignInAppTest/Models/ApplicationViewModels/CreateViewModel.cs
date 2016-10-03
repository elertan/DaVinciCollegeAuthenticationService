using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Models.ApplicationViewModels
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name= "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name= "Login Callback Url")]
        public string LoginCallbackUrl { get; set; }
    }
}
