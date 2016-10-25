using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models
{
    public class Accesstoken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public virtual Application App { get; set; }
    }
}