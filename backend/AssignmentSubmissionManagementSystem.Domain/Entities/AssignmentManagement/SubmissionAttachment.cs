using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class SubmissionAttachment : BaseEntity
{
    public long AttachmentId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    public Submission Submission { get; set; } = null!;
}