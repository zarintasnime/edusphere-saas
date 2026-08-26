using AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Assignments;

public sealed class ChangeAssignmentStatusDtoValidator
    : AbstractValidator<ChangeAssignmentStatusDto>
{
    public ChangeAssignmentStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("A valid assignment status is required.");
    }
}