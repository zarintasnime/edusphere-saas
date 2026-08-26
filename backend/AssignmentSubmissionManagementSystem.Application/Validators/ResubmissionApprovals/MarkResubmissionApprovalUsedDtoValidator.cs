using AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionApprovals;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.ResubmissionApprovals;

public sealed class MarkResubmissionApprovalUsedDtoValidator
    : AbstractValidator<MarkResubmissionApprovalUsedDto>
{
    public MarkResubmissionApprovalUsedDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.ApprovalId)
            .GreaterThan(0)
            .WithMessage("Approval ID must be greater than 0.");
    }
}