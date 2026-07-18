using FluentValidation;
using School_DTOs.Hostel;

namespace School_API.Validators.Hostel
{
    public class CreateHostelDtoValidator : AbstractValidator<CreateHostelDto>
    {
        public CreateHostelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hostel name is required.")
                .MaximumLength(200).WithMessage("Hostel name cannot exceed 200 characters.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Hostel code is required.")
                .MaximumLength(20).WithMessage("Hostel code cannot exceed 20 characters.");

            RuleFor(x => x.HostelType)
                .NotEmpty().WithMessage("Hostel type is required.")
                .Must(t => t == "Boys" || t == "Girls" || t == "Staff")
                .WithMessage("Hostel type must be Boys, Girls, or Staff.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero.");

            RuleFor(x => x.EmergencyContact)
                .NotEmpty().WithMessage("Emergency contact is required.")
                .When(x => !string.IsNullOrEmpty(x.EmergencyContact));
        }
    }

    public class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
    {
        public CreateRoomDtoValidator()
        {
            RuleFor(x => x.RoomNumber)
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(20).WithMessage("Room number cannot exceed 20 characters.");

            RuleFor(x => x.HostelId)
                .GreaterThan(0).WithMessage("Hostel is required.");

            RuleFor(x => x.BuildingId)
                .GreaterThan(0).WithMessage("Building is required.");

            RuleFor(x => x.FloorId)
                .GreaterThan(0).WithMessage("Floor is required.");

            RuleFor(x => x.RoomCategoryId)
                .GreaterThan(0).WithMessage("Room category is required.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Room capacity must be greater than zero.");
        }
    }

    public class CreateBedDtoValidator : AbstractValidator<CreateBedDto>
    {
        public CreateBedDtoValidator()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("Room is required.");

            RuleFor(x => x.BedNumber)
                .NotEmpty().WithMessage("Bed number is required.")
                .MaximumLength(20).WithMessage("Bed number cannot exceed 20 characters.");
        }
    }

    public class CreateHostelAdmissionDtoValidator : AbstractValidator<CreateHostelAdmissionDto>
    {
        public CreateHostelAdmissionDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required.");

            RuleFor(x => x.HostelId)
                .GreaterThan(0).WithMessage("Hostel is required.");

            RuleFor(x => x.RoomId)
                .GreaterThan(0).WithMessage("Room is required.");

            RuleFor(x => x.BedId)
                .GreaterThan(0).WithMessage("Bed is required.");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("Academic year is required.");

            RuleFor(x => x.AdmissionDate)
                .NotEmpty().WithMessage("Admission date is required.");

            RuleFor(x => x.SecurityDeposit)
                .GreaterThanOrEqualTo(0).WithMessage("Security deposit cannot be negative.");
        }
    }

    public class CreateHostelVisitorDtoValidator : AbstractValidator<CreateHostelVisitorDto>
    {
        public CreateHostelVisitorDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required.");

            RuleFor(x => x.VisitorName)
                .NotEmpty().WithMessage("Visitor name is required.")
                .MaximumLength(200).WithMessage("Visitor name cannot exceed 200 characters.");

            RuleFor(x => x.Relation)
                .NotEmpty().WithMessage("Relation is required.");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required.");

            RuleFor(x => x.Purpose)
                .NotEmpty().WithMessage("Visit purpose is required.");
        }
    }

    public class CreateHostelGatePassDtoValidator : AbstractValidator<CreateHostelGatePassDto>
    {
        public CreateHostelGatePassDtoValidator()
        {
            RuleFor(x => x.AdmissionId)
                .GreaterThan(0).WithMessage("Admission ID is required.");

            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required.");

            RuleFor(x => x.OutTime)
                .NotEmpty().WithMessage("Out time is required.");

            RuleFor(x => x.ExpectedReturn)
                .NotEmpty().WithMessage("Expected return time is required.")
                .GreaterThan(x => x.OutTime).WithMessage("Expected return must be after out time.");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required.")
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.");
        }
    }

    public class CreateHostelComplaintDtoValidator : AbstractValidator<CreateHostelComplaintDto>
    {
        public CreateHostelComplaintDtoValidator()
        {
            RuleFor(x => x.StudentId)
                .GreaterThan(0).WithMessage("Student is required.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Complaint category is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

            RuleFor(x => x.Priority)
                .Must(p => p == "Low" || p == "Medium" || p == "High" || p == "Critical")
                .WithMessage("Priority must be Low, Medium, High, or Critical.");
        }
    }

    public class CreateHostelWardenDtoValidator : AbstractValidator<CreateHostelWardenDto>
    {
        public CreateHostelWardenDtoValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required.");

            RuleFor(x => x.HostelId)
                .GreaterThan(0).WithMessage("Hostel is required.");

            RuleFor(x => x.RoleType)
                .NotEmpty().WithMessage("Role type is required.")
                .Must(r => r == "ChiefWarden" || r == "AssistantWarden" || r == "Supervisor")
                .WithMessage("Role type must be ChiefWarden, AssistantWarden, or Supervisor.");
        }
    }
}
