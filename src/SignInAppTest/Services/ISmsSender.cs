using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}