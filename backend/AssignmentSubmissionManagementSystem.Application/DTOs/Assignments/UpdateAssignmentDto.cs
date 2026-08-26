using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;

public sealed class UpdateAssignmentDto
{
    public long CourseSubjectId { get; set; }

    public long AcademicYearId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal TotalMarks { get; set; }
    public long TeacherSubjectId { get; set; }

    public DateTime DueDate { get; set; }

    public bool AllowLateSubmission { get; set; }

    public DateTime? LateSubmissionDeadline { get; set; }

    public AssignmentStatus AssignmentStatus { get; set; }

    public bool IsActive { get; set; }
}