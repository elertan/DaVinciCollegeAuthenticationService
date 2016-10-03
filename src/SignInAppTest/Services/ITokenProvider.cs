namespace DaVinciCollegeAuthenticationService.Services
{
    public interface ITokenProvider
    {
        string GetToken();
        bool ValidateToken(string token);
    }

    public class TokenProvider : ITokenProvider
    {
        public string GetToken()
        {
            return "";
        }

        public bool ValidateToken(string token)
        {
            if (token == "abc") return true;
            return false;
        }
    }
}