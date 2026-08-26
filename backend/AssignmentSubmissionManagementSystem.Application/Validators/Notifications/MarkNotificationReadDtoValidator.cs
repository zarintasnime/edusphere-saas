using AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;
using FluentValidation;

namespace AssignmentSubmissionManagementSystem.Application.Validators.Notifications;

public sealed class MarkNotificationReadDtoValidator
    : AbstractValidator<MarkNotificationReadDto>
{
    public MarkNotificationReadDtoValidator()
    {
        RuleFor(x => x.NotificationId)
            .GreaterThan(0)
            .WithMessage("Notification ID must be greater than 0.");
    }
}