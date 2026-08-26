using AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionApprovals;
using AssignmentSubmissionManagementSystem.Domain.Enums;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.ResubmissionApprovals;

public sealed class ReviewResubmissionRequestDtoValidator
    : AbstractValidator<ReviewResubmissionRequestDto>
{
    public ReviewResubmissionRequestDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.RequestId)
            .GreaterThan(0)
            .WithMessage("Request ID must be greater than 0.");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0)
            .WithMessage("Teacher ID must be greater than 0.");

        RuleFor(x => x.ApprovalStatus)
            .IsInEnum()
            .WithMessage("A valid approval status is required.");

        RuleFor(x => x.ApprovalStatus)
            .Must(status =>
                status == ApprovalStatus.Approved ||
                status == ApprovalStatus.Rejected)
            .WithMessage(
                "Approval status must be Approved or Rejected.");

        RuleFor(x => x.Remarks)
            .Must(value =>
                value is null ||
                !string.IsNullOrWhiteSpace(value))
            .WithMessage(
                "Remarks cannot contain only whitespace.");
    }
}