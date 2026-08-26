namespace AssignmentSubmissionManagementSystem.Application.DTOs.ResubmissionRequests;

public sealed class CreateResubmissionRequestDto
{
    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public string Reason { get; set; } = string.Empty;
}