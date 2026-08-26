using AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Notifications;

public sealed class CreateNotificationDtoValidator
    : AbstractValidator<CreateNotificationDto>
{
    public CreateNotificationDtoValidator()
    {
        RuleFor(x => x.InstitutionId)
            .GreaterThan(0)
            .When(x => x.InstitutionId.HasValue)
            .WithMessage("Institution ID must be greater than 0.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required.");

        RuleFor(x => x.NotificationType)
            .NotEmpty()
            .WithMessage("Notification type is required.")
            .MaximumLength(50)
            .WithMessage("Notification type cannot exceed 50 characters.");

        RuleFor(x => x.Channel)
            .IsInEnum()
            .WithMessage("A valid notification channel is required.");

        RuleFor(x => x.ReferenceId)
            .GreaterThan(0)
            .When(x => x.ReferenceId.HasValue)
            .WithMessage("Reference ID must be greater than 0.");
    }
}