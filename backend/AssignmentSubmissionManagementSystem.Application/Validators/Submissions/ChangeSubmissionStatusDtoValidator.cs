using AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Submissions;

public sealed class ChangeSubmissionStatusDtoValidator
    : AbstractValidator<ChangeSubmissionStatusDto>
{
    public ChangeSubmissionStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("A valid submission status is required.");
    }
}