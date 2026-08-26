using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.AuditLogs;

public sealed class CreateAuditLogDto
{
    public long? InstitutionId { get; set; }

    public long UserId { get; set; }

    public AuditAction Action { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public long EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }
}