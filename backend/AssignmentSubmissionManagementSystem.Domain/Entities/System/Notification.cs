using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.System;

public class Notification : BaseEntity
{
    public long NotificationId { get; set; }

    public long? InstitutionId { get; set; }

    public long UserId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string NotificationType { get; set; } = string.Empty;

    public NotificationChannel Channel { get; set; }
        = NotificationChannel.InApp;

    public long? ReferenceId { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public Institution? Institution { get; set; }

    public User User { get; set; } = null!;
}