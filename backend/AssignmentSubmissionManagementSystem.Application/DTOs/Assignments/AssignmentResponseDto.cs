using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Application.DTOs.Assignments;

public sealed class AssignmentResponseDto
{
    public long AssignmentId { get; set; }

    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public long CourseSubjectId { get; set; }

    public string CourseName { get; set; } = string.Empty;

    public string SubjectName { get; set; } = string.Empty;

    public long AcademicYearId { get; set; }

    public string AcademicYearName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal TotalMarks { get; set; }
    public long TeacherSubjectId { get; set; }

    public DateTime DueDate { get; set; }

    public bool AllowLateSubmission { get; set; }

    public DateTime? LateSubmissionDeadline { get; set; }

    public AssignmentStatus AssignmentStatus { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}