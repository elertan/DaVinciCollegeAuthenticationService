using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}