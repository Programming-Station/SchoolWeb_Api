using FluentValidation;
using School_DTOs.Hr;

namespace School_API.Validators.Hr
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                                 .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("Mobile Number is required.")
                                        .Matches(@"^\d{10}$").WithMessage("Mobile Number must be 10 digits.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Department is required.");
            RuleFor(x => x.JoiningDate).NotEmpty().WithMessage("Joining Date is required.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of Birth is required.");
        }
    }

    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Employee Id.");
            RuleFor(x => x.EmployeeCode).NotEmpty().WithMessage("Employee Code is required.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                                 .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(x => x.MobileNumber).NotEmpty().WithMessage("Mobile Number is required.")
                                        .Matches(@"^\d{10}$").WithMessage("Mobile Number must be 10 digits.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.");
            RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("Department is required.");
        }
    }
}
