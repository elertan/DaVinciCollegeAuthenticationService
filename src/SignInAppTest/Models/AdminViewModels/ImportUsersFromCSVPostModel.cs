using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaVinciCollegeAuthenticationService.Models.AdminViewModels
{
    public class ImportUsersFromCSVPostModel
    {
        public SuccessType SuccessType { get; set; }
    }

    public enum SuccessType
    {
        None,
        Failed,
        Succeeded
    }
}
