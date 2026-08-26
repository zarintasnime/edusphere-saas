using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class ResubmissionApproval : BaseEntity
{
    public long ApprovalId { get; set; }

    public long InstitutionId { get; set; }

    public long RequestId { get; set; }

    public long TeacherId { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }
        = ApprovalStatus.Pending;

    public string? Remarks { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ResubmissionRequest Request { get; set; } = null!;

    public TeacherProfile Teacher { get; set; } = null!;
}