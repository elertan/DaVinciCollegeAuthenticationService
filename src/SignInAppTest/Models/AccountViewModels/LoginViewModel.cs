﻿using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Studentnummer")]
        public string UserNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}