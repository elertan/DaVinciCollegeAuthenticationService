using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaVinciCollegeAuthenticationService.Models
{
    public class Application
    {
        public virtual ApplicationUser User { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Token { get; set; }

        public string LoginCallbackUrl { get; set; }
    }
}