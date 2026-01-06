using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace School.Models.CustomeVailidation
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            List<string> errorMessage = new List<string>();
            if (value is string password)
            {
                if (string.IsNullOrEmpty(password))
                {
                    return ValidationResult.Success!;
                }
                // Check length
                if (password.Length < 8 || password.Length > 16)
                {
                    errorMessage.Add("Password must be between 8 and 16 characters long.");
                }

                // Check for at least one uppercase letter
                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    errorMessage.Add("Password must contain at least one uppercase letter.");
                }

                // Check for at least one lowercase letter
                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    errorMessage.Add("Password must contain at least one lowercase letter.");
                }

                // Check for at least one digit
                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    errorMessage.Add("Password must contain at least one digit.");
                }

                // Check for at least one special character
                if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':;{}|<>]"))
                {
                    errorMessage.Add("Password must contain at least one special character.");
                }

                if (errorMessage != null && errorMessage.Count > 0)
                    return new ValidationResult(string.Join(" ", errorMessage));
            }

            return ValidationResult.Success!;
        }
    }
}
