namespace DaVinciCollegeAuthenticationService.Models
{
    public class ApplicationUserHasAuthLevel
    {
        public int Id { get; set; }

        public string UserNumber { get; set; }
        public int AuthLevel { get; set; }

        public virtual Application App { get; set; }
    }
}