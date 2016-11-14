using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Models
{
    public class PasswordReset
    {
        public int Id { get; set; }
        public int UserNumber { get; set; }
        public Guid VertificationCode { get; set; }
    }
}
