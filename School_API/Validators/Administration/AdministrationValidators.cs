using FluentValidation;
using School_DTOs.Administration;

namespace School_API.Validators.Administration
{
    public class CreateComplaintDtoValidator : AbstractValidator<CreateComplaintDto>
    {
        public CreateComplaintDtoValidator()
        {
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Complaint subject is required.")
                .MaximumLength(200).WithMessage("Subject cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.");

            RuleFor(x => x.Priority)
                .Must(p => p == "Low" || p == "Medium" || p == "High" || p == "Critical")
                .WithMessage("Priority must be Low, Medium, High, or Critical.");
        }
    }

    public class CreateVisitorDtoValidator : AbstractValidator<CreateVisitorDto>
    {
        public CreateVisitorDtoValidator()
        {
            RuleFor(x => x.VisitorName)
                .NotEmpty().WithMessage("Visitor name is required.")
                .MaximumLength(200).WithMessage("Visitor name cannot exceed 200 characters.");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required.")
                .MaximumLength(20).WithMessage("Contact number cannot exceed 20 characters.");

            RuleFor(x => x.Purpose)
                .NotEmpty().WithMessage("Visit purpose is required.");

            RuleFor(x => x.NumberOfVisitors)
                .GreaterThan(0).WithMessage("Number of visitors must be at least 1.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }

    public class CreateCertificateIssuanceDtoValidator : AbstractValidator<CreateCertificateIssuanceDto>
    {
        public CreateCertificateIssuanceDtoValidator()
        {
            RuleFor(x => x.CertificateType)
                .NotEmpty().WithMessage("Certificate type is required.")
                .Must(t => t == "TC" || t == "Bonafide" || t == "Character" || t == "Migration" || t == "StudyCertificate" || t == "Custom")
                .WithMessage("Invalid certificate type.");

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required.");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("Academic year is required.");
        }
    }
}
