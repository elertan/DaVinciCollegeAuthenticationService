using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Gebruikernummer")]
        public string UserNumber { get; set; }
    }
}