﻿using System.ComponentModel.DataAnnotations;

namespace IdentityAppCourse2022.ViewModels
{
    public class LoginViewModel
    {
      
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public Boolean RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
