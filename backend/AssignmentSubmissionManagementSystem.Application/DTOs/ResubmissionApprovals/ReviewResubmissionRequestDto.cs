using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionApprovals;

public sealed class ReviewResubmissionRequestDto
{
    public long InstitutionId { get; set; }

    public long RequestId { get; set; }

    public long TeacherId { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }

    public string? Remarks { get; set; }
}