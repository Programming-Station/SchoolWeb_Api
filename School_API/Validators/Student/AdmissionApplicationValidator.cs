using FluentValidation;
using School_DTOs.Student;

namespace School_API.Validators.Student
{
    /// <summary>Validates the admission application form before it is persisted.</summary>
    public class CreateAdmissionApplicationDtoValidator : AbstractValidator<AdmissionApplicationDto>
    {
        public CreateAdmissionApplicationDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Student full name is required.")
                .MaximumLength(150).WithMessage("Full name cannot exceed 150 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.")
                .GreaterThan(DateTime.Today.AddYears(-30)).WithMessage("Date of birth appears too far in the past.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be Male, Female, or Other.");

            RuleFor(x => x.Mobile)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^\d{10}$").WithMessage("Mobile number must be exactly 10 digits.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email address is required.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("Academic year is required.");

            RuleFor(x => x.EducationLevelId)
                .GreaterThan(0).WithMessage("Education level is required.");

            RuleFor(x => x.PermanentAddress)
                .MaximumLength(500).WithMessage("Permanent address cannot exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.PermanentAddress));

            RuleFor(x => x.AadhaarNo)
                .Matches(@"^\d{12}$").WithMessage("Aadhaar number must be exactly 12 digits.")
                .When(x => !string.IsNullOrEmpty(x.AadhaarNo));
        }
    }
}
