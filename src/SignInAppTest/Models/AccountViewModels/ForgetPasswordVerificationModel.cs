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
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Check Password")]
        [DataType(DataType.Password)]
        public string CheckPassword { get; set; }
    }
}
