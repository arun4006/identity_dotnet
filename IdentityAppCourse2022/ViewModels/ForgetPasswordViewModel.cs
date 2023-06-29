using System.ComponentModel.DataAnnotations;

namespace IdentityAppCourse2022.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]      
        public string Email { get; set; }
    }
}
