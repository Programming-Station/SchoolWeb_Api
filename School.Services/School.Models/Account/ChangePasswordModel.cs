using School.Models.CustomeVailidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Account
{
    public class ChangePasswordModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [MinLength(3, ErrorMessage = "{0} must be at least 3 characters long.")]
        [MaxLength(20, ErrorMessage = "{0} cannot exceed 20 characters.")]
        public required string UserName { get; set; }

        [Required]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        public required string OldPassword { get; set; }

        [Required]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [PasswordComplexity]
        public required string NewPassword { get; set; }
    }
}
