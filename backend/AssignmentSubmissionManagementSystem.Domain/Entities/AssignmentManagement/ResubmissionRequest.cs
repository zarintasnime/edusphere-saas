using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class ResubmissionRequest : BaseEntity
{
    public long RequestId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }

    public Submission Submission { get; set; } = null!;
}