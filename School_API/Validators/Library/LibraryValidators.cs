using FluentValidation;
using School_DTOs.Library;

namespace School_API.Validators.Library
{
    public class CreateBookDtoValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Book title is required.")
                .MaximumLength(500).WithMessage("Title cannot exceed 500 characters.");

            RuleFor(x => x.ISBN)
                .MaximumLength(20).WithMessage("ISBN cannot exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.ISBN));

            RuleFor(x => x.AccessionNumber)
                .NotEmpty().WithMessage("Accession number is required.")
                .MaximumLength(50).WithMessage("Accession number cannot exceed 50 characters.");

            RuleFor(x => x.TotalCopies)
                .GreaterThanOrEqualTo(0).WithMessage("Total copies cannot be negative.");

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Purchase price cannot be negative.");

            RuleFor(x => x.MinimumStock)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum stock cannot be negative.");
        }
    }

    public class CreateBookIssueDtoValidator : AbstractValidator<CreateBookIssueDto>
    {
        public CreateBookIssueDtoValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book is required.");

            RuleFor(x => x.DaysToBorrow)
                .InclusiveBetween(1, 365).WithMessage("Borrow period must be between 1 and 365 days.");
        }
    }

    public class CreateBookCategoryDtoValidator : AbstractValidator<CreateBookCategoryDto>
    {
        public CreateBookCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(200).WithMessage("Category name cannot exceed 200 characters.");
        }
    }

    public class CreateBookAuthorDtoValidator : AbstractValidator<CreateBookAuthorDto>
    {
        public CreateBookAuthorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Author name is required.")
                .MaximumLength(200).WithMessage("Author name cannot exceed 200 characters.");
        }
    }

    public class CreateBookPublisherDtoValidator : AbstractValidator<CreateBookPublisherDto>
    {
        public CreateBookPublisherDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Publisher name is required.")
                .MaximumLength(300).WithMessage("Publisher name cannot exceed 300 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }

    public class CreateBookVendorDtoValidator : AbstractValidator<CreateBookVendorDto>
    {
        public CreateBookVendorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Vendor name is required.")
                .MaximumLength(300).WithMessage("Vendor name cannot exceed 300 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }

    public class CreateLibraryMemberDtoValidator : AbstractValidator<CreateLibraryMemberDto>
    {
        public CreateLibraryMemberDtoValidator()
        {
            RuleFor(x => x.MemberType)
                .NotEmpty().WithMessage("Member type is required.");

            RuleFor(x => x.MemberName)
                .NotEmpty().WithMessage("Member name is required.")
                .MaximumLength(200).WithMessage("Member name cannot exceed 200 characters.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.Today).WithMessage("Expiry date must be in the future.");

            RuleFor(x => x.BorrowLimit)
                .InclusiveBetween(1, 50).WithMessage("Borrow limit must be between 1 and 50.");
        }
    }

    public class CreateBookReservationDtoValidator : AbstractValidator<CreateBookReservationDto>
    {
        public CreateBookReservationDtoValidator()
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book is required.");

            RuleFor(x => x.MemberId)
                .GreaterThan(0).WithMessage("Member is required.");
        }
    }

    public class CreateFineRuleDtoValidator : AbstractValidator<CreateFineRuleDto>
    {
        public CreateFineRuleDtoValidator()
        {
            RuleFor(x => x.RuleName)
                .NotEmpty().WithMessage("Rule name is required.")
                .MaximumLength(200).WithMessage("Rule name cannot exceed 200 characters.");

            RuleFor(x => x.PerDayFine)
                .GreaterThanOrEqualTo(0).WithMessage("Per day fine cannot be negative.");

            RuleFor(x => x.MaxFine)
                .GreaterThanOrEqualTo(0).WithMessage("Max fine cannot be negative.");

            RuleFor(x => x.GraceDays)
                .GreaterThanOrEqualTo(0).WithMessage("Grace days cannot be negative.");
        }
    }
}
