using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaVinciCollegeAuthenticationService.Models
{
    public class Application
    {
        public virtual ApplicationUser User { get; set; }

        public int Id { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        //[Display(Name = "Secret Code")]
        [MinLength(5)]
        [MaxLength(100)]
        public string Secret { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Token { get; set; }

        [DataType(DataType.Url)]
        public string LoginCallbackUrl { get; set; }

        [DataType(DataType.Duration)]
        public int ValidFor { get; set; }

        public bool ExtendExpiryOnRequest { get; set; }
    }
}