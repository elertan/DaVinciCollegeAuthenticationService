﻿using System.ComponentModel.DataAnnotations;

namespace DaVinciCollegeAuthenticationService.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}