using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class ForgetPasswordVerificationModel
    {
        public PasswordReset PasswordReset { get; set; }

        [Required]
        [Display(Name = "Nieuw Wachtwoord")]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Herhaal Nieuw Wachtwoord")]
        public string CheckPassword { get; set; }
    }
}