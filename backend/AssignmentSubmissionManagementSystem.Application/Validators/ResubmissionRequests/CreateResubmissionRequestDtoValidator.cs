using AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionRequests;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.ResubmissionRequests;

public sealed class CreateResubmissionRequestDtoValidator
    : AbstractValidator<CreateResubmissionRequestDto>
{
    public CreateResubmissionRequestDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.SubmissionId)
            .GreaterThan(0)
            .WithMessage("Submission ID must be greater than 0.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required.");
    }
}