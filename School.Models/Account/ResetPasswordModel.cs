using School.Models.CustomeVailidation;
using System.ComponentModel.DataAnnotations;

namespace School.Models.Account
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "Please enter correct email address")]
        public required string EmailId { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public DateTime ExpireTime { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [NoScript(ErrorMessage = "{0} cannot contained.")]
        [PasswordComplexity]
        public required string NewPassword { get; set; }
    }
}
