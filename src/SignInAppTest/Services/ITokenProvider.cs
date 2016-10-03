using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return "test getToken";
        }

        public bool ValidateToken(string token)
        {
            if (token == "mark")
            {
                return true;
            }

            return false;
        }
    }
}
