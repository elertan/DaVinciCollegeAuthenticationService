using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Services
{
    public interface IEmailProvider
    {
        string GetEmailByUserNumber(string userNumber);
    }

    public class EmailProvider : IEmailProvider
    {
        public string GetEmailByUserNumber(string userNumber) => $"{userNumber}@mydavinci.nl";
    }
}
