namespace AssignmentSubmissionManagementSystem.Application.DTOs.Submissions;

public sealed class CreateResubmissionDto
{
    public long InstitutionId { get; set; }

    public long AssignmentId { get; set; }

    public long StudentId { get; set; }

    public string? SubmissionText { get; set; }
}