using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class ForgetPasswordVerificationModel
    {
        public PasswordReset PasswordReset { get; set; }

        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Check Password")]
        public string CheckPassword { get; set; }
    }
}
