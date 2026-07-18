using FluentValidation;
using School_DTOs.Fee;

namespace School_API.Validators.Fee
{
    public class FeeTypeDtoValidator : AbstractValidator<FeeTypeDto>
    {
        public FeeTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Fee type name is required.")
                .MaximumLength(100).WithMessage("Fee type name cannot exceed 100 characters.");
        }
    }

    public class FeeStructureDtoValidator : AbstractValidator<FeeStructureDto>
    {
        public FeeStructureDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Fee structure name is required.")
                .MaximumLength(200).WithMessage("Fee structure name cannot exceed 200 characters.");

            RuleFor(x => x.CampusId)
                .GreaterThan(0).WithMessage("Campus is required.");

            RuleFor(x => x.ProgramId)
                .GreaterThan(0).WithMessage("Program is required.");

            RuleFor(x => x.BatchId)
                .GreaterThan(0).WithMessage("Batch is required.");

            RuleForEach(x => x.FeeStructureItems).ChildRules(item =>
            {
                item.RuleFor(i => i.FeeTypeId)
                    .GreaterThan(0).WithMessage("Fee type is required for each item.");
                item.RuleFor(i => i.Amount)
                    .GreaterThanOrEqualTo(0).WithMessage("Amount must be zero or positive.");
            });
        }
    }
}
