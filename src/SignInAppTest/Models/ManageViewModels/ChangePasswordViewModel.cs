using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord opnieuw")]
        [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord komt niet overeen met het opnieuw ingevulde veld.")]
        public string ConfirmPassword { get; set; }
    }
}
