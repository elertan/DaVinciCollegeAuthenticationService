using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DaVinciCollegeAuthenticationService.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public sealed class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Applications = new List<Application>();
        }

        public string UserNumber { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Application> Applications { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Prefix { get; set; }
    }
}