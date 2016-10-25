using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.ApplicationViewModels
{
    public class UpdateViewModel
    {
        [Required]
        public string Token { get; set; }

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

        [Required]
        [Display(Name = "Geldig Duratie (Verlopen van accesstoken) in seconden")]
        [DataType(DataType.Duration)]
        public int ValidFor { get; set; } = 1800;

        [Required]
        [Display(Name = "Verloop van accesstoken verlengen op aanvraag?")]
        public bool ExtendExpiryOnRequest { get; set; } = true;
    }
}