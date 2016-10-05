using System.Collections.Generic;

namespace DaVinciCollegeAuthenticationService.Models.ApplicationViewModels
{
    public class IndexViewModel
    {
        public ICollection<Application> Applications { get; set; }
    }
}