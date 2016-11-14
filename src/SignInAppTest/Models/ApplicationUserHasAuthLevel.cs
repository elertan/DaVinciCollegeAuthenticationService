namespace DaVinciCollegeAuthenticationService.Models
{
    public class ApplicationUserHasAuthLevel
    {
        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public int AuthLevel { get; set; }
    }
}