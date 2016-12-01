using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Gebruikernaam")]
        public string UserNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Onthoudt mij?")]
        public bool RememberMe { get; set; }
    }
}