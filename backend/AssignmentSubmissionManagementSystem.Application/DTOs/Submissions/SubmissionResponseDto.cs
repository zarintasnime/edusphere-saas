using AssignmentSubmissionManagementSystem.Domain.Enums;
using AssignmentSubmissionManagementSystem.Application.DTOs.SubmissionAttachments;


namespace AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;

public sealed class SubmissionResponseDto
{
    public long SubmissionId { get; set; }

    public long InstitutionId { get; set; }

    public long AssignmentId { get; set; }

    public string AssignmentTitle { get; set; } = string.Empty;

    public long StudentId { get; set; }

    public string StudentCode { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public int SubmissionVersion { get; set; }

    public string? SubmissionText { get; set; }

    public DateTime SubmittedAt { get; set; }

    public bool IsLateSubmission { get; set; }

    public bool IsLatestSubmission { get; set; }

    public SubmissionStatus SubmissionStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }


    // NEW
    public List<SubmissionAttachmentResponseDto> Attachments { get; set; }
        = new();
}