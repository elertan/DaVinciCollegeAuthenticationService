using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.ApplicationViewModels
{
    public class CreateViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Login Callback Url")]
        public string LoginCallbackUrl { get; set; }
    }
}