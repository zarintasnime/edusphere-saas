namespace AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;

public sealed class SubmissionAttachmentResponseDto
{
    public long AttachmentId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    public DateTime CreatedAt { get; set; }
}