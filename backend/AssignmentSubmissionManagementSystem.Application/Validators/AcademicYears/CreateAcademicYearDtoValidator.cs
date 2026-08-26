using AssignmentSubmissionManagementSystem.Application.DTOs.AcademicYears;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.AcademicYears;

public sealed class CreateAcademicYearDtoValidator
    : AbstractValidator<CreateAcademicYearDto>
{
    public CreateAcademicYearDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.BatchId)
            .GreaterThan(0)
            .WithMessage("Batch ID must be greater than 0.");

        RuleFor(x => x.YearName)
            .NotEmpty()
            .WithMessage("Year name is required.")
            .MaximumLength(50)
            .WithMessage("Year name cannot exceed 50 characters.");

        RuleFor(x => x.YearOrder)
            .GreaterThan(0)
            .WithMessage("Year order must be greater than 0.");
    }
}