namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;

public sealed class CreateAssessmentDto
{
    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public long TeacherId { get; set; }

    public long? PolicyId { get; set; }

    public decimal MarksObtained { get; set; }

    public string? Feedback { get; set; }
}