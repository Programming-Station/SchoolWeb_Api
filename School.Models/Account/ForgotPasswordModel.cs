using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models.Account
{
    public class ForgotPasswordModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [MinLength(3, ErrorMessage = "{0} must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "{0} cannot exceed 20 characters.")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [EmailAddress(ErrorMessage = "Please enter correct email address")]
        public string Email { get; set; } = default!;

        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [MaxLength(10, ErrorMessage = "{0} cannot exceed less than 10 characters.")]
        public string MobileNumber { get; set; } = default!;
    }
}
