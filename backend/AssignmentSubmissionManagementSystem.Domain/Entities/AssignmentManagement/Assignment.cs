using AssignmentSubmissionManagementSystem.Domain.Entities.Academic;
using AssignmentSubmissionManagementSystem.Domain.Enums;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.AssignmentManagement;

public class Assignment : BaseEntity
{
    public long AssignmentId { get; set; }

    public long InstitutionId { get; set; }

    public long TeacherId { get; set; }

    public long CourseSubjectId { get; set; }

    public long TeacherSubjectId { get; set; }

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


    public DateTime? UpdatedAt { get; set; }



    public TeacherProfile Teacher { get; set; } = null!;


    public CourseSubject CourseSubject { get; set; } = null!;


    public TeacherSubject TeacherSubject { get; set; } = null!;


    public AcademicYear AcademicYear { get; set; } = null!;
}