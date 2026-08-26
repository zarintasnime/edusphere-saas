using AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Assessments;

public sealed class CreateAssessmentDtoValidator
    : AbstractValidator<CreateAssessmentDto>
{
    public CreateAssessmentDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.SubmissionId)
            .GreaterThan(0)
            .WithMessage("Submission ID must be greater than 0.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0)
            .WithMessage("Teacher ID must be greater than 0.");

        RuleFor(x => x.PolicyId)
            .GreaterThan(0)
            .When(x => x.PolicyId.HasValue)
            .WithMessage("Policy ID must be greater than 0.");

        RuleFor(x => x.MarksObtained)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Marks obtained cannot be negative.");

        RuleFor(x => x.Feedback)
            .Must(value =>
                value is null ||
                !string.IsNullOrWhiteSpace(value))
            .WithMessage("Feedback cannot contain only whitespace.");
    }
}