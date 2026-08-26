namespace AssignmentSubmissionManagementSystem.Application.DTOs.AssignmentAttachments;

public sealed class CreateAssignmentAttachmentDto
{
    public long InstitutionId { get; set; }

    public long AssignmentId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string? FileType { get; set; }

    public long? FileSize { get; set; }
}