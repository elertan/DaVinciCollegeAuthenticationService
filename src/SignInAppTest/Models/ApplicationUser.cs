using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DaVinciCollegeAuthenticationService.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Applications = new List<Application>();
        }

        public string UserNumber { get; set; }
        public bool IsTeacher { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
    }
}