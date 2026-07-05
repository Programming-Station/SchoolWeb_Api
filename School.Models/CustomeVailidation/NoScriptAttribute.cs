using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace School.Models.CustomeVailidation
{
    public class NoScriptAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string strValue)
            {
                var scriptRegex = new Regex(@"<script.*?>.*?</script>|<.*?>", RegexOptions.IgnoreCase);
                if (scriptRegex.IsMatch(strValue))
                {
                    return new ValidationResult("Input cannot contain script or HTML tags.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
