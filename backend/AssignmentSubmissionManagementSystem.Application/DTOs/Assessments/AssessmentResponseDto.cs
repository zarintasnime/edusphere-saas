namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assessments;

public sealed class AssessmentResponseDto
{
    public long AssessmentId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public long TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public long? PolicyId { get; set; }

    public string? PolicyName { get; set; }

    public decimal MarksObtained { get; set; }

    public int PenaltyPercentageApplied { get; set; }

    public decimal FinalMarks { get; set; }

    public string? Feedback { get; set; }

    public DateTime ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}