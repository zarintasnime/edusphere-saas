using AssignmentSubmissionManagementSystem.Application.DTOs.Batches;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Batches;

public sealed class CreateBatchDtoValidator
    : AbstractValidator<CreateBatchDto>
{
    public CreateBatchDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID must be greater than 0.");

        RuleFor(x => x.BatchCode)
            .NotEmpty()
            .WithMessage("Batch code is required.")
            .MaximumLength(20)
            .WithMessage("Batch code cannot exceed 20 characters.");

        RuleFor(x => x.BatchName)
            .NotEmpty()
            .WithMessage("Batch name is required.")
            .MaximumLength(100)
            .WithMessage("Batch name cannot exceed 100 characters.");

        RuleFor(x => x.StartYear)
            .GreaterThan(0)
            .WithMessage("Start year must be greater than 0.");

        RuleFor(x => x.EndYear)
            .GreaterThanOrEqualTo(x => x.StartYear)
            .When(x => x.EndYear.HasValue)
            .WithMessage("End year must be greater than or equal to start year.");
    }
}