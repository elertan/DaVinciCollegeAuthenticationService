using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.ApplicationViewModels
{
    public class CreateViewModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "Login Callback Url")]
        public string LoginCallbackUrl { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        [Display(Name = "Secret Code")]
        public string Secret { get; set; }
    }
}