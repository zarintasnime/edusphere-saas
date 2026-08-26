using AssignmentSubmissionManagementSystem.Domain.Enums;

public sealed class CreateAssignmentDto
{
    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public long TeacherSubjectId { get; set; }

    public long CourseSubjectId { get; set; }

    public long AcademicYearId { get; set; }


    public string Title { get; set; } = string.Empty;


    public string? Description { get; set; }


    public decimal TotalMarks { get; set; }


    public DateTime DueDate { get; set; }


    public bool AllowLateSubmission { get; set; }


    public DateTime? LateSubmissionDeadline { get; set; }


    public AssignmentStatus AssignmentStatus { get; set; }
        = AssignmentStatus.Draft;


    public bool IsActive { get; set; } = true;
}