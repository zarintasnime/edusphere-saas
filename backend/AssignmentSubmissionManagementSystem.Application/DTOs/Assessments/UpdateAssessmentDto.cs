namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;

public sealed class UpdateAssessmentDto
{
    public long? PolicyId { get; set; }

    public decimal MarksObtained { get; set; }

    public string? Feedback { get; set; }
}