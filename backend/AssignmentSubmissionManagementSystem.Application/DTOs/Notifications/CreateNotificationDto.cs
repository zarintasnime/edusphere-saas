using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Notifications;

public sealed class CreateNotificationDto
{
    public long? InstitutionId { get; set; }

    public long UserId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string NotificationType { get; set; } = string.Empty;

    public NotificationChannel Channel { get; set; }
        = NotificationChannel.InApp;

    public long? ReferenceId { get; set; }
}