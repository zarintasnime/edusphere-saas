using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionApprovals;

public sealed class ResubmissionApprovalResponseDto
{
    public long ApprovalId { get; set; }

    public long InstitutionId { get; set; }

    public long RequestId { get; set; }

    public long TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public ApprovalStatus ApprovalStatus { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}