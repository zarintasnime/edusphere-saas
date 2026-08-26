using AssignmentSubmissionManagementSystem.Application.DTOs.LateSubmissionPolicies;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.LateSubmissionPolicies;

public sealed class CreateLateSubmissionPolicyDtoValidator
    : AbstractValidator<CreateLateSubmissionPolicyDto>
{
    public CreateLateSubmissionPolicyDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.PolicyName)
            .NotEmpty()
            .WithMessage("Policy name is required.")
            .MaximumLength(100)
            .WithMessage("Policy name cannot exceed 100 characters.");

        RuleFor(x => x.PenaltyPercentage)
            .InclusiveBetween(0, 100)
            .WithMessage("Penalty percentage must be between 0 and 100.");
    }
}