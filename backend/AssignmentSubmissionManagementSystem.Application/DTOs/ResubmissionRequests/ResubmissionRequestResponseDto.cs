namespace AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionRequests;

public sealed class ResubmissionRequestResponseDto
{
    public long RequestId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}