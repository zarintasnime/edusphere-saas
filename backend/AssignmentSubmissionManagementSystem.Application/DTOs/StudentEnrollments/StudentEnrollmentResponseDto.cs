namespace AssignmentSubmissionManagementSystem.Application.DTOs.StudentEnrollments;

public sealed class StudentEnrollmentResponseDto
{
    public long EnrollmentId { get; set; }

    public long InstitutionId { get; set; }

    public long StudentId { get; set; }

    public string StudentCode { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public long AcademicYearId { get; set; }

    public string AcademicYearName { get; set; } = string.Empty;

    public string RollNumber { get; set; } = string.Empty;

    public DateOnly? EnrollmentDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}