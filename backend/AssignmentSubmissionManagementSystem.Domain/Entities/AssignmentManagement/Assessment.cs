using AssignmentSubmissionManagementSystem.Domain.Entities;
using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class Assessment : BaseEntity
{
    public long AssessmentId { get; set; }

    public long InstitutionId { get; set; }

    public long SubmissionId { get; set; }

    public long TeacherId { get; set; }

    public long? PolicyId { get; set; }

    public decimal MarksObtained { get; set; }

    public int PenaltyPercentageApplied { get; set; }

    public decimal FinalMarks { get; set; }

    public string? Feedback { get; set; }

    public DateTime ReviewedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Submission Submission { get; set; } = null!;

    public TeacherProfile Teacher { get; set; } = null!;

    public LateSubmissionPolicy? Policy { get; set; }
}