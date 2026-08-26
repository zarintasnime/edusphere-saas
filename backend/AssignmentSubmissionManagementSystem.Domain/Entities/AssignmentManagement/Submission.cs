using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class Submission : BaseEntity
{
    public long SubmissionId { get; set; }

    public long InstitutionId { get; set; }

    public long AssignmentId { get; set; }

    public long StudentId { get; set; }

    public int SubmissionVersion { get; set; } = 1;

    public string? SubmissionText { get; set; }

    public DateTime SubmittedAt { get; set; }

    public bool IsLateSubmission { get; set; }

    public bool IsLatestSubmission { get; set; } = true;

    public SubmissionStatus SubmissionStatus { get; set; }
        = SubmissionStatus.Submitted;

    public DateTime? UpdatedAt { get; set; }

    public Assignment Assignment { get; set; } = null!;

    public StudentProfile Student { get; set; } = null!;
    public ICollection<SubmissionAttachment> SubmissionAttachments { get; set; }
    = new List<SubmissionAttachment>();
}