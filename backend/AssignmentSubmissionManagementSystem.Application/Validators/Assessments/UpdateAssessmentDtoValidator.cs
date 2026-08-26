using AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Assessments;

public sealed class UpdateAssessmentDtoValidator
    : AbstractValidator<UpdateAssessmentDto>
{
    public UpdateAssessmentDtoValidator()
    {
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