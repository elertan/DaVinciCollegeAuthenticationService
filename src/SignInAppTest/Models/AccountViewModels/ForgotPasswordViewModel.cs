using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "GebruikerNaam")]
        public string UserName { get; set; }
    }
}