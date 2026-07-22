using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using School.Models.CustomeVailidation;

namespace School.Models.Account
{
    public class ChangePasswordModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
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
