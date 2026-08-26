namespace AssignmentSubmissionManagementSystem.Application.DTOs.AssignmentAttachments;

public sealed class AssignmentAttachmentResponseDto
{
    public long AttachmentId { get; set; }

    public long InstitutionId { get; set; }

    public long AssignmentId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string? FileType { get; set; }

    public long? FileSize { get; set; }

    public DateTime CreatedAt { get; set; }
}