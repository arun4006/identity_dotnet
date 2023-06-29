using System.ComponentModel.DataAnnotations;

namespace IdentityAppCourse2022.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Passowrd")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Passowrd")]
        [Compare("Password",ErrorMessage ="Password don't match..!")]
        public string ConfirmPassword { get; set; }

        public string? Code { get; set; }    
        public string? __RequestVerificationToken { get; set; }    


    }
}
