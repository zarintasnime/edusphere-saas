using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Core;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.System;

public class AuditLog : BaseEntity
{
    public long AuditLogId { get; set; }

    public long? InstitutionId { get; set; }

    public long UserId { get; set; }

    public AuditAction Action { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public long EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public Institution? Institution { get; set; }

    public User User { get; set; } = null!;
}