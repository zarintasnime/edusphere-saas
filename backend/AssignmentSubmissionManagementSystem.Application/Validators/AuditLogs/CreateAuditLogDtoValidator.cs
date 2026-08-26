using AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.AuditLogs;

public sealed class CreateAuditLogDtoValidator
    : AbstractValidator<CreateAuditLogDto>
{
    public CreateAuditLogDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .When(x => x.InstitutionId.HasValue)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Action)
            .IsInEnum()
            .WithMessage("A valid audit action is required.");

        RuleFor(x => x.EntityName)
            .NotEmpty()
            .WithMessage("Entity name is required.")
            .MaximumLength(100)
            .WithMessage("Entity name cannot exceed 100 characters.");

        RuleFor(x => x.EntityId)
            .GreaterThan(0)
            .WithMessage("Entity ID must be greater than 0.");
    }
}