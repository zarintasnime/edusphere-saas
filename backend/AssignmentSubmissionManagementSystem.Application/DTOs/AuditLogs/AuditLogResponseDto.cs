using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;

public sealed class AuditLogResponseDto
{
    public long AuditLogId { get; set; }

    public long? InstitutionId { get; set; }

    public long UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public AuditAction Action { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public long EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public DateTime CreatedAt { get; set; }
}