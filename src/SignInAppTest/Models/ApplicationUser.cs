using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DaVinciCollegeAuthenticationService.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string UserNumber { get; set; }
        public bool IsTeacher { get; set; }
        public List<Application> Applications { get; set; }
        // Does this work
    }
}