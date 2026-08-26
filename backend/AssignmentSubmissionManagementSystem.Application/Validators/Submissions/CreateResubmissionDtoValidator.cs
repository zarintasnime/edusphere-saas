using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Submissions;

public sealed class CreateResubmissionDtoValidator
    : AbstractValidator<CreateResubmissionDto>
{
    public CreateResubmissionDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.AssignmentId)
            .GreaterThan(0)
            .WithMessage("Assignment ID must be greater than 0.");

        RuleFor(x => x.StudentId)
            .GreaterThan(0)
            .WithMessage("Student ID must be greater than 0.");

        RuleFor(x => x.SubmissionText)
            .Must(value =>
                value is null ||
                !string.IsNullOrWhiteSpace(value))
            .WithMessage(
                "Submission text cannot contain only whitespace.");
    }
}