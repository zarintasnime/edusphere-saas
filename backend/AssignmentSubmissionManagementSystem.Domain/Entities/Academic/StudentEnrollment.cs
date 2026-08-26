using AssignmentSubmissionManagementSystem.Domain.Entities;

namespace AssignmentSubmissionManagementSystem.Domain.Entities.Academic;

public class StudentEnrollment : BaseEntity
{
    public long EnrollmentId { get; set; }

    public long InstitutionId { get; set; }

    public long StudentId { get; set; }

    public long AcademicYearId { get; set; }

    public string RollNumber { get; set; } = string.Empty;

    public DateOnly? EnrollmentDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? UpdatedAt { get; set; }

    public StudentProfile Student { get; set; } = null!;

    public AcademicYear AcademicYear { get; set; } = null!;
}